CREATE PROCEDURE dc_NoDataSelectList
@dcResultQueId int
AS
BEGIN

	set nocount on

	declare @dcOptId int = (select DataCompareOptionId from DataCompareOption with(nolock) where OptionName = 'Matching Profiles')

	declare @list varchar(max)
	select @list = COALESCE(@list + ', ', '') + 'sf.' + c.CodeName
	from DataCompareUserMatrix um with(nolock)
	join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
	join UAD_Lookup..Code c with(nolock) on cm.CodeId = c.CodeId 
	where um.DataCompareResultQueId = @dcResultQueId
	and cm.DataCompareOptionId = @dcOptId
	and um.IsActive = 'true'
	and c.IsActive = 'true'
	and cm.CodeTypeId in (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Profile Standard Attributes' or CodeTypeName = 'Profile Premium Attributes')
	
	select @list

END
go