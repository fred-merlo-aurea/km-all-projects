CREATE PROCEDURE [dbo].[e_ExportType_Select]
AS
BEGIN

	SET NOCOUNT ON

	Select * 
	from ExportType WITH (NOLOCK)
	order by [type]
	
END