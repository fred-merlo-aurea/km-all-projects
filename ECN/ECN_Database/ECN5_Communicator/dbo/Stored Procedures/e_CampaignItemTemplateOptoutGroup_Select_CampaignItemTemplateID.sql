﻿CREATE PROCEDURE [dbo].[e_CampaignItemTemplateOptoutGroup_Select_CampaignItemTemplateID]
	@CampaignItemTemplateID int
AS
	SELECT * 
	FROM CampaignItemTemplateOptOutGroup citg with(nolock) 
	WHERE citg.CampaignItemTemplateID = @CampaignItemTemplateID and ISNULL(citg.IsDeleted, 0) = 0