create procedure e_Account_Select_KMClientId
@KMClientId int
as
BEGIN

	set nocount on

	select *
	from Account
	where KMClientId = @KMClientId

END
go