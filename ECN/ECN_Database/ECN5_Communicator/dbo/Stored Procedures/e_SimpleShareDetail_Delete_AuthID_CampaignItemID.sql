CREATE PROCEDURE [dbo].[e_SimpleShareDetail_Delete_AuthID_CampaignItemID]
	@SocialMediaAuthID int,
	@CampaignItemID int
AS
	DELETE FROM SimpleShareDetail
	WHERE CampaignItemID = @CampaignItemID and SocialMediaAuthID = @SocialMediaAuthID
