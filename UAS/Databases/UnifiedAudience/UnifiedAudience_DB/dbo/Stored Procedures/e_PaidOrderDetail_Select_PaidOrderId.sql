create procedure e_PaidOrderDetail_Select_PaidOrderId
@PaidOrderId int
as
BEGIN

	set nocount on

	select *
	from PaidOrderDetail with(nolock)
	where PaidOrderId = @PaidOrderId

END
go