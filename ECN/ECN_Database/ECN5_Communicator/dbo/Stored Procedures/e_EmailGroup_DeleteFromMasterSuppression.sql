--2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE  PROC [dbo].[e_EmailGroup_DeleteFromMasterSuppression] 
(
	@EmailID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN

	declare @msgroupID int
	select @msgroupID = GroupID from Groups with (NOLOCK) where CustomerID=@CustomerID and MasterSupression=1
 
	SET NOCOUNT ON

	DELETE FROM ECN_ACTIVITY.dbo.BlastActivityUnSubscribes 
	WHERE 
		EmailID = @EmailID 
		AND UnsubscribeCodeID = 2 
		AND Comments LIKE '%AUTO MASTERSUPPRESSED%'
		
	DELETE FROM ecn5_communicator.dbo.EmailActivityLog 
	WHERE 
		EmailID = @EmailID 
		AND ActionTypeCode = 'MASTSUP_UNSUB'
	
	DELETE FROM ecn5_communicator.dbo.emailgroups
	WHERE 
		EmailID = @EmailID
		AND GroupID = @msgroupID

	UPDATE 
		ecn5_communicator.dbo.emailgroups
	SET 
		SubscribeTypeCode = 'S', 
		LastChanged = GETDATE()
	WHERE 
		EmailID = @EmailID 
		AND SubscribeTypeCode = 'M'
	
	UPDATE 
		ecn5_communicator.dbo.Emails 
	SET
		bouncescore = 0,
		softbouncescore = 0
	WHERE 
		emailID = @EmailID
END
