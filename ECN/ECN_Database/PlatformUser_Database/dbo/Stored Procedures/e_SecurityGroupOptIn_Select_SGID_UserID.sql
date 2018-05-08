CREATE PROCEDURE [dbo].[e_SecurityGroupOptIn_Select_SGID_UserID]
	@SecurityGroupID int,
	@UserID int
AS
	Select * 
	FROM SecurityGroupOptIn sgoi with(nolock)
	WHERE sgoi.UserID = @UserID and sgoi.SecurityGroupID = @SecurityGroupID
