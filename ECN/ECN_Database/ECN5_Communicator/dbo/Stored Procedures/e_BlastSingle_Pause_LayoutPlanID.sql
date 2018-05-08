CREATE PROCEDURE [dbo].[e_BlastSingle_Pause_LayoutPlanID]
	@LayoutPlanID int,
	@IsPause bit
AS
if @IsPause = 1
BEGIN
	Update bs
	set Processed = 'p'
	from BlastSingles bs 
	join LayoutPlans lp with(nolock) on bs.LayoutPlanID = lp.LayoutPlanID and bs.BlastID = lp.BlastID
	where bs.LayoutPlanID = @LayoutPlanID and Processed = 'n'
END
Else if @IsPause = 0
BEGIN
	Update bs
	set Processed = 'n'
	from BlastSingles bs 
	join LayoutPlans lp with(nolock) on bs.LayoutPlanID = lp.LayoutPlanID and bs.BlastID = lp.BlastID
	where bs.LayoutPlanID = @LayoutPlanID and Processed = 'p'
END