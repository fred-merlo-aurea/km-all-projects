CREATE  function GetFirstName(@fullName as varchar(100)) returns varchar(50)
AS
Begin
	if @fullName is null
		return ''
	declare @firstName varchar(50)
	declare @index integer
	set @index = CharIndex(' ', @fullName)
	if @index > 0
	Begin
		set @firstName = SubString(@fullName,0, @index)
		return @firstName
	End	 
	return @fullName	
end
