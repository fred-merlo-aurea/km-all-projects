create procedure e_Address_Select_OldAddress_AccountId_PublicationId
@AccountId int,
@PublicationId int,
@address varchar(50),
@address2 varchar(50),
@city varchar(50),
@state varchar(60),
@zipCode varchar(12)
as
BEGIN

	set nocount on

	select a.*
	from Address a with(nolock)
		join Subscription s with(nolock) on a.address_id = s.mailing_address_id
	where a.account_id = @AccountId 
		and s.publication_id = @PublicationId
		and a.address = @address
		and a.address_line_2 = @address2
		and a.city = @city
		and a.state = @state
		and a.zip_code = @zipCode

END
go