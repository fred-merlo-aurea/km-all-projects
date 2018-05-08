CREATE PROCEDURE [dbo].[e_Application_Select_ApplicationID]
@ApplicationID int = NULL
AS
BEGIN
	SELECT * FROM [Application] WHERE ApplicationID = @ApplicationID
END
