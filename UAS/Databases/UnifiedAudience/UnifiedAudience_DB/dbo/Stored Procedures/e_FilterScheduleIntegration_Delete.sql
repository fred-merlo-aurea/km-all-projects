CREATE PROCEDURE [dbo].[e_FilterScheduleIntegration_Delete]
@FilterScheduleID int 
AS
BEGIN

	SET NOCOUNT ON

	delete 
	from FilterScheduleIntegration 
	where FilterScheduleID = @FilterScheduleID

END