CREATE PROCEDURE [dbo].[e_CampaignItemSocialMedia_Delete_CampaignItemID]
	@CampaignItemID int,
	@SimpleOrSubscriber varchar(20)
AS
	if(@SimpleOrSubscriber = 'simple')
	BEGIN
		--This will delete only the simple share records for the campaign item
		DELETE FROM CampaignItemSocialMedia
		WHERE CampaignItemID = @CampaignItemID and SocialMediaID in (1,2,3) and SimpleShareDetailID is not null
	END
	ELSE
	BEGIN
		--This will delete all subscriber share records for the campaing item
		DELETE FROM CampaignItemSocialMedia
		WHERE CampaignItemID = @CampaignItemID and SimpleShareDetailID is null
	END