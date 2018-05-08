CREATE PROCEDURE [dbo].[e_TriggerPlans_Select_TriggerPlanID]   
@TriggerPlanID int = NULL
AS
	SELECT * FROM TriggerPlans WITH (NOLOCK) WHERE TriggerPlanID = @TriggerPlanID and IsDeleted = 0