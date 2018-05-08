CREATE PROCEDURE [dbo].[e_ReportGroups_Select]
AS
	SELECT * FROM ReportGroups With(NoLock)
