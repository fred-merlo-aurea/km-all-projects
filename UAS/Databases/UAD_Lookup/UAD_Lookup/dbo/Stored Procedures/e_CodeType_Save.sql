create procedure e_CodeType_Save
@CodeTypeId int,
@CodeTypeName varchar(50), 
@CodeTypeDescription varchar(max),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
BEGIN

	set nocount on

	IF @CodeTypeId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
				
			UPDATE CodeType
			SET CodeTypeName = @CodeTypeName,
				CodeTypeDescription = @CodeTypeDescription,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE CodeTypeId = @CodeTypeId;
			
			SELECT @CodeTypeId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
				
			INSERT INTO CodeType (CodeTypeName,CodeTypeDescription,IsActive,DateCreated,CreatedByUserID)
			VALUES(@CodeTypeName,@CodeTypeDescription,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
go