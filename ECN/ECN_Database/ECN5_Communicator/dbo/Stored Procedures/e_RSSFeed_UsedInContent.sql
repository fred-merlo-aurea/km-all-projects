CREATE PROCEDURE [dbo].[e_RSSFeed_UsedInContent]
	@RSSFeedName varchar(100),
	@CustomerID int
AS
	IF EXISTS(SELECT top 1 c.ContentID from Content c with(nolock) where c.CustomerID = @CustomerID and c.ContentSource LIKE '%ECN.RSSFEED.' + @RSSFeedName + '.ECN.RSSFEED%' and c.IsDeleted = 0)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
