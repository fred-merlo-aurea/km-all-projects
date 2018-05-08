CREATE PROCEDURE [dbo].[e_Gateway_Select_CustomerID]
	@CustomerID int
AS
	Select * 
	FROM Gateway g with(nolock)
	where g.CustomerID = @CustomerID and g.IsDeleted = 0
