CREATE PROCEDURE [dbo].[sp_SSB_CreateConversation]
	@Destination sysname,
	@Source sysname,
	@Contract sysname,
	@MessageType sysname,
	@MessageBody XML
AS

/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/07/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create SP to manage SSB conversations and supporting table sturcture

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Nathan C. Hoialmen			ECN Phase III 			Initial Release


==============================================================================*/

DECLARE @dialog_handle UNIQUEIDENTIFIER

/*Get the conversation id.*/
SELECT @dialog_handle = dialog_handle
FROM dbo.SSB_Settings 
WHERE [Source] = @Source 
	AND [Destination] = @Destination 
	AND [Contract] = @Contract;

/*If there is no current handle, or the conversation has had 1000 messages 
sent on it, create a new conversation.*/
IF @dialog_handle IS NULL OR
	(SELECT send_sequence 
        FROM sys.conversation_endpoints 
        WHERE conversation_id = @dialog_handle) >= 1000
BEGIN
	BEGIN TRANSACTION
	/*If there is a conversation dialog handle signal the destination 
         code that the old conversation is dead.*/
	IF @dialog_handle IS NOT NULL
	BEGIN
		UPDATE dbo.SSB_Settings 
		SET dialog_handle = NULL
		WHERE [Source] = @Source
			AND [Destination] = @Destination 
			AND	[Contract] = @Contract;

		SEND ON CONVERSATION @dialog_handle
		MESSAGE TYPE EndOfConversation
		END CONVERSATION @dialog_handle WITH CLEANUP;
	END

	/*Setup the new conversation*/
	BEGIN DIALOG CONVERSATION @dialog_handle
	FROM SERVICE @Source
	TO SERVICE @Destination
	ON CONTRACT @Contract
	WITH ENCRYPTION=OFF;

	/*Log the new conversation ID*/
	UPDATE dbo.SSB_Settings 
		SET dialog_handle = @dialog_handle 
	WHERE [Source] = @Source
		AND [Destination] = @Destination 
		AND [Contract] = @Contract;

	IF @@ROWCOUNT = 0
		INSERT INTO dbo.SSB_Settings 
		([Source], [Destination], [Contract], [dialog_handle])
		VALUES
		(@Source, @Destination, @Contract, @dialog_handle);
		
	COMMIT TRANSACTION
END;

/*Send the message*/
SEND ON CONVERSATION @dialog_handle
MESSAGE TYPE @MessageType
(@MessageBody);

/*Verify that the conversation handle is still the one logged in the table. 
  If not then mark this conversation as done.*/
IF (SELECT dialog_handle 
    FROM dbo.SSB_Settings  
    WHERE [Source] = @Source 
        AND [Destination] = @Destination 
        AND [Contract] = @Contract) <> @dialog_handle 
    SEND ON CONVERSATION @dialog_handle
        MESSAGE TYPE EndOfConversation;
