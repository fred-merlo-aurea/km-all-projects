CREATE  PROC [dbo].[e_CampaignItemTestBlast_Delete_All] 
(
	@CustomerID int,
    @CampaignItemID int,
    @UserID int 
)
AS 
BEGIN
	UPDATE citb SET citb.IsDeleted = 1, citb.UpdatedDate = GETDATE(), citb.UpdatedUserID = @UserID
	FROM CampaignItemTestBlast citb WITH (NOLOCK) JOIN CampaignItem ci WITH (NOLOCK) ON citb.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE citb.CampaignItemID = @CampaignItemID AND c.CustomerID = @CustomerID
END