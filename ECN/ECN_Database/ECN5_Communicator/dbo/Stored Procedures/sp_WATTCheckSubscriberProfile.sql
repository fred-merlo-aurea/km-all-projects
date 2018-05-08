CREATE PROCEDURE [dbo].[sp_WATTCheckSubscriberProfile] 
	@EmailAddress varchar(100), 
	@GroupID int	
AS
BEGIN	
	SELECT top 1 e.EmailID, eg.SubscribeTypeCode from Emails e join EmailGroups eg 
	on e.EmailID = eg.EmailID where eg.GroupID = @GroupID and e.EmailAddress = @EmailAddress	
END
