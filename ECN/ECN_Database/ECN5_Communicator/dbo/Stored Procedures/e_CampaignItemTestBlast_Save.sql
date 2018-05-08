CREATE  PROC [dbo].[e_CampaignItemTestBlast_Save] 
(
	@CampaignItemTestBlastID int = NULL,
	@CampaignItemID int = NULL,
	@GroupID int = NULL,
	@HasEmailPreview bit = NULL,
	@BlastID int = NULL,
	@UserID int = NULL,
	@FilterID int = NULL,
	@CampaignItemTestBlastType varchar(50),
	@LayoutID int = NULL,
	@EmailSubject VARCHAR (255) = NULL,
	@FromName VARCHAR (255) = NULL,
    @FromEmail VARCHAR (100) = NULL,
    @ReplyTo VARCHAR (100) = NULL
)
AS 
BEGIN
	IF @CampaignItemTestBlastID is NULL or @CampaignItemTestBlastID <= 0
	BEGIN
		INSERT INTO CampaignItemTestBlast
		(
			CampaignItemID, GroupID, HasEmailPreview, BlastID, CreatedUserID,CreatedDate,IsDeleted, FilterID, CampaignItemTestBlastType,
			LayoutID, EmailSubject, FromName, FromEmail, ReplyTo
		)
		VALUES
		(
			@CampaignItemID, @GroupID, @HasEmailPreview, @BlastID, @UserID, GetDate(), 0, @FilterID, @CampaignItemTestBlastType,
			@LayoutID, @EmailSubject, @FromName, @FromEmail, @ReplyTo
		)
		SET @CampaignItemTestBlastID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE CampaignItemTestBlast
			SET CampaignItemID=@CampaignItemID, GroupID=@GroupID, HasEmailPreview=@HasEmailPreview,
				BlastID=@BlastID, UpdatedUserID=@UserID, UpdatedDate=GETDATE(), FilterID = @FilterID, CampaignItemTestBlastType = @CampaignItemTestBlastType,
				LayoutID = @LayoutID, EmailSubject = @EmailSubject, FromName = @FromName, FromEmail = @FromEmail, ReplyTo = @ReplyTo
		WHERE
			CampaignItemTestBlastID = @CampaignItemTestBlastID
	END

	SELECT @CampaignItemTestBlastID
END