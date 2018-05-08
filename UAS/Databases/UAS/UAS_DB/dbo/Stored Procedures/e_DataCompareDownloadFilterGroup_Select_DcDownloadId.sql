CREATE PROCEDURE [dbo].[e_DataCompareDownloadFilterGroup_Select_DcDownloadId]
@dcDownloadId int
AS
	begin
		set nocount on
		select d.*
		from DataCompareDownloadFilterGroup d with(nolock)
		where d.DcDownloadId = @dcDownloadId
	end