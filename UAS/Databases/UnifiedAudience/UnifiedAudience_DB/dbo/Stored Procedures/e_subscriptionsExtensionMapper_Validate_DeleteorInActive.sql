CREATE PROCEDURE [dbo].[e_subscriptionsExtensionMapper_Validate_DeleteorInActive]
	@SubscriptionsExtensionMapperID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	declare @ColReference varchar(100)
	
	select @ColReference = StandardField  
	from SubscriptionsExtensionMapper
	where SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperID
	
	declare @references table  (Reference varchar(100), ReferenceName varchar(100), ReferenceID1 int, ReferenceID2 int)
	
	if exists (select top 1 1 from FilterDetails with (NOLOCK) where [Group]  like '%e|' + @ColReference + '|%')
		Begin
			insert into @references
			select distinct 'Filter', f.Name, f.filterID, 0
			from FilterDetails fd with (NOLOCK) 
				join FilterGroup fg  with (NOLOCK)on fg.FilterGroupID = fd.FilterGroupID 
				join Filters f  with (NOLOCK) on f.FilterID = fg.FilterID
			where [Group] like '%e|' + @ColReference + '|%' and f.IsDeleted = 0
		End
	if exists (select top 1 1 from FilterExportField with (NOLOCK) where ExportColumn = @ColReference)
		Begin
			insert into @references
			select distinct 'Filter Export Schedule', f.Name, f.FilterID, fs.FilterScheduleID
			from FilterExportField fef with (NOLOCK) 
				join FilterSchedule fs  with (NOLOCK)on fef.FilterScheduleID = fs.FilterScheduleID 
				join Filters f  with (NOLOCK) on f.FilterID = fs.FilterID
			where ExportColumn = @ColReference and f.IsDeleted = 0 and fs.IsDeleted = 0
		End
	
	select * 
	from @references

End