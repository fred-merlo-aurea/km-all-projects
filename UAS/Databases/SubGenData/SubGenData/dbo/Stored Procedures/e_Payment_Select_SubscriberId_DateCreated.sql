create procedure e_Payment_Select_SubscriberId_DateCreated
@subscriberId int,
@dateCreated date
as
BEGIN

	set nocount on

	select *
	from Payment with(nolock)
	where subscriber_id = @subscriberId
	and cast(date_created as date) = @dateCreated
	--and (subscription_id is null or subscription_id = 0)

END
GO