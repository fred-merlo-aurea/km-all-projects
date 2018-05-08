create procedure o_SourceFile_IsFileNameUnique
@clientId int,
@fileName varchar(250)
as
	begin
		set nocount on
		select count([FileName])
		from SourceFile with(nolock)
		where ClientId = @clientId and [FileName] = @fileName and IsDeleted = 'false'
	end
go
