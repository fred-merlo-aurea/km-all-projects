CREATE PROCEDURE [e_DataImportExport_Select]
as
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM DataImportExport With(NoLock)

END