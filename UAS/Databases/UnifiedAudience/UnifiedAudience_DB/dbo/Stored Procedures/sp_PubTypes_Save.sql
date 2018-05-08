CREATE proc [dbo].[sp_PubTypes_Save](
@PubTypeID int, 
@PubTypeDisplayName varchar(50),
@ColumnReference  varchar(50),
@IsActive  bit,
@SortOrder int)
as
BEGIN

	SET NOCOUNT ON

	Declare @CurrentSortOrder int
	
	if (@PubTypeID > 0)
		Begin

			select @CurrentSortOrder = SortOrder 
			from  PubTypes 
			where PubTypeID = @PubTypeID;
			
			Update PubTypes 
				set PubTypeDisplayName =  @PubTypeDisplayName, 
					ColumnReference =  @ColumnReference,
					IsActive =  @IsActive,
					SortOrder = @SortOrder  
				where PubTypeID = @PubTypeID			
			if (@CurrentSortOrder < @SortOrder)
				Begin
					 Update PubTypes 
						set SortOrder = SortOrder - 1 
						WHERE SortOrder >=  @CurrentSortOrder and  SortOrder <=   @SortOrder and PubTypeID <> @PubTypeID
				end
			else
				begin
			  
					Update PubTypes 
						set SortOrder = SortOrder + 1 
						WHERE SortOrder >=  @SortOrder and  SortOrder <=  @CurrentSortOrder and PubTypeID <> @PubTypeID
				end
			select @PubTypeID
		End
	else
		Begin
			
  			select @CurrentSortOrder = ISNULL(MAX(sortorder),0)+1 from PubTypes
		
			INSERT INTO PubTypes
					(PubTypeDisplayName, 
					ColumnReference, 
					IsActive,
					SortOrder) 
			VALUES (@PubTypeDisplayName, 
					@ColumnReference, 
					@IsActive, 
					@SortOrder) 
					
			if (@CurrentSortOrder < @SortOrder)
				Begin
					 Update PubTypes 
						set SortOrder = SortOrder - 1 
						WHERE SortOrder >=  @CurrentSortOrder and  SortOrder <=   @SortOrder and PubTypeID <> @@IDENTITY
				end
			else
				begin
					Update PubTypes 
						set SortOrder = SortOrder + 1 
						WHERE SortOrder >=  @SortOrder and  SortOrder <=  @CurrentSortOrder and PubTypeID <> @@IDENTITY
				end							
			SELECT @@IDENTITY;
		End	

End