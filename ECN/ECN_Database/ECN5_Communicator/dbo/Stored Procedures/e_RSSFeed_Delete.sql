CREATE PROCEDURE [dbo].[e_RSSFeed_Delete]
	@RSSFeedID int,
    @UserID int
AS
	UPDATE RSSFeed
	SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE FeedId = @RSSFeedID
