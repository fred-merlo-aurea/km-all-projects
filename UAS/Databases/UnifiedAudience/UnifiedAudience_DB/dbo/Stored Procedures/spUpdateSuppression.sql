CREATE Proc [dbo].[spUpdateSuppression] 
(
	@subscriptionID int,
	@emailaddress varchar(100)
)
as
BEGIN
	
	SET NOCOUNT ON

	declare @mastersuppressedEmailstatusID int,
		@unsubscribeEmailstatusID int
			
	select @mastersuppressedEmailstatusID = emailstatusID 
	from EmailStatus 
	where Status = 'MasterSuppressed'

	select @unsubscribeEmailstatusID = emailstatusID 
	from EmailStatus 
	where Status = 'UnSubscribe'
			

	update  subscriptions
		set  email = '', emailexists = 0
		where subscriptionID = @subscriptionID
	
	update PubSubscriptions
		set EmailStatusID = @mastersuppressedEmailstatusID, StatusUpdatedDate = GETDATE()
		where subscriptionID = @subscriptionID and EmailStatusID <> @unsubscribeEmailstatusID

End