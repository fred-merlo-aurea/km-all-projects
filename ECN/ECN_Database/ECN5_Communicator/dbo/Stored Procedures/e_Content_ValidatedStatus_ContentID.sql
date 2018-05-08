CREATE PROCEDURE [dbo].[e_Content_ValidatedStatus_ContentID]
@ContentID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 IsValidated FROM Content WITH (NOLOCK) WHERE ContentID = @ContentID AND IsDeleted = 0 AND IsValidated=1) SELECT 1 ELSE SELECT 0
END