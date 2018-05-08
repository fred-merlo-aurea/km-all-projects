CREATE PROCEDURE [dbo].[e_EmailPreview_Select_EmailTestID]
@EmailTestID int
AS
SELECT *
FROM EmailPreview WITH(NOLOCK)
WHERE EmailTestID = @EmailTestID
