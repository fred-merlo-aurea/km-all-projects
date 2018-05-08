CREATE PROCEDURE [dbo].e_CampaignItem_Select_SampleID   
@SampleID int,
@Type varchar(20)

AS
	SELECT ci.*, c.CustomerID 
	FROM CampaignItem ci WITH (NOLOCK)
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE ci.SampleID = @SampleID and ci.CampaignItemType=@Type and ci.IsDeleted = 0 and c.IsDeleted = 0