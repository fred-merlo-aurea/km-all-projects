CREATE  PROC [dbo].[e_CampaignItem_Save] 
(
	@CampaignItemID int = NULL,
	@CampaignID int = NULL,
	@CampaignItemName varchar(255) = NULL,
	@CampaignItemType varchar(20) = NULL,
	@FromName varchar(255) = NULL,
	@FromEmail varchar(100) = NULL,
	@ReplyTo varchar(100) = NULL,
	@SendTime datetime = null,
	@PageWatchID int = NULL,
	@SampleID int = NULL,
	@BlastScheduleID int = null,
	@OverrideAmount int = null,
	@OverrideIsAmount bit = null,
	@BlastField1 varchar(255) = NULL,
	@BlastField2 varchar(255) = NULL,
	@BlastField3 varchar(255) = NULL,
	@BlastField4 varchar(255) = NULL,
	@BlastField5 varchar(255) = NULL,
	@CompletedStep int = NULL,
	@UserID int = NULL,
	@CampaignItemFormatType varchar(10) = NULL,
	@IsHidden bit = NULL,
	@CampaignItemNameOriginal varchar(255) = NULL,
	@CampaignItemIDOriginal int = NULL,
	@NodeID varchar(100) = NULL,
	@CampaignItemTemplateID int = NULL,
	@SFCampaignID varchar(500) = null,
	@EnableCacheBuster bit = null,
	@IgnoreSuppression bit = null
)
AS 
BEGIN
	IF @CampaignItemID is NULL or @CampaignItemID <= 0
	BEGIN
		INSERT INTO CampaignItem
		(
			CampaignID, CampaignItemName, CampaignItemType,
			FromName, FromEmail, ReplyTo, SendTime, PageWatchID, SampleID, BlastScheduleID, OverrideAmount, OverrideIsAmount,
			BlastField1, BlastField2, BlastField3, BlastField4, BlastField5, CampaignItemFormatType,
			CompletedStep, CreatedUserID, CreatedDate,IsDeleted, IsHidden, CampaignItemNameOriginal, CampaignItemIDOriginal, NodeID,
			CampaignItemTemplateID, SFCampaignID, EnableCacheBuster, IgnoreSuppression
		)
		VALUES
		(
			@CampaignID, @CampaignItemName, @CampaignItemType,
			@FromName, @FromEmail, @ReplyTo, @SendTime, @PageWatchID, @SampleID, @BlastScheduleID, @OverrideAmount, @OverrideIsAmount,
			@BlastField1, @BlastField2, @BlastField3, @BlastField4, @BlastField5, @CampaignItemFormatType, 
			@CompletedStep, @UserID, GETDATE(),0, @IsHidden, @CampaignItemNameOriginal, @CampaignItemIDOriginal,@NodeID,
			@CampaignItemTemplateID, @SFCampaignID, @EnableCacheBuster, @IgnoreSuppression
		)
		SET @CampaignItemID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE CampaignItem
			SET CampaignID=@CampaignID, CampaignItemName=@CampaignItemName, 
				CampaignItemType=@CampaignItemType, 
				FromName=@FromName,FromEmail=@FromEmail, ReplyTo=@ReplyTo, SendTime=@SendTime, 
				PageWatchID=@PageWatchID, SampleID=@SampleID, BlastScheduleID=@BlastScheduleID, OverrideAmount=@OverrideAmount,
				OverrideIsAmount=@OverrideIsAmount, BlastField1=@BlastField1, BlastField2=@BlastField2, CampaignItemFormatType = @CampaignItemFormatType,
				BlastField3=@BlastField3, BlastField4=@BlastField4, BlastField5=@BlastField5, CompletedStep=@CompletedStep,
				UpdatedUserID=@UserID, UpdatedDate=GETDATE(), IsHidden=@IsHidden, CampaignItemNameOriginal=@CampaignItemNameOriginal,
				CampaignItemIDOriginal=@CampaignItemIDOriginal, NodeID= @NodeID, CampaignItemTemplateID=@CampaignItemTemplateID, SFCampaignID=@SFCampaignID,
				EnableCacheBuster = @EnableCacheBuster, IgnoreSuppression = @IgnoreSuppression
		WHERE
			CampaignItemID = @CampaignItemID
	END

	SELECT @CampaignItemID
END