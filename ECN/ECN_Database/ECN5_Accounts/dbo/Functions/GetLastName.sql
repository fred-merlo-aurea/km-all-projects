CREATE  function GetLastName(@fullName as varchar(100)) returns varchar(50)
AS
Begin
	declare @lastName varchar(50)
	declare @firstIndex integer
	declare @secondIndex integer
	if @fullName is null
		return ''
	set @firstIndex = CharIndex(' ', @fullName)
	if @firstIndex = 0
		return ''
	set @secondIndex = CharIndex(' ', @fullName, @firstIndex+1);
	
	if @secondIndex = 0
		return SubString(@fullName,@firstIndex+1, len(@fullName) - @firstIndex)
	set @lastName = SubString(@fullName,@secondIndex+1, len(@fullName) - @secondIndex)
	return @lastName	
end
