CREATE PROCEDURE [dbo].[e_TriggerPlans_Exist]
	@TriggerPlanID int,
	@CustomerID int
AS
BEGIN
	if not exists(SELECT Top 1 * from TriggerPlans tp with(nolock) where tp.TriggerPlanID = @TriggerPlanID and tp.CustomerID = @CustomerID and tp.IsDeleted = 0)
	begin
		select 0
	end
	else
	begin
		select 1
	end
	
END