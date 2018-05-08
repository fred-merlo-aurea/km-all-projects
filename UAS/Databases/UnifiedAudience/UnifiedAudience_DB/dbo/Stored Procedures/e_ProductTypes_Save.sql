CREATE PROCEDURE [dbo].[e_ProductTypes_Save]
	@PubTypeID int, 
	@PubTypeDisplayName varchar(50),
	@ColumnReference  varchar(50),
	@IsActive  bit,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
as
BEGIN

	SET NOCOUNT ON

	Declare @CurrentSortOrder int

	IF @PubTypeID > 0
		BEGIN						
			SELECT @CurrentSortOrder = SortOrder from  PubTypes with(nolock) where PubTypeID = @PubTypeID;

			UPDATE PubTypes 
				SET PubTypeDisplayName =  @PubTypeDisplayName, 
					ColumnReference =  @ColumnReference,
					IsActive =  @IsActive,
					SortOrder = @SortOrder,
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID
			WHERE PubTypeID = @PubTypeID			
			
			IF (@CurrentSortOrder < @SortOrder)
				BEGIN
					 UPDATE PubTypes set SortOrder = SortOrder - 1, DateUpdated = @DateUpdated, UpdatedByUserID = @UpdatedByUserID WHERE SortOrder >=  @CurrentSortOrder and  SortOrder <=   @SortOrder and PubTypeID <> @PubTypeID
				END
			ELSE
				BEGIN			  
					UPDATE PubTypes set SortOrder = SortOrder + 1, DateUpdated = @DateUpdated, UpdatedByUserID = @UpdatedByUserID  WHERE SortOrder >=  @SortOrder and  SortOrder <=  @CurrentSortOrder and PubTypeID <> @PubTypeID
				END
			
			SELECT @PubTypeID
			
		END
	ELSE
		BEGIN
			
  			SELECT @CurrentSortOrder = ISNULL(MAX(sortorder),0)+1 from PubTypes
		
			INSERT INTO PubTypes (PubTypeDisplayName, ColumnReference, IsActive, SortOrder,DateCreated,CreatedByUserID) 
			VALUES (@PubTypeDisplayName, @ColumnReference, @IsActive, @SortOrder,@DateCreated,@CreatedByUserID) 
					
			IF (@CurrentSortOrder < @SortOrder)
				BEGIN
					 UPDATE PubTypes set SortOrder = SortOrder - 1, DateUpdated = @DateCreated, UpdatedByUserID = @CreatedByUserID  WHERE SortOrder >= @CurrentSortOrder and  SortOrder <= @SortOrder and PubTypeID <> @@IDENTITY
				END
			ELSE
				BEGIN			  
					UPDATE PubTypes set SortOrder = SortOrder + 1, DateUpdated = @DateCreated, UpdatedByUserID = @CreatedByUserID WHERE SortOrder >= @SortOrder and  SortOrder <= @CurrentSortOrder and PubTypeID <> @@IDENTITY
				END					
					
			SELECT @@IDENTITY;
		END	

END