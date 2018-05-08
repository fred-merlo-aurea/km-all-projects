create procedure e_PaidOrderDetail_Select_ProductSubscriptionId
@ProductSubscriptionId int
as
BEGIN

	set nocount on

	select *
	from PaidOrderDetail with(nolock)
	where ProductSubscriptionId = @ProductSubscriptionId

END
go