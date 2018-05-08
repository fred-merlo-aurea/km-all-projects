CREATE  PROC [dbo].[e_CampaignItemOptOutGroup_Select_CampaignItemID] 
(
    @CampaignItemID int
)
AS 
BEGIN
	
	select 
		ciog.*, c.CustomerID 
	from 
		CampaignItemOptOutGroup ciog with (nolock)
		join CampaignItem ci with (nolock) on ciog.CampaignItemID = ci.CampaignItemID
		join Campaign c with (nolock) on ci.CampaignID= c.CampaignID
	where 
		ciog.CampaignItemID = @CampaignItemID and
		ciog.IsDeleted = 0 and 
		ci.IsDeleted = 0 and
		c.IsDeleted = 0
	
END