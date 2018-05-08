CREATE  PROC [dbo].[e_CampaignItem_UpdateIsHidden] 
(
	@CampaignItemID int,
	@IsHidden bit,
    @UserID int
)
AS 
BEGIN
	UPDATE CampaignItem set UpdatedUserID = @UserID, UpdatedDate = GETDATE(), IsHidden = @IsHidden WHERE CampaignItemID = @CampaignItemID

END