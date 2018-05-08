create procedure e_PaidOrder_Select_SubscriptionID
@SubscriptionId int
as
BEGIN

	set nocount on

	select *
	from PaidOrder with(nolock)
	where SubscriptionId = @SubscriptionId

END
go