create procedure e_AdHocDimensionGroup_Save
@AdHocDimensionGroupId int,
@AdHocDimensionGroupName varchar(50),
@ClientID int,
@IsActive bit,
@SourceFileID int,
@OrderOfOperation int,
@StandardField varchar(50),
@CreatedDimension varchar(50),
@DefaultValue varchar(MAX),
@IsPubcodeSpecific bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @AdHocDimensionGroupId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE AdHocDimensionGroup
			SET AdHocDimensionGroupName = @AdHocDimensionGroupName,
				ClientID = @ClientID,
				IsActive = @IsActive,
				SourceFileID = @SourceFileID,
				OrderOfOperation = @OrderOfOperation,
				StandardField = @StandardField,
				CreatedDimension = @CreatedDimension,
				DefaultValue = @DefaultValue,
				IsPubcodeSpecific = @IsPubcodeSpecific,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE AdHocDimensionGroupId = @AdHocDimensionGroupId;
		
			SELECT @AdHocDimensionGroupId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO AdHocDimensionGroup (AdHocDimensionGroupName,ClientID,SourceFileID,IsActive,OrderOfOperation,StandardField,CreatedDimension,DefaultValue,IsPubcodeSpecific,DateCreated,CreatedByUserID)
			VALUES(@AdHocDimensionGroupName,@ClientID,@SourceFileID,@IsActive,@OrderOfOperation,@StandardField,@CreatedDimension,@DefaultValue,@IsPubcodeSpecific,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO