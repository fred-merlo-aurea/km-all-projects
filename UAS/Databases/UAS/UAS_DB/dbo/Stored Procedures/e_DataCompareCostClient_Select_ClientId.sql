create procedure e_DataCompareCostClient_Select_ClientId
@ClientId int
as
BEGIN

	set nocount on

	select *
	from DataCompareCostClient with(nolock)
	where ClientId = @ClientId

END
go