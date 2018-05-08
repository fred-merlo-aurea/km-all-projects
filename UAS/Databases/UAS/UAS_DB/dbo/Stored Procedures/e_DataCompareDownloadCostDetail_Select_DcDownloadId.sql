CREATE PROCEDURE [dbo].[e_DataCompareDownloadCostDetail_Select_DcDownloadId]
@dcDownloadId int
as
	begin
		set nocount on
		select cd.*
		from DataCompareDownloadCostDetail cd with(nolock)
		where cd.DcDownloadId = @dcDownloadId
	end
go

