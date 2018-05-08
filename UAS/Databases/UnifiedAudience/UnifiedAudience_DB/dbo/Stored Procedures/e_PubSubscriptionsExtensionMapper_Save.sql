CREATE PROCEDURE [dbo].[e_PubSubscriptionsExtensionMapper_Save]
@PubSubscriptionsExtensionMapperId int,
@PubID int,
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

	IF @PubSubscriptionsExtensionMapperId > 0
		BEGIN
	
			UPDATE 
				PubSubscriptionsExtensionMapper
			SET CustomField = @CustomField, 
				CustomFieldDataType = @CustomFieldDataType, 
				Active = @Active, 
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE PubSubscriptionsExtensionMapperID = @PubSubscriptionsExtensionMapperId
	
		END	
	ELSE
		BEGIN
	
			DECLARE @NewStandardFieldColumnNumber VARCHAR(3)

			SELECT @NewStandardFieldColumnNumber = ISNULL(MAX(CAST(SUBSTRING(StandardField, 6, LEN(StandardField) - 5) AS INT) + 1), 1) 
			FROM PubSubscriptionsExtensionMapper 
			where PubID = @pubID

			INSERT INTO PubSubscriptionsExtensionMapper (PubID, StandardField,CustomField,CustomFieldDataType,Active,DateCreated, CreatedByUserID)
			VALUES (@PubID, 'Field' + @NewStandardFieldColumnNumber, @CustomField, @CustomFieldDataType, @Active, GETDATE(), @CreatedByUserID)	
		
		END

END