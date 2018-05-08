CREATE  PROC dbo.e_SocialShareDetail_Save 
(
	@SocialShareDetailID int = NULL,
    @ContentID int = NULL,
    @Title varchar(50) = NULL,
    @Description varchar(50) = NULL,
    @Image varchar(250) = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @SocialShareDetailID is NULL or @SocialShareDetailID <= 0
	BEGIN
		INSERT INTO SocialShareDetail
		(
			ContentID, Title, [Description], [Image],  CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@ContentID, @Title, @Description, @Image,  @UserID, GetDate(), 0
		)
		SET @SocialShareDetailID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE SocialShareDetail
			SET Title=@Title,[Description]=@Description,[Image]=@Image, UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			SocialShareDetailID = @SocialShareDetailID and ContentID=@ContentID
	END

	SELECT @SocialShareDetailID
END