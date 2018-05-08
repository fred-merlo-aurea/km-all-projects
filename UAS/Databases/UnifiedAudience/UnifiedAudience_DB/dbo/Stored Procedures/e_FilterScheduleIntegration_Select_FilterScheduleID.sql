CREATE PROCEDURE [dbo].[e_FilterScheduleIntegration_Select_FilterScheduleID]
@FilterScheduleID int 
AS
BEGIN

	SET NOCOUNT ON

	select *
	from FilterScheduleIntegration  WITH (NOLOCK)
	where FilterScheduleID = @FilterScheduleID		

END