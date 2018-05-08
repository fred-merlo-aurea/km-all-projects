CREATE PROCEDURE [dbo].[e_SimpleShareDetail_Save]
	@SimpleShareDetailID int = null,
	@SocialMediaID int,
	@SocialMediaAuthID int,
	@Title varchar(200),
	@SubTitle varchar(200),
	@ImagePath varchar(200),
	@Content varchar(200),
	@UseThumbnail bit,
	@IsDeleted bit,
	@UpdatedUserID int = null,
	@CreatedUserID int = null,
	@PageAccessToken varchar(MAX),
	@PageID varchar(MAX)
AS
	if(@SimpleShareDetailID is null)
	BEGIN
		INSERT INTO SimpleShareDetail(SocialMediaID, SocialMediaAuthID, Title, SubTitle, ImagePath, Content, UseThumbnail, IsDeleted, CreatedUserID, CreatedDate, PageAccessToken, PageID)
		VALuES(@SocialMediaID, @SocialMediaAuthID, @Title, @SubTitle, @ImagePath, @Content, @UseThumbnail, @IsDeleted, @CreatedUserID, GETDATE(), @PageAccessToken, @PageID)
		SELECT @@IDENTITY;
	END
	ELSE
	BEGIN
		UPDATE SimpleShareDetail
		SET Title = @Title, SubTitle = @SubTitle, ImagePath = @ImagePath, Content = @Content, UseThumbnail = @UseThumbnail, IsDeleted = @IsDeleted, UpdatedUserID = @UpdatedUserID, UpdatedDate = GETDATE(), PageAccessToken = @PageAccessToken, PageID = @PageID
		WHERE SimpleShareDetailID = @SimpleShareDetailID and IsDeleted = 0
		SELECT @SimpleShareDetailID
	END
	