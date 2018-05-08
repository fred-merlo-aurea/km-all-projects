CREATE PROCEDURE [dbo].[e_UserClientSecurityGroupMap_SelectForUserAuthorization_UserID]
@UserID int
AS
	select * 
	from UserClientSecurityGroupMap with(nolock)
	where UserID = @UserID
	and IsActive = 'true'
GO
