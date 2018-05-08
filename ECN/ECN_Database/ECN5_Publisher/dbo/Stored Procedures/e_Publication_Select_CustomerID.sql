CREATE PROCEDURE [dbo].[e_Publication_Select_CustomerID]   
@CustomerID int
AS
	select * from Publication where CustomerID = @CustomerID and IsDeleted=0