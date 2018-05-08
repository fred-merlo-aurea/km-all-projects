CREATE PROCEDURE [dbo].[e_UserClientSecurityGroupMap_Select_UserID]
@UserID int
AS
	select * 
	from UserClientSecurityGroupMap with(nolock)
	where UserID = @UserID