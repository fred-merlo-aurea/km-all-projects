CREATE PROCEDURE [dbo].[e_DownloadTemplate_Select]
AS
BEGIN

	SET NOCOUNT ON

	select * 
	from DownloadTemplate with(nolock) 
	where IsDeleted = 0 
	order by DownloadTemplateName

End