CREATE PROCEDURE [dbo].[e_UserGroups_Select_Single]  	
	@UserID int,
	@GroupID int
AS     
BEGIN     
	Select ug.*, g.CustomerID
	FROM UserGroups ug with (nolock)
		--join [ECN5_ACCOUNTS].[DBO].[USERS] u on ug.UserID = u.UserID
		join [Groups] g WITH (NOLOCK) on ug.GroupID = g.GroupID
	WHERE g.GroupID=@GroupID and  ug.UserID = @UserID and ug.IsDeleted = 0-- and u.IsDeleted = 0
	
END