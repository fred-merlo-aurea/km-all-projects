CREATE  PROC [dbo].[e_CampaignItemTestBlast_UpdateBlastID] 
(
	@CampaignItemTestBlastID int,
	@BlastID int,
    @UserID int
)
AS 
BEGIN
	UPDATE CampaignItemTestBlast set UpdatedUserID = @UserID, UpdatedDate = GETDATE(), BlastID = @BlastID WHERE CampaignItemTestBlastID = @CampaignItemTestBlastID

END
