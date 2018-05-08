CREATE  PROC [dbo].[e_CampaignItemBlast_Save] 
(
	@CampaignItemBlastID int = NULL,
	@CampaignItemID int = NULL,
	@EmailSubject varchar(255) = NULL,
	@DynamicFromName varchar(100) = NULL,
	@DynamicFromEmail varchar(100) = NULL,
	@DynamicReplyTo varchar(100) = NULL,
	@LayoutID int = NULL,
	@GroupID int = NULL,
	@SocialMediaID int = NULL,
	@FilterID int = NULL,
	@SmartSegmentID int = NULL,
	@BlastID int = NULL,
	@AddOptOuts_to_MS bit = NULL,
	@UserID int = NULL,
	@EmailFrom varchar(100) = null,
	@ReplyTo varchar(100) = null,
	@FromName varchar(100) = null
)
AS 
BEGIN
	IF @CampaignItemBlastID is NULL or @CampaignItemBlastID <= 0
	BEGIN
		INSERT INTO CampaignItemBlast
		(
			CampaignItemID, EmailSubject, DynamicFromName, DynamicFromEmail, DynamicReplyTo, LayoutID,
			GroupID, SocialMediaID, FilterID, SmartSegmentID, BlastID, AddOptOuts_to_MS, CreatedUserID,CreatedDate,IsDeleted, FromName, EmailFrom, ReplyTo
		)
		VALUES
		(
			@CampaignItemID, @EmailSubject, @DynamicFromName, @DynamicFromEmail, @DynamicReplyTo, @LayoutID,
			@GroupID, @SocialMediaID, @FilterID, @SmartSegmentID, @BlastID, @AddOptOuts_to_MS, @UserID,GetDate(),0, @FromName, @EmailFrom, @ReplyTo
		)
		SET @CampaignItemBlastID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE CampaignItemBlast
			SET CampaignItemID=@CampaignItemID, EmailSubject=@EmailSubject,DynamicFromName=@DynamicFromName,
			DynamicFromEmail=@DynamicFromEmail, DynamicReplyTo=@DynamicReplyTo,LayoutID=@LayoutID,
			GroupID=@GroupID, SocialMediaID=@SocialMediaID,FilterID=@FilterID,SmartSegmentID=@SmartSegmentID,BlastID=@BlastID,
			AddOptOuts_to_MS=@AddOptOuts_to_MS,UpdatedUserID=@UserID,UpdatedDate=GETDATE(), FromName = @FromName, EmailFrom = @EmailFrom, ReplyTo = @ReplyTo
		WHERE
			CampaignItemBlastID = @CampaignItemBlastID
	END

	SELECT @CampaignItemBlastID
END