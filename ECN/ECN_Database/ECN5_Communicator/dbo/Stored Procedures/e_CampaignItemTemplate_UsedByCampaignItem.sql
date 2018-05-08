CREATE PROCEDURE [dbo].[e_CampaignItemTemplate_UsedByCampaignItem]
	@TemplateID int
AS
	if exists (Select top 1 * from CampaignItem ci with(nolock) where ISNULL(ci.CampaignItemTemplateID, 0) = @TemplateID and ISNULL(ci.IsDeleted, 0) = 0)
	BEGIN
		Select 1
	END
	else
	BEGIN
		select 0
	END
