create procedure e_AdHocDimensionGroupPubcodeMap_Save
@AdHocDimensionGroupId int,
@Pubcode varchar(50),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF EXISTS (Select AdHocDimensionGroupId From AdHocDimensionGroupPubcodeMap With(NoLock) Where AdHocDimensionGroupId = @AdHocDimensionGroupId and Pubcode = @Pubcode) 
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE AdHocDimensionGroupPubcodeMap
			SET IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE AdHocDimensionGroupId = @AdHocDimensionGroupId and Pubcode = @Pubcode;
		
			SELECT @AdHocDimensionGroupId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO AdHocDimensionGroupPubcodeMap (AdHocDimensionGroupId,Pubcode,IsActive,DateCreated,CreatedByUserID)
			VALUES(@AdHocDimensionGroupId,@Pubcode,@IsActive,@DateCreated,@CreatedByUserID);
		END

END
GO