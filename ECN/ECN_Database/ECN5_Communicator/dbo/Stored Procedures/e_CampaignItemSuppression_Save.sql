CREATE  PROC [dbo].[e_CampaignItemSuppression_Save] 
(
	@CampaignItemSuppressionID int = NULL,
	@CampaignItemID int = NULL,
	@GroupID int = NULL,
	@UserID int = null
	
)
AS 
BEGIN
	IF @CampaignItemSuppressionID is NULL or @CampaignItemSuppressionID <= 0
	BEGIN
		INSERT INTO CampaignItemSuppression
		(
			CampaignItemID, GroupID, CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@CampaignItemID, @GroupID, @UserID,GetDate(),0
		)
		SET @CampaignItemSuppressionID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE CampaignItemSuppression
			SET CampaignItemID=@CampaignItemID, GroupID=@GroupID,UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			CampaignItemSuppressionID = @CampaignItemSuppressionID
	END

	SELECT @CampaignItemSuppressionID
END