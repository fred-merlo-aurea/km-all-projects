create procedure e_Address_Select_SubscriberId
@subscriberId int
as
BEGIN

	set nocount on

	select *
	from Address with(nolock)
	where subscriber_id = @subscriberId

END
go