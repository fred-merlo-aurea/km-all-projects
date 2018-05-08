create procedure e_ProductSubscriptionsExtensionMapper_Save
@PubSubscriptionsExtensionMapperID int,
@PubID int,
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

	IF @PubSubscriptionsExtensionMapperID > 0
		BEGIN
			UPDATE PubSubscriptionsExtensionMapper
			SET PubID = @PubID,
				StandardField = @StandardField,
				CustomField = @CustomField,
				CustomFieldDataType = @CustomFieldDataType,
				Active = @Active,
				DateUpdated = GETDATE(),
				UpdatedByUserID = @UpdatedByUserID
			WHERE PubSubscriptionsExtensionMapperID = @PubSubscriptionsExtensionMapperID
			Select @PubSubscriptionsExtensionMapperID
		END 
	ELSE
		BEGIN
			INSERT INTO PubSubscriptionsExtensionMapper (PubID,StandardField,CustomField,CustomFieldDataType,Active,DateCreated,CreatedByUserID)           
			VALUES(@PubID,@StandardField,@CustomField,@CustomFieldDataType,@Active,GetDate(),@CreatedByUserID);SELECT @@IDENTITY;
		END

END
go