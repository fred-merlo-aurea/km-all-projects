﻿CREATE PROCEDURE [dbo].[e_PubSubscriptionsExtensionMapper_Validate_DeleteorInActive]
	@PubSubscriptionsExtensionMapperID int,
	@PubID int
AS
BEGIN

	SET NOCOUNT ON
	
	declare @ColReference varchar(100)
	
	select @ColReference = StandardField  
	from PubSubscriptionsExtensionMapper
	where PubSubscriptionsExtensionMapperID = @PubSubscriptionsExtensionMapperID
	
	declare @references table  (Reference varchar(100), ReferenceName varchar(100), ReferenceID1 int, ReferenceID2 int)
	
	if exists	(select top 1 1 
				from FilterDetails fd with (NOLOCK) 
					join FilterGroup fg  with (NOLOCK)on fg.FilterGroupID = fd.FilterGroupID 
					join Filters f with (NOLOCK) on fg.FilterID = f.FilterID 
				where [Group]  like '%e|' + @ColReference + '|%'  and f.PubID = @PubID
				)
	Begin
		insert into @references
		select distinct 'Filter', f.Name, f.filterID, 0
		from FilterDetails fd with (NOLOCK) 
			join FilterGroup fg  with (NOLOCK) on fg.FilterGroupID = fd.FilterGroupID 
			join Filters f  with (NOLOCK) on f.FilterID = fg.FilterID
		where [Group] like '%e|' + @ColReference + '|%' and f.IsDeleted = 0 and f.PubID = @PubID
	End
	
	if exists	(select top 1 1 
				from FilterExportField fef with (NOLOCK) 
					join FilterSchedule fs on fef.FilterScheduleID = fs.FilterScheduleID
					join Filters f with (NOLOCK) on fs.FilterID = f.FilterID 
				where ExportColumn = @ColReference and f.PubID = @PubID)
	Begin
		insert into @references
		select distinct 'Filter Export Schedule', f.Name, f.FilterID, fs.FilterScheduleID
		from FilterExportField fef with (NOLOCK) 
			join FilterSchedule fs  with (NOLOCK)on fef.FilterScheduleID = fs.FilterScheduleID 
			join Filters f  with (NOLOCK) on f.FilterID = fs.FilterID
		where ExportColumn = @ColReference and f.IsDeleted = 0 and fs.IsDeleted = 0 and f.PubID = @PubID
	End
	
	select * 
	from @references

End