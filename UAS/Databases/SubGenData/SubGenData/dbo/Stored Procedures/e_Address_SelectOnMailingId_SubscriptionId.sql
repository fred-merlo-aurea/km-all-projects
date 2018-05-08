create procedure e_Address_SelectOnMailingId_SubscriptionId
@subscriptionId int
as
BEGIN

	set nocount on

	select a.*
	from Address a with(nolock)
	join Subscription s with(nolock) on a.address_id = s.mailing_address_id
	where s.subscription_id = @subscriptionId

END
go