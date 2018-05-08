CREATE  PROC [dbo].[e_CampaignItemBlast_Delete_All] 
(
	@CustomerID int,
    @CampaignItemID int,
    @UserID int
)
AS 
BEGIN
	UPDATE cib SET cib.IsDeleted = 1, cib.UpdatedDate = GETDATE(), cib.UpdatedUserID = @UserID
	FROM CampaignItemBlast cib WITH (NOLOCK) JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE cib.CampaignItemID = @CampaignItemID AND c.CustomerID = @CustomerID
END