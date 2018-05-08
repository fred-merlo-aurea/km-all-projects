create procedure e_Code_Select_CodeTypeName
@CodeTypeName varchar(50)
as
BEGIN

	set nocount on

	declare @CodeTypeId int = (select CodeTypeId from CodeType with(nolock) where CodeTypeName = @CodeTypeName)
	select *
	from Code with(nolock)
	where CodeTypeId = @CodeTypeId
	order by CodeName

END
go