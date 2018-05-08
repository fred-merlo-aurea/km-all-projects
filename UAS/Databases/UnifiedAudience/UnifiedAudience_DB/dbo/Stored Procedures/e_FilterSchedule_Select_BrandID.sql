﻿CREATE PROCEDURE [dbo].[e_FilterSchedule_Select_BrandID]
@BrandID int,
@IsFilterSegmentation bit
AS
BEGIN

	SET NOCOUNT ON
	
	if @IsFilterSegmentation = 1
	Begin
		if @brandID > 0
			begin	
				select fs.*, rt.[Type] as RecurrenceType, f.Name as FilterName, b.brandID, b.brandName, fsg.FilterSegmentationName 
				from filterschedule fs with(nolock) 
					left outer join RecurrenceType rt with(nolock) on fs.RecurrenceTypeID = rt.RecurrenceTypeID 
					join Filters f with(nolock) on f.FilterID = fs.FilterID  
					join Brand b with(nolock) on b.BrandID = f.BrandID 
					join FilterSegmentation fsg with(nolock) on fsg.FilterSegmentationID = fs.FilterSegmentationID  
				where fs.IsDeleted  = 0 and f.IsDeleted = 0 and isnull(b.IsDeleted,0) = 0 and b.BrandID = @BrandID and isnull(fs.FilterSegmentationID, 0) >= 0
 			end
 		else
 			begin
 				select fs.*, rt.[Type] as RecurrenceType, f.Name as FilterName, b.brandID, b.brandName, fsg.FilterSegmentationName 
				from  filterschedule fs with(nolock) 
					left outer join RecurrenceType rt with(nolock) on fs.RecurrenceTypeID = rt.RecurrenceTypeID 
					join  Filters f with(nolock) on f.FilterID = fs.FilterID 
					left outer join Brand b with(nolock) on b.BrandID = f.BrandID
					join FilterSegmentation fsg with(nolock) on fsg.FilterSegmentationID = fs.FilterSegmentationID
				where fs.IsDeleted  = 0 and f.IsDeleted = 0  and isnull(b.IsDeleted,0) = 0 and ISNULL(b.brandID, 0) = 0 and isnull(fs.FilterSegmentationID, 0) >= 0
 			end
	End
	Else
	Begin
		if @brandID > 0
			begin	
				select fs.*, rt.[Type] as RecurrenceType, f.Name as FilterName, b.brandID, b.brandName 
				from filterschedule fs with(nolock) 
					left outer join RecurrenceType rt with(nolock) on fs.RecurrenceTypeID = rt.RecurrenceTypeID 
					join Filters f with(nolock) on f.FilterID = fs.FilterID  
					join Brand b with(nolock) on b.BrandID = f.BrandID  
				where fs.IsDeleted  = 0 and f.IsDeleted = 0 and isnull(b.IsDeleted,0) = 0 and b.BrandID = @BrandID and isnull(fs.FilterSegmentationID, 0) = 0
 			end
 		else
 			begin
 				select fs.*, rt.[Type] as RecurrenceType, f.Name as FilterName, b.brandID, b.brandName 
				from  filterschedule fs with(nolock) 
					left outer join RecurrenceType rt with(nolock) on fs.RecurrenceTypeID = rt.RecurrenceTypeID 
					join  Filters f with(nolock) on f.FilterID = fs.FilterID 
					left outer join Brand b with(nolock) on b.BrandID = f.BrandID
				where fs.IsDeleted  = 0 and f.IsDeleted = 0  and isnull(b.IsDeleted,0) = 0 and ISNULL(b.brandID, 0) = 0 and isnull(fs.FilterSegmentationID, 0) = 0
 			end	
	End
End