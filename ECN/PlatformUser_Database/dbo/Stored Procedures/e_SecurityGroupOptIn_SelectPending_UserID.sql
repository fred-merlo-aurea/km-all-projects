
CREATE PROCEDURE [e_SecurityGroupOptIn_SelectPending_UserID]
	@UserID int
AS
BEGIN
	SELECT * 
	FROM SecurityGroupOptIn sgoi with(nolock)
	where sgoi.UserID = @UserID and ISNULL(sgoi.HasAccepted,0) = 0
END