CREATE PROCEDURE [dbo].[e_RSSFeed_Select_ID]
	@FeedID int
AS
	SELECT * FROM RSSFeed r with(nolock) where r.FeedId = @FeedID and r.IsDeleted = 0
