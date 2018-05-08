CREATE PROCEDURE [dbo].[e_BlastPlans_Select_BlastPlanID]   
@BlastPlanID int
AS
	SELECT * FROM BlastPlans WITH (NOLOCK) WHERE BlastPlanID = @BlastPlanID and IsDeleted = 0