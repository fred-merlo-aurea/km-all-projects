CREATE PROCEDURE [dbo].[e_CampaignItemBlastFilter_Delete_CampaignItemBlastID]
	@CampaignItemBlastID int
AS
	Update CampaignItemBlastFilter
	Set IsDeleted = 1
	where CampaignItemBlastID = @CampaignItemBlastID
