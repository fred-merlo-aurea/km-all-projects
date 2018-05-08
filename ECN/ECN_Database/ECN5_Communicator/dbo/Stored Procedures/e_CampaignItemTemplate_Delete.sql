CREATE  PROC [dbo].[e_CampaignItemTemplate_Delete]
(
	@CampaignItemTemplateID int,
    @UserID int
)
AS 
BEGIN
	update CampaignItemTemplates set Isdeleted=1 , UpdatedUserID=@UserID where CampaignItemTemplateID=@CampaignItemTemplateID
END