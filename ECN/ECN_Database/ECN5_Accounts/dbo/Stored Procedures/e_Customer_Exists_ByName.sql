CREATE PROCEDURE [dbo].[e_Customer_Exists_ByName]
	@CustomerName varchar(255),
	@CustomerID int
AS
if exists (Select top 1 * from Customer c with(nolock) where c.CustomerName = @CustomerName and c.CustomerID != @CustomerID)
BEGIN
	SELECT 1
END
ELSE
BEGIN
	SELECT 0
END
