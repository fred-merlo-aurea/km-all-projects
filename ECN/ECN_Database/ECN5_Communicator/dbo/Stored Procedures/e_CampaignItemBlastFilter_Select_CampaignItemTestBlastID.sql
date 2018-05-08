CREATE PROCEDURE [dbo].[e_CampaignItemBlastFilter_Select_CampaignItemTestBlastID]
	@CampaignItemTestBlastID int
AS
	SELECT *
	FROM CampaignItemBlastFilter cibf with(nolock)
	where cibf.CampaignItemTestBlastID = @CampaignItemTestBlastID and cibf.IsDeleted = 0