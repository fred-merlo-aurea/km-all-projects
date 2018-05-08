CREATE PROCEDURE [dbo].[sp_PubSubscriptions_Save]
(
	@PubSubscriptionID	int,
	@EmailStatusID	int,
	@Email varchar(100)
)
AS
BEGIN

	SET NOCOUNT ON

	declare @OldEmailStatusID int
	
	Select @OldEmailStatusID = iSNULL(EmailStatusID,0) 
	from PubSubscriptions 
	where PubSubscriptionID = @PubSubscriptionID
		
	if (@EmailStatusID != @OldEmailStatusID) 	
		Begin
			update PubSubscriptions 
				set EmailStatusID = @EmailStatusID, StatusUpdatedDate =  GETDATE() 
				where  PubSubscriptionID = @PubSubscriptionID  
		end		
	
	update PubSubscriptions 
		set Email = @Email 
		where  PubSubscriptionID = @PubSubscriptionID 

END