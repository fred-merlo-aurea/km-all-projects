CREATE  PROC [dbo].[e_CampaignItemBlastRefBlast_Delete_Single] 
(
	@CustomerID int,
    @CampaignItemBlastID int,
    @CampaignItemBlastRefBlastID int,
    @UserID int
)
AS 
BEGIN
	UPDATE cibrb SET cibrb.IsDeleted = 1, cibrb.UpdatedDate = GETDATE(), cibrb.UpdatedUserID = @UserID
	FROM CampaignItemBlastRefBlast cibrb
		JOIN CampaignItemBlast cib WITH (NOLOCK) ON cibrb.CampaignItemBlastID = cib.CampaignItemBlastID
		JOIN CampaignItem ci WITH (NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE cibrb.CampaignItemBlastID = @CampaignItemBlastID AND cibrb.CampaignItemBlastRefBlastID = @CampaignItemBlastRefBlastID AND c.CustomerID = @CustomerID
END