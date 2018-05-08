CREATE  PROCEDURE [dbo].[sp_SubscriptionsExtensionMapper_Select_All]   

AS  
BEGIN

	SET NOCOUNT ON   

	SELECT SubscriptionsExtensionMapperID, StandardField, CustomField, CustomFieldDataType, Active
	FROM SubscriptionsExtensionMapper
	ORDER BY SubscriptionsExtensionMapperID
	
End