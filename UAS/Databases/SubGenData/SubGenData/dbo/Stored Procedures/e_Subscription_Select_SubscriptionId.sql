create procedure e_Subscription_Select_SubscriptionId
@subscriptionId int
as
BEGIN

	set nocount on

	select *
	from Subscription with(nolock)
	where subscription_id = @subscriptionId

END
go