create procedure e_ProductAudit_Save
@ProductAuditId int,
@ProductId int,
@AuditField varchar(255),
@FieldMappingTypeId int,
@ResponseGroupID int,
@SubscriptionsExtensionMapperID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
BEGIN

	set nocount on

	IF @ProductAuditId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
					
			UPDATE ProductAudit
				SET ProductId = @ProductId,
					AuditField = @AuditField,
					FieldMappingTypeId = @FieldMappingTypeId,
					ResponseGroupID = @ResponseGroupID,
					SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperID,
					IsActive = @IsActive,
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID
			WHERE ProductAuditId = @ProductAuditId
			SELECT @ProductAuditId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO ProductAudit (ProductId,AuditField,FieldMappingTypeId,ResponseGroupID,SubscriptionsExtensionMapperID,IsActive,DateCreated,CreatedByUserID)
			VALUES (@ProductId,@AuditField,@FieldMappingTypeId,@ResponseGroupID,@SubscriptionsExtensionMapperID,@IsActive,@DateCreated,@CreatedByUserID);Select @@IDENTITY;
		END	

END
go