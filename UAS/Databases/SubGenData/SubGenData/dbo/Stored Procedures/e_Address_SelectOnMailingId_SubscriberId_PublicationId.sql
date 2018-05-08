create procedure e_Address_SelectOnMailingId_SubscriberId_PublicationId
@SubscriberId int,
@PublicationId int
as
BEGIN

	set nocount on

	select a.*
	from Address a with(nolock)
	join Subscription s with(nolock) on a.address_id = s.mailing_address_id
	where s.publication_id = @PublicationId
	and a.subscriber_id = @SubscriberId

END
go