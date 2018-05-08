CREATE PROCEDURE [dbo].[e_DataCompareDownload_Select_DcViewId]
@dcViewId int
AS
	begin
		set nocount on
		select d.*
		from DataCompareDownload d with(nolock)
		where d.DcViewId = @dcViewId
	end
go
