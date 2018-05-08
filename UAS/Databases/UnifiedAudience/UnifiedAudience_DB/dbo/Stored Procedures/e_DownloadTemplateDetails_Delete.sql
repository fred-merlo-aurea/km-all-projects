CREATE PROCEDURE [dbo].[e_DownloadTemplateDetails_Delete]
@DownloadTemplateID int 
AS
BEGIN

	SET NOCOUNT ON

	delete 
	from DownloadTemplateDetails 
	where DownloadTemplateID = @DownloadTemplateID

End