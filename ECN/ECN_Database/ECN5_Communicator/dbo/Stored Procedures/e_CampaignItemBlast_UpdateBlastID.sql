CREATE  PROC [dbo].[e_CampaignItemBlast_UpdateBlastID] 
(
	@CampaignItemBlastID int,
	@BlastID int,
    @UserID int
)
AS 
BEGIN
	UPDATE CampaignItemBlast set UpdatedUserID = @UserID, UpdatedDate = GETDATE(), BlastID = @BlastID WHERE CampaignItemBlastID = @CampaignItemBlastID

END
