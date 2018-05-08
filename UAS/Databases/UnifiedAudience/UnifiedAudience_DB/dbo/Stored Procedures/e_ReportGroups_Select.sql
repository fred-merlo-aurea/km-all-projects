CREATE PROCEDURE [dbo].[e_ReportGroups_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM ReportGroups With(NoLock)

END