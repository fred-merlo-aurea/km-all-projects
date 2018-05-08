CREATE PROCEDURE [dbo].[e_CampaignItemSocialMedia_Select_ByStatus]
	@Status varchar(10)
AS
	SELECT * 
	FROM CampaignItemSocialMedia cism with(nolock)
	WHERE cism.Status = @Status
	ORDER BY cism.StatusDate asc

