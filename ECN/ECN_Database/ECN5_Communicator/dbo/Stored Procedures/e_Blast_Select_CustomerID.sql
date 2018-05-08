CREATE PROCEDURE [dbo].[e_Blast_Select_CustomerID]
@CustomerID int
AS
SELECT *
FROM Blast WITH(NOLOCK)
WHERE CustomerID = @CustomerID and StatusCode <> 'Deleted'
