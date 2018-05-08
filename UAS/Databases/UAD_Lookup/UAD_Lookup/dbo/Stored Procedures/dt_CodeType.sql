create procedure dt_CodeType
	@CodeTypeName varchar(50)
	as
		begin
			set nocount on
			select CodeId,DisplayName
			from Code c with(nolock)
			join CodeType ct with(nolock) on c.CodeTypeId = ct.CodeTypeId
			where ct.CodeTypeName = @CodeTypeName and c.IsActive = 'true' 
			order by DisplayName
		end
	go