CREATE  PROC [dbo].[e_CampaignItemTemplateSuppressionGroup_Save] 
(
	@CampaignItemTemplateID int,
	@GroupID int,
	@CreatedUserID int = NULL
)
AS 
BEGIN

	BEGIN
		INSERT INTO CampaignItemTemplateSuppressionGroup
		(
			CampaignItemTemplateID,GroupID, CreatedDate,IsDeleted,CreatedUserID
		)
		VALUES
		(
			@CampaignItemTemplateID,@GroupID,GETDATE(),0,@CreatedUserID
		)
	END
END