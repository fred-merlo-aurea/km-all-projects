create procedure e_Code_Select_ParentCodeId
@ParentCodeId int
as
BEGIN

	set nocount on

	select *
	from Code with(nolock)
	where ParentCodeId = @ParentCodeId
	order by CodeName

END
go