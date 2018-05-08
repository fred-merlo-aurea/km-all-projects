CREATE PROCEDURE [dbo].[e_DataCompareDownloadFilterGroup_Save]
@DcDownloadId int
as
	begin
		set nocount on
		
		insert into DataCompareDownloadFilterGroup (DcDownloadId)
		values(@DcDownloadId);
		
		select @@IDENTITY;
	end
