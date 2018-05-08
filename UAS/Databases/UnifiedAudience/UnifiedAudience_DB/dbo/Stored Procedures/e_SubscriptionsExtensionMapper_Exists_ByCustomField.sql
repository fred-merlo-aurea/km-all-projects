CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Exists_ByCustomField]
@CustomField varchar(255)
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 SubscriptionsExtensionMapperID
		FROM SubscriptionsExtensionMapper WITH (NOLOCK)
		WHERE CustomField = @CustomField
	) SELECT 1 ELSE SELECT 0

END