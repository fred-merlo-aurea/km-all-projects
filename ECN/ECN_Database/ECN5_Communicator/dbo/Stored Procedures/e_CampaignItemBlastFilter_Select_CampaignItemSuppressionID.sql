CREATE PROCEDURE [dbo].[e_CampaignItemBlastFilter_Select_CampaignItemSuppressionID]
	@CampaignItemSuppressionID int
AS
	SELECT *
	FROM CampaignItemBlastFilter cibf with(nolock)
	WHERE cibf.CampaignItemSuppressionID = @CampaignItemSuppressionID and cibf.IsDeleted = 0
