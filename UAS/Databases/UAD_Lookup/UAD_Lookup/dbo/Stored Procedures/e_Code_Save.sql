create procedure e_Code_Save
@CodeId int,
@CodeTypeId int,
@CodeName varchar(50), 
@CodeValue varchar(50),
@DisplayName varchar(50),
@CodeDescription varchar(max),
@DisplayOrder int,
@HasChildren bit,
@ParentCodeId int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
BEGIN

	set nocount on

	IF @CodeId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
				
			UPDATE Code
			SET CodeTypeId = @CodeTypeId,
				CodeName = @CodeName,
				CodeValue = @CodeValue,
				DisplayName = @DisplayName,
				CodeDescription = @CodeDescription,
				DisplayOrder = @DisplayOrder,
				HasChildren = @HasChildren,
				ParentCodeId = @ParentCodeId,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE CodeId = @CodeId;
			
			SELECT @CodeId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
				
			INSERT INTO Code (CodeTypeId,CodeName,CodeValue,DisplayName,CodeDescription,DisplayOrder,HasChildren,ParentCodeId,IsActive,DateCreated,CreatedByUserID)
			VALUES(@CodeTypeId,@CodeName,@CodeValue,@DisplayName,@CodeDescription,@DisplayOrder,@HasChildren,@ParentCodeId,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END
END