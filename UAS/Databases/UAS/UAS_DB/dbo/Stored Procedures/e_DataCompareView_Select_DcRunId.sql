CREATE PROCEDURE [dbo].[e_DataCompareView_Select_DcRunId]
@dcRunId int
AS
	begin
		set nocount on
		select v.*
		from DataCompareView v with(nolock)
		join DataCompareRun r with(nolock) on v.DcRunId = r.DcRunId
		where r.DcRunId = @dcRunId
	end
go

