CREATE  PROC [dbo].[e_CampaignItemOptOutGroup_Save] 
(
	@CampaignItemID int,
	@GroupID int,
	@CreatedUserID int = NULL
)
AS 
BEGIN
	DECLARE @exist int
	select @exist = IsNull(COUNT(CampaignItemID),0) FROM CampaignItemOptOutGroup with (nolock) WHERE CampaignItemID = @CampaignItemID and GroupID = @GroupID and IsDeleted = 0

	IF @exist <= 0
	BEGIN
		INSERT INTO CampaignItemOptOutGroup
		(
			CampaignItemID,GroupID, CreatedDate,IsDeleted,CreatedUserID
		)
		VALUES
		(
			@CampaignItemID,@GroupID,GETDATE(),0,@CreatedUserID
		)
	END
END