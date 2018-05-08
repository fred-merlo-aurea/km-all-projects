CREATE PROCEDURE [dbo].[e_BlastSingle_Delete_TriggerPlan]
	@TriggerPlanID int,
	@BlastID int,
	@UpdatedUserID int
AS
	UPDATE BlastSingles
	SET Processed = 'c',UpdatedUserID=@UpdatedUserID,UpdatedDate=GETDATE()
	WHERE BlastID = @BlastID and LayoutPlanID = @TriggerPlanID and Processed = 'n'