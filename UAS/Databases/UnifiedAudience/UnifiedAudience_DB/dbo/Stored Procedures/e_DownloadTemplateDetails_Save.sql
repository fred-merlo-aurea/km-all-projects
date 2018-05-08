CREATE PROCEDURE [dbo].[e_DownloadTemplateDetails_Save]
@DownloadTemplateID int, 
@ExportColumn varchar(100),
@IsDescription bit,
@FieldCase varchar(100)
AS
BEGIN

	SET NOCOUNT ON

    insert into DownloadTemplateDetails (DownloadTemplateID, ExportColumn, IsDescription, FieldCase) 
	values (@DownloadTemplateID, @ExportColumn, @IsDescription, @FieldCase)

End