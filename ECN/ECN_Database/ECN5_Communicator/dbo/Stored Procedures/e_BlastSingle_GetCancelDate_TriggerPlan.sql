-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_BlastSingle_GetCancelDate_TriggerPlan] 
	@TriggerPlanID int,
	@BlastID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT top 1 bs.UpdatedDate as CancelDate from BlastSingles bs with(nolock) where bs.LayoutPlanID = @TriggerPlanID and bs.BlastID = @BlastID and bs.Processed = 'c'
END
