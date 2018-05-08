CREATE PROCEDURE [dbo].[e_EmailDirect_Select_CustomerID]
	@CustomerID int
AS
	Select * From EmailDirect ed with(nolock) where ed.CustomerID = @CustomerID
