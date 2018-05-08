create procedure e_DataCompareCostBase_Select
as
BEGIN

	set nocount on

	select *
	from DataCompareCostBase with(nolock)

END
go