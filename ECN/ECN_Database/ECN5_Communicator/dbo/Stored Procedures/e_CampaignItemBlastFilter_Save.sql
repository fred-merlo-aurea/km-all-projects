CREATE PROCEDURE [dbo].[e_CampaignItemBlastFilter_Save]
	@CampaignItemBlastFilterID int = null,
	@CampaignItemBlastID int = null,
	@CampaignItemTestBlastID int = null,
	@CampaignItemSuppressionID int = null,
	@SuppressionGroupID int = null,
	@IsDeleted bit,
	@SmartSegmentID int = null,
	@RefBlastIDs varchar(500) = null,
	@FilterID int = null
AS
	if @CampaignItemBlastFilterID is null
	BEGIN
		INSERT INTO CampaignItemBlastFilter(CampaignItemBlastID, CampaignItemTestBlastID, CampaignItemSuppressionID,SuppressionGroupID, SmartSegmentID, RefBlastIDs, FilterID, IsDeleted)
		VALUES(@CampaignItemBlastID, @CampaignItemTestBlastID, @CampaignItemSuppressionID,@SuppressionGroupID, @SmartSegmentID, @RefBlastIDs, @FilterID, 0)
		SELECT @@IDENTITY;
	END
	ELSE
	BEGIN
		UPDATE CampaignItemBlastFilter
		SET CampaignItemBlastID = @CampaignItemBlastID, CampaignItemTestBlastID = @CampaignItemTestBlastID, CampaignItemSuppressionID = @CampaignItemSuppressionID, SuppressionGroupID = @SuppressionGroupID, SmartSegmentID = @SmartSegmentID, RefBlastIDs = @RefBlastIDs, FilterID = @FilterID, IsDeleted = @IsDeleted
		WHERE CampaignItemBlastFilterID = @CampaignItemBlastFilterID and IsDeleted = 0
	END