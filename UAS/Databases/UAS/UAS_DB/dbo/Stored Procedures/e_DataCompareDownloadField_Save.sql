CREATE PROCEDURE [dbo].[e_DataCompareDownloadField_Save]
@DcDownloadId int,
@DcDownloadFieldCodeId int,
@ColumnName varchar(50) = null,
@ColumnID int = 0,
@IsDescription bit = 'false'
as
	begin
		set nocount on
		
		insert into DataCompareDownloadField (DcDownloadId,DcDownloadFieldCodeId,ColumnName,ColumnID,IsDescription)
		values(@DcDownloadId,@DcDownloadFieldCodeId,@ColumnName,@ColumnID,@IsDescription);
	
		select @@IDENTITY;
	end