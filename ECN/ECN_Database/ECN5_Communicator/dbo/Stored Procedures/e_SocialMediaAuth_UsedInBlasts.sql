CREATE PROCEDURE [dbo].[e_SocialMediaAuth_UsedInBlasts]
	@SocialMediaAuthID int
AS
	SELECT COUNT(cism.CampaignItemSocialMediaID) 
	FROM CampaignItemSocialMedia cism with(nolock)
	join CampaignItemBlast cib with(nolock) on cism.CampaignItemID = cib.CampaignItemID
	join Blast b with(nolock) on cib.BlastID = b.BlastID
	where b.StatusCode in ('pending','active') and cism.SocialMediaAuthID = @SocialMediaAuthID and cism.SimpleShareDetailID is not null
	
	