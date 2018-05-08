CREATE PROCEDURE [dbo].[e_CampaignItemBlastFilter_Select_CampaignItemBlastID]
	@CampaignItemBlastID int
AS
	SELECT *
	FROM CampaignItemBlastFilter cibf with(nolock)
	WHERE cibf.CampaignItemBlastID = @CampaignItemBlastID and cibf.IsDeleted = 0
