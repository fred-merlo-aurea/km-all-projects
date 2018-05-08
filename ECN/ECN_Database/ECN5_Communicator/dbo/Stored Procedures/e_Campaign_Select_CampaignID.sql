CREATE PROCEDURE [dbo].[e_Campaign_Select_CampaignID]   
@CampaignID int
AS
	SELECT * FROM Campaign WITH (NOLOCK) WHERE CampaignID = @CampaignID and IsDeleted = 0