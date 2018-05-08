create procedure e_Bundle_Select_Name_AccountId
@name varchar(250),
@accountId int
as
BEGIN

	set nocount on

	select *
	from Bundle with(nolock)
	where name = @name and account_id = @accountId and active = 'true'

END
go
