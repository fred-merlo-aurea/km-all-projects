create procedure e_Payment_UpdateSubscriptionId
@orderId int,
@subscriptionId int
as
BEGIN

	set nocount on

	update Payment
	set subscription_id = @subscriptionId
	where order_id = @orderId

END
go