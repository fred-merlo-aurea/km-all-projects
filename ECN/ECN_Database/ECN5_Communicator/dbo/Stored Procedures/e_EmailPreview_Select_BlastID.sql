CREATE PROCEDURE [dbo].[e_EmailPreview_Select_BlastID]
@BlastID int
AS
SELECT *
FROM EmailPreview WITH(NOLOCK)
WHERE BlastID = @BlastID
