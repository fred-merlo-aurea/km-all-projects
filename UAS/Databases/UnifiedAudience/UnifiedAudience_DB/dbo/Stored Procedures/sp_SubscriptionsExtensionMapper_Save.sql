CREATE  PROCEDURE [dbo].[sp_SubscriptionsExtensionMapper_Save]   
@SubscriptionsExtensionMapperId int,
@CustomField varchar(255),
@CustomFieldDataType varchar(25),
@Active bit
AS  
BEGIN

	SET NOCOUNT ON  

	IF EXISTS (SELECT 1 FROM SubscriptionsExtensionMapper WHERE SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperId)
		BEGIN
	
			UPDATE SubscriptionsExtensionMapper
			SET CustomField = @CustomField, CustomFieldDataType = @CustomFieldDataType, Active = @Active, DateUpdated = GETDATE()
			WHERE SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperId
	
		END
	ELSE
		BEGIN
	
			DECLARE @NewStandardFieldColumnNumber VARCHAR(3)
			SELECT @NewStandardFieldColumnNumber = ISNULL(MAX(CAST(SUBSTRING(StandardField, 6, LEN(StandardField) - 5) AS INT) + 1), 1) 
			FROM SubscriptionsExtensionMapper

			INSERT INTO SubscriptionsExtensionMapper (StandardField,CustomField,CustomFieldDataType,Active,DateCreated)
					VALUES ('Field' + @NewStandardFieldColumnNumber, @CustomField, @CustomFieldDataType, @Active, GETDATE())	
		
		END
END