CREATE  PROC [dbo].[e_CampaignItemSuppression_Delete_Single] 
(
	@CustomerID int,
    @CampaignItemID int,
    @CampaignItemSuppressionID int,
    @UserID int
)
AS 
BEGIN
	UPDATE cis SET cis.IsDeleted = 1, cis.UpdatedDate = GETDATE(), cis.UpdatedUserID = @UserID
	FROM CampaignItemSuppression cis WITH (NOLOCK) JOIN CampaignItem ci WITH (NOLOCK) ON cis.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE cis.CampaignItemID = @CampaignItemID AND c.CustomerID = @CustomerID and cis.CampaignItemSuppressionID = @CampaignItemSuppressionID
END