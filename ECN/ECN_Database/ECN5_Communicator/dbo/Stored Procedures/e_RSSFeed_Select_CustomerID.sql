CREATE PROCEDURE [dbo].[e_RSSFeed_Select_CustomerID]
	@CustomerID int
AS
	SELECT * FROM RSSFeed r with(nolock)
	WHERE r.CustomerID = @CustomerID and r.IsDeleted = 0
