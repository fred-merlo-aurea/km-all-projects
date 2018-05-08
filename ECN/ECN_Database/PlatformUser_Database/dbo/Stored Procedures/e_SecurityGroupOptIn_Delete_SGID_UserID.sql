CREATE PROCEDURE [dbo].[e_SecurityGroupOptIn_Delete_SGID_UserID]
	@SecurityGroupID int,
	@UserID int
AS
	DELETE FROM SecurityGroupOptIn
	WHERE SecurityGroupID = @SecurityGroupID and UserID = @UserID and HasAccepted = 0
