CREATE PROCEDURE [dbo].[e_BlastSingle_Delete_NoOpenFromAbandon_EmailID]
	@EmailID int,
	@LayoutPlanID int,
	@UpdatedUserID int
AS
	declare @TriggerPlanID int

	select @TriggerPlanID = ISNULL(tp.TriggerPlanID,-1) from BlastSingles bs with(nolock)
	join TriggerPlans tp with(nolock) on bs.BlastID = tp.RefTriggerID
	where bs.LayoutPlanID = @LayoutPlanID and bs.EmailID = @EmailID

	if @TriggerPlanID is not null and @TriggerPlanID > 0
	BEGIN
		update BlastSingles 
		set Processed = 'c', UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
		where LayoutPlanID = @TriggerPlanID and EmailID = @EmailID and Processed = 'n'
	END