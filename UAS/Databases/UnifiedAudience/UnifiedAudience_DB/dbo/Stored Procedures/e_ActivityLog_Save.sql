--INSERT
create PROCEDURE [dbo].[e_ActivityLog_Save]
	@UserID	uniqueidentifier,
	@Activity   varchar(150), 
	@PageUrl    varchar(150),
	@SessionID  varchar(50) 

AS
BEGIN

	set nocount on

	INSERT INTO ActivityLog(UserId, Activity, PageUrl, ActivityDate, SessionID) 
	VALUES(@UserId, @Activity, @PageUrl, GETDATE(), @SessionID ) 

end