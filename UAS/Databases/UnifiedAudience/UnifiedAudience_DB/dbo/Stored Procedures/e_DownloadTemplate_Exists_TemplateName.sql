CREATE PROCEDURE [dbo].[e_DownloadTemplate_Exists_TemplateName]
@DownloadTemplateID int, 
@DownloadTemplateName varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS (
		SELECT TOP 1 DownloadTemplateID
		FROM DownloadTemplate WITH (NOLOCK)
		WHERE DownloadTemplateName = @DownloadTemplateName AND 
			DownloadTemplateID <> @DownloadTemplateID AND 
			IsDeleted = 0
	) SELECT 1 ELSE SELECT 0

End