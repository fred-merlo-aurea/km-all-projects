CREATE PROCEDURE [dbo].[e_DataCompareDownload_Select_ClientId]
@clientId int
AS
	begin
		set nocount on
		select d.*
		from DataCompareDownload d with(nolock)
		join DataCompareView v with(nolock) on d.DcViewId = v.DcViewId
		join DataCompareRun r with(nolock) on v.DcRunId = r.DcRunId
		where r.ClientId = @clientId
	end
go
