CREATE PROCEDURE [dbo].[e_ApplicationUser_Delete]
@UserID int = NULL,
@ApplicationID int = NULL
AS
BEGIN
	IF @UserID != NULL AND @ApplicationID != NULL
	BEGIN
		DELETE ApplicationUser WHERE ApplicationID=@ApplicationID AND UserID = @UserID
	END
	ELSE IF @UserID != NULL
	BEGIN
		DELETE ApplicationUser WHERE UserID = @UserID
	END
	ELSE IF @ApplicationID != NULL
	BEGIN
		DELETE ApplicationUser WHERE ApplicationID = @ApplicationID
	END
END
