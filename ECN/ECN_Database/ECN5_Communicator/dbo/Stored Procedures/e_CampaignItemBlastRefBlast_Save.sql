CREATE  PROC [dbo].[e_CampaignItemBlastRefBlast_Save] 
(
	@CampaignItemBlastRefBlastID int = NULL,
	@CampaignItemBlastID int = NULL,
	@RefBlastID int = NULL,
	@UserID int = null
)
AS 
BEGIN
	IF @CampaignItemBlastRefBlastID is NULL or @CampaignItemBlastRefBlastID <= 0
	BEGIN
		INSERT INTO CampaignItemBlastRefBlast
		(
			CampaignItemBlastID, RefBlastID, CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@CampaignItemBlastID, @RefBlastID, @UserID,GetDate(),0
		)
		SET @CampaignItemBlastRefBlastID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE CampaignItemBlastRefBlast
			SET CampaignItemBlastID=@CampaignItemBlastID, RefBlastID=@RefBlastID, UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			CampaignItemBlastRefBlastID = @CampaignItemBlastRefBlastID
	END

	SELECT @CampaignItemBlastRefBlastID
END