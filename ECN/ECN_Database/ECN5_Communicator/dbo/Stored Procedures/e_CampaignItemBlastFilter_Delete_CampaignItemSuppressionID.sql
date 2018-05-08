CREATE PROCEDURE [dbo].[e_CampaignItemBlastFilter_Delete_CampaignItemSuppressionID]
	@CampaignItemSuppressionID int
AS
	Update CampaignItemBlastFilter
	SET IsDeleted = 1
	where CampaignItemSuppressionID = @CampaignItemSuppressionID