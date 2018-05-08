CREATE PROCEDURE [dbo].[e_GroupConfig_Select_CustomerID]
@CustomerID int
AS
SELECT *
FROM GroupConfig WITH(NOLOCK)
WHERE CustomerID = @CustomerID and IsDeleted=0