create procedure e_Code_Select_CodeTypeName_CodeValue
@CodeTypeName varchar(50),
@CodeValue varchar(50)
as
BEGIN

	set nocount on

	declare @CodeTypeId int = (select CodeTypeId from CodeType with(nolock) where CodeTypeName = @CodeTypeName)
	select *
	from Code with(nolock)
	where CodeValue = @CodeValue
	and CodeTypeId = @CodeTypeId

END
go