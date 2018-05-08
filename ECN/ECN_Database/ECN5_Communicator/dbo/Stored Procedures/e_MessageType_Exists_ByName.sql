CREATE PROCEDURE [dbo].[e_MessageType_Exists_ByName] 
	@MessageTypeID int = NULL,
	@BaseChannelID int = NULL,
	@Name varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 MessageTypeID FROM MessageType WITH (NOLOCK) WHERE BaseChannelID = @BaseChannelID AND MessageTypeID != ISNULL(@MessageTypeID, -1) AND Name = @Name AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END