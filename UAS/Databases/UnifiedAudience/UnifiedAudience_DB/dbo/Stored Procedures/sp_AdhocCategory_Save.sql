CREATE proc [dbo].[sp_AdhocCategory_Save](
@CategoryID int, 
@CategoryName varchar(50),
@SortOrder int)
as
BEGIN
	
	SET NOCOUNT ON

	Declare @CurrentSortOrder int
	
	if (@CategoryID > 0)
		Begin
			
--Update AdhocCategory set CategoryName =  @CategoryName, SortOrder= @SortOrder  where CategoryID = @CategoryID			
			
----			Update AdhocCategory set SortOrder = SortOrder+1 where SortOrder >= @SortOrder
			
--			Update AdhocCategory set SortOrder = X.newsort
--			from AdhocCategory ac join
--				(
--					select categoryID, ROW_NUMBER() over (order by sortorder) as newsort from AdhocCategory
--				)  x on x.CategoryID =  ac.CategoryID
			
--			select @CategoryID

			
			
			select @CurrentSortOrder = SortOrder 
			from  AdhocCategory 
			where CategoryID =  @CategoryID;
			
			Update AdhocCategory 
				set CategoryName =  @CategoryName, SortOrder = @SortOrder  
				where CategoryID = @CategoryID			
			
			if (@CurrentSortOrder < @SortOrder)
				Begin
					Update AdhocCategory 
						set SortOrder = SortOrder - 1 
						WHERE SortOrder >=  @CurrentSortOrder and  SortOrder <=   @SortOrder and CategoryID <> @CategoryID
				end
			else
				begin
					Update AdhocCategory 
						set SortOrder = SortOrder + 1 
						WHERE SortOrder >=  @SortOrder and  SortOrder <=  @CurrentSortOrder and CategoryID <> @CategoryID
				end
			select @CategoryID
		End
	else
		Begin
			
			select @CurrentSortOrder = ISNULL(MAX(sortorder),0)+1 
			from AdhocCategory
		
			insert  into AdhocCategory (CategoryName, SortOrder) 
			values (@CategoryName, @SortOrder)
			
			if (@CurrentSortOrder < @SortOrder)
				Begin
					Update AdhocCategory 
						set SortOrder = SortOrder - 1 
						WHERE SortOrder >=  @CurrentSortOrder and  SortOrder <=   @SortOrder and CategoryID <> @@IDENTITY
				end
			else
				begin
					Update AdhocCategory 
						set SortOrder = SortOrder + 1 
						WHERE SortOrder >=  @SortOrder and  SortOrder <=  @CurrentSortOrder and CategoryID <> @@IDENTITY
				end			
			SELECT @@IDENTITY;
		End	
End