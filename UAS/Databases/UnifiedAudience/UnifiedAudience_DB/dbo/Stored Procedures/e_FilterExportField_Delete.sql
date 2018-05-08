create proc [dbo].[e_FilterExportField_Delete](
@FilterScheduleID int
)
as
BEGIN

	SET NOCOUNT ON

	delete 
	from FilterExportField 
	where FilterScheduleID = @FilterScheduleID

End