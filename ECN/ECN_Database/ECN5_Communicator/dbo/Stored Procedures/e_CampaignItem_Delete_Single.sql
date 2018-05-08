CREATE  PROC [dbo].[e_CampaignItem_Delete_Single] 
(
	@CustomerID int,
    @CampaignID int,
    @CampaignItemID int,
    @UserID int
)
AS 
BEGIN
	UPDATE ci SET ci.IsDeleted = 1, ci.UpdatedDate = GETDATE(), ci.UpdatedUserID = @UserID
	FROM CampaignItem ci
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE ci.CampaignID = @CampaignID  AND ci.CampaignItemID = @CampaignItemID AND c.CustomerID = @CustomerID
END