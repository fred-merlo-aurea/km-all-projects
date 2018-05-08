CREATE PROCEDURE [dbo].[e_ApplicationUser_Insert]
@UserID int = NULL,
@ApplicationID int = NULL
AS
BEGIN

	INSERT INTO ApplicationUser
	(
		UserID, ApplicationID
	)
	VALUES
	(
		@UserID, @ApplicationID
	)

END
