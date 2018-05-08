CREATE PROCEDURE [dbo].[e_CampaignItemSocialMedia_Select_CampaignItemSocialMediaID] 
	@CampaignItemSocialMediaID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT top 1 CISM.* from CampaignItemSocialMedia CISM with(nolock) where CISM.CampaignItemSocialMediaID  = @CampaignItemSocialMediaID 
END
GO
