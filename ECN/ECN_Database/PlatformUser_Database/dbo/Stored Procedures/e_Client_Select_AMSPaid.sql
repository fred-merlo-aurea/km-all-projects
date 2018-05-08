create procedure e_Client_Select_AMSPaid
as
	select *
	from Client with(nolock)
	where IsAMS = 'true' and HasPaid = 'true'
go
