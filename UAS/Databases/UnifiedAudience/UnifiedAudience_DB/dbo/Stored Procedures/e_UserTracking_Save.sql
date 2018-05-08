CREATE proc [dbo].[e_UserTracking_Save](
@UserID int, 
@Activity varchar(50),
@IPAddress varchar(100),
@BrowserInfo varchar(2048)
)
as
BEGIN
	
	SET NOCOUNT ON
	
	
	insert into UserTracking (
		UserID, 
		Activity, 
		ActivityDateTime, 
		IPAddress, 
		BrowserInfo, 
		PlatformID,
		ClientID) 
		values (
		@UserID, 
		@Activity, 
		GETDATE(), 
		@IPAddress, 
		@BrowserInfo,
		dbo.[fn_GetPlatformID] (@BrowserInfo),
		dbo.[fn_GetClientID] (@BrowserInfo))
	select @@IDENTITY

End
GO