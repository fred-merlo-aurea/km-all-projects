CREATE PROCEDURE [dbo].[e_DownloadTemplate_Save]
@DownloadTemplateID int,
@DownloadTemplateName varchar(50),
@BrandID int,
@PubID int,
@IsDeleted bit,
@CreatedUserID int,
@CreatedDate datetime,
@UpdatedUserID int,
@UpdatedDate datetime
AS
BEGIN

	SET NOCOUNT ON

	if (@DownloadTemplateID > 0)
		begin
			update DownloadTemplate 
				set DownloadTemplateName = @DownloadTemplateName, 
					BrandID = @BrandID, 
					PubID = @PubID, 
					IsDeleted = @IsDeleted,
					UpdatedUserID = @UpdatedUserID, 
					UpdatedDate = GETDATE()  
				where DownloadTemplateID = @DownloadTemplateID
			select @DownloadTemplateID;
		end
	else
		begin
			insert into DownloadTemplate (DownloadTemplateName, BrandID, PubID, IsDeleted, CreatedUserID, CreatedDate) values (@DownloadTemplateName, @BrandID, @PubID, @IsDeleted, @CreatedUserID, @CreatedDate)
		
			select @@IDENTITY;
		end
End