CREATE  PROC [dbo].[e_CampaignItem_Delete_All] 
(
	@CustomerID int,
    @CampaignID int,
    @UserID int
)
AS 
BEGIN
	UPDATE ci SET ci.IsDeleted = 1, ci.UpdatedDate = GETDATE(), ci.UpdatedUserID = @UserID
	FROM CampaignItem ci
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE ci.CampaignID = @CampaignID  AND c.CustomerID = @CustomerID
END