CREATE PROCEDURE [dbo].[DeleteEmail] (
	@emailID int -- NOT NULL
) AS
SET NOCOUNT ON
DELETE EmailGroups
WHERE EmailID = @emailID
DELETE EmailDataValues
WHERE EmailID = @emailID
DELETE Emails
WHERE EmailID = @emailID
RETURN 0
