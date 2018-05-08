create procedure e_Rule_Select_ClientId
@clientId int
as
	begin
		select *
		from [Rule] with(nolock)
		where ClientId = @clientId
	end
go
