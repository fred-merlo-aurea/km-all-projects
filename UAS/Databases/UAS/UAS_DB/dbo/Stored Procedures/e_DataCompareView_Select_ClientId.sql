create procedure e_DataCompareView_Select_ClientId
@ClientId int
as
BEGIN

	set nocount on

	select v.*
	from DataCompareView v with(nolock)
	join DataCompareRun r with(nolock) on r.DcRunId = v.DcRunId
	where r.ClientId = @ClientId

END
go