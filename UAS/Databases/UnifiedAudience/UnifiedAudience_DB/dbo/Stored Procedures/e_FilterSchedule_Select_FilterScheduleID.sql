CREATE PROCEDURE [dbo].[e_FilterSchedule_Select_FilterScheduleID]
@FilterScheduleID int
AS
Begin

 	select fs.*, rt.[Type] as RecurrenceType, f.Name as FilterName
	from filterschedule fs with(nolock) 
		left outer join RecurrenceType rt with(nolock) on fs.RecurrenceTypeID = rt.RecurrenceTypeID 
		join Filters f with(nolock) on f.FilterID = fs.FilterID 
		left outer join Brand b with(nolock) on b.BrandID = f.BrandID
	where fs.FilterScheduleID  = @FilterScheduleID 

End