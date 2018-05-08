CREATE PROCEDURE [dbo].[e_TriggerPlans_Select_RefTriggerID]   
@RefTriggerID int = NULL
AS
	SELECT * FROM TriggerPlans WITH (NOLOCK) WHERE RefTriggerID = @RefTriggerID and IsDeleted = 0