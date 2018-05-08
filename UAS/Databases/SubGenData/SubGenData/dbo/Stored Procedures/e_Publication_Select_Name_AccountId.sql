create procedure e_Publication_Select_Name_AccountId
@name varchar(50),
@accountId int
as
BEGIN

	set nocount on

	select *
	from Publication with(nolock)
	where name = @name and account_id = @accountId

END
go