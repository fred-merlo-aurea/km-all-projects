CREATE PROCEDURE [dbo].[e_CampaignItemTemplate_Select_CampaignItemTemplateID]
(
	@CampaignItemTemplateID int,    
	@ArchiveFilter varchar(20) = 'all'
)
AS 
BEGIN
	select * from CampaignItemTemplates where CampaignItemTemplateID=@CampaignItemTemplateID and ISNULL(IsDeleted,0)=0	
	and ISNULL(Archived,0) = case when @ArchiveFilter = 'archived' then 1 when @ArchiveFilter = 'all' then ISNULL(Archived,0) when @ArchiveFilter = 'active' then 0 END
	
END