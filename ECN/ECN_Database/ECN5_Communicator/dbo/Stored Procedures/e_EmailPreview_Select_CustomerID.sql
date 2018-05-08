CREATE PROCEDURE [dbo].[e_EmailPreview_Select_CustomerID]
@CustomerID int
AS
SELECT *
FROM EmailPreview WITH(NOLOCK)
WHERE CustomerID = @CustomerID
