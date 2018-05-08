CREATE PROCEDURE [dbo].[e_RSSFeed_Select_Name_CustomerID]
	@Name varchar(100),
	@CustomerID int
AS
	Select top 1 * from RSSFeed r with(nolock) where r.CustomerID = @CustomerID and r.Name = @Name and r.IsDeleted = 0