CREATE PROCEDURE [dbo].[e_Rule_Select_CustomerID]   
@CustomerID int
AS


SELECT r.* FROM [Rule] r WITH (NOLOCK)
WHERE r.CustomerID = @CustomerID and r.IsDeleted = 0