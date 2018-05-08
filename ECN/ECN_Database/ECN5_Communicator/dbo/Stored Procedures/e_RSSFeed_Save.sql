CREATE PROCEDURE [dbo].[e_RSSFeed_Save]
	@FeedID int = null,
    @CustomerID int,
    @Name varchar(50), 
    @URL varchar(500),
    @StoriesToShow int = 5,
    @IsDeleted bit,
    @UserID int
AS
    IF(@FeedID is not null)
    BEGIN
        UPDATE RSSFeed
        SET Name = @Name, URL = @URL, StoriesToShow = @StoriesToShow, IsDeleted = @IsDeleted, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
        WHERE FeedId = @FeedID
        SELECT @FeedID
    END
    ELSE
    BEGIN
        INSERT INTO RSSFeed(Name, URL, StoriesToShow, IsDeleted, CreatedDate, CreatedUserID, CustomerID)
        VALUES(@Name, @URL, @StoriesToShow, @IsDeleted, GETDATE(), @UserID, @CustomerID)
        SELECT @@IDENTITY;
    END
