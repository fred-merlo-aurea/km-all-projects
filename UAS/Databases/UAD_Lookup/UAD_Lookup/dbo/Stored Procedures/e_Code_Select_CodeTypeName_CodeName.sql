create procedure e_Code_Select_CodeTypeName_CodeName
@CodeTypeName varchar(50),
@CodeName varchar(50)
as
BEGIN

	set nocount on

	declare @CodeTypeId int = (select CodeTypeId from CodeType with(nolock) where CodeTypeName = @CodeTypeName)
	select *
	from Code with(nolock)
	where CodeName = @CodeName
	and CodeTypeId = @CodeTypeId

END
go