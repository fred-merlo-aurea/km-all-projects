CREATE PROCEDURE [dbo].[e_FilterSchedule_Select_FilterSegmentationID]
@FilterSegmentationID int
AS
Begin

 	select fsc.*, rt.[Type] as RecurrenceType, f.Name as FilterName, fs.FilterSegmentationName as FilterSegmentationName
	from filterschedule fsc with(nolock) 
		left outer join RecurrenceType rt with(nolock) on fsc.RecurrenceTypeID = rt.RecurrenceTypeID 
		join Filters f with(nolock) on f.FilterID = fsc.FilterID 
		join FilterSegmentation fs with(nolock) on fs.FilterSegmentationID = fsc.FilterSegmentationID
	where fsc.FilterSegmentationID  = @FilterSegmentationID 

End