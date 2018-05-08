CREATE  PROC dbo.e_CampaignItemTemplateSuppressionGroup_Select_CampaignItemTemplateID 
(
    @CampaignItemTemplateID int
)
AS 
BEGIN
	
	select 
		citsg.*, cit.CustomerID 
	from 
		CampaignItemTemplateSuppressionGroup citsg with (nolock)
		join CampaignItemTemplates cit with (nolock) on cit.CampaignItemTemplateID = citsg.CampaignItemTemplateID
	where 
		citsg.CampaignItemTemplateID = @CampaignItemTemplateID and
		citsg.IsDeleted = 0 and 
		cit.IsDeleted = 0 	
END