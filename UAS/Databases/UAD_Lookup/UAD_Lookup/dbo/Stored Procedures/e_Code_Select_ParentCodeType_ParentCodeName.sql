create procedure e_Code_Select_ParentCodeType_ParentCodeName
@parentCodeType varchar(100),
@parentCode varchar(50)
as
	begin
		set nocount on

		declare @ctid int = (Select CodeTypeId from CodeType with(nolock) where CodeTypeName = @parentCodeType)

		select c.*
		from Code c with(nolock)
		where c.ParentCodeId = (select d.CodeId from Code d with(nolock) where d.CodeTypeId = @ctid and d.CodeName = @parentCode)
		order by c.DisplayOrder
	end
go

