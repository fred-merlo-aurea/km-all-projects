CREATE PROCEDURE [dbo].[e_RSSFeed_Exists_Name]
	@Name varchar(100),
	@CustomerID int,
	@FeedID int
AS
	IF EXISTS(Select top 1 * from RSSFeed r with(nolock) where r.CustomerID = @CustomerID and r.Name = @Name and r.FeedId <> ISNULL(@FeedID, 0) and r.IsDeleted = 0)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
