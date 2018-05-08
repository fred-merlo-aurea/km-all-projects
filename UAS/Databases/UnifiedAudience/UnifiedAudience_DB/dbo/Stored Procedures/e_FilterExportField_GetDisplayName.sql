CREATE proc [dbo].[e_FilterExportField_GetDisplayName](
@FilterScheduleID int
)
as
BEGIN

	SET NOCOUNT ON

    declare @PubID int
    select @PubID = PubID 
	from filters f 
		join FilterSchedule fs on f.FilterID = fs.FilterID 
	where FilterScheduleID = @FilterScheduleID 
    
    if(@PubID>0)
		Begin
 			select fef.*, 
 				case when rg.DisplayName is not null then rg.DisplayName when psem.CustomField IS not null then psem.CustomField else ExportColumn end as displayname  
 			from FilterExportField fef with (nolock) 
				left outer join ResponseGroups rg with (nolock) on fef.ExportColumn  = CONVERT(varchar, rg.ResponseGroupID) 
				left outer join PubSubscriptionsExtensionMapper psem with (nolock) on fef.ExportColumn  = psem.StandardField  and psem.PubID = @PubID
			where FilterScheduleID = @FilterScheduleID  
			order by FilterExportFieldID  
		End
    else
		Begin
			select fef.*, 
				case when mg.ColumnReference is not null then mg.DisplayName when sem.CustomField IS not null then sem.CustomField else ExportColumn end as displayname 
			from FilterExportField fef with (nolock) 
				left outer join SubscriptionsExtensionMapper sem with (nolock) on fef.ExportColumn  = sem.StandardField 
				left outer join MasterGroups mg with (nolock) on fef.ExportColumn = mg.ColumnReference 
			where FilterScheduleID = @FilterScheduleID  
			order by FilterExportFieldID
		End

End