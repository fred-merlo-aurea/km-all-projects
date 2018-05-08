CREATE PROCEDURE [dbo].[e_FilterScheduleLog_Select]
AS
BEGIN
	SET NOCOUNT ON

	SELECT * FROM FilterScheduleLog With(NoLock)

END
