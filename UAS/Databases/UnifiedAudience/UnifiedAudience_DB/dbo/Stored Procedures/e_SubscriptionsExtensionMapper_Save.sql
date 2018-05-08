CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Save]
	@SubscriptionsExtensionMapperId int,
	@StandardField varchar(255),
	@CustomField varchar(255),
	@CustomFieldDataType varchar(25),
	@Active bit,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF @SubscriptionsExtensionMapperId > 0
		BEGIN
			UPDATE SubscriptionsExtensionMapper
			SET
				StandardField = @StandardField,
				CustomField = @CustomField,
				CustomFieldDataType = @CustomFieldDataType,
				Active = @Active,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE SubscriptionsExtensionMapperId = @SubscriptionsExtensionMapperId
			Select @SubscriptionsExtensionMapperId
		END 
	ELSE
		BEGIN
			INSERT INTO SubscriptionsExtensionMapper (StandardField,CustomField,CustomFieldDataType,Active,DateCreated,CreatedByUserID)           
			VALUES(@StandardField,@CustomField,@CustomFieldDataType,@Active,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END