CREATE PROCEDURE [dbo].[e_DownloadTemplateDetails_Select]
AS
BEGIN

	SET NOCOUNT ON

	select * 
	from DownloadTemplateDetails with(nolock) 

End