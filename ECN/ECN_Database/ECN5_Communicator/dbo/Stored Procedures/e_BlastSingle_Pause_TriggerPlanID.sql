CREATE PROCEDURE [dbo].[e_BlastSingle_Pause_TriggerPlanID]
	@TriggerPlanID int,
	@IsPause bit
AS
if @IsPause = 1
BEGIN
	Update bs	
	set Processed = 'p'
	from BlastSingles bs
	join TriggerPlans tp with(nolock) on bs.LayoutPlanID = tp.TriggerPlanID and bs.BlastID = tp.BlastID
	where LayoutPlanID = @TriggerPlanID and Processed = 'n'
END
Else if @IsPause = 0
BEGIN
	Update bs
	set Processed = 'n'
	from BlastSingles bs
	join TriggerPlans tp with(nolock) on bs.LayoutPlanID = tp.TriggerPlanID and bs.BlastID = tp.BlastID
	where LayoutPlanID = @TriggerPlanID and Processed = 'p'
END
