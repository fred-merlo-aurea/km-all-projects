CREATE  PROCEDURE [dbo].[sp_SubscriptionsExtensionMapper_Select]   
@SubscriptionsExtensionMapperId int
AS  
BEGIN

	SET NOCOUNT ON  

	SELECT SubscriptionsExtensionMapperID, StandardField, CustomField, CustomFieldDataType, Active
	FROM SubscriptionsExtensionMapper
	WHERE SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperId

END