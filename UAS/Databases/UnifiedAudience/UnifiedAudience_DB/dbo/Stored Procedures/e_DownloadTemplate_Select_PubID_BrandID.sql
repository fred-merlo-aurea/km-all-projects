CREATE PROCEDURE [dbo].[e_DownloadTemplate_Select_PubID_BrandID]
@PubID int,
@BrandID int
AS
BEGIN

	SET NOCOUNT ON

	if(@PubID > 0)
		begin
			if(@BrandID > 0)
				begin
					select * 
					from DownloadTemplate with(nolock) 
					where IsDeleted = 0 and BrandID  = @BrandID and PubID = @PubID
					order by DownloadTemplateName
				end
			else
				begin
					select * 
					from DownloadTemplate with(nolock) 
					where IsDeleted = 0 and BrandID = 0 and PubID = @PubID
					order by DownloadTemplateName	
				end
		end
	else
		begin
			if(@BrandID > 0)
				begin
					select * 
					from DownloadTemplate with(nolock) 
					where IsDeleted = 0 and BrandID = @BrandID and PubID = 0
					order by DownloadTemplateName
				end
			else
				begin
					select * 
					from DownloadTemplate with(nolock) 
					where IsDeleted = 0 and PubID = 0 and BrandID =0
					order by DownloadTemplateName	
				end
		end

end
GO