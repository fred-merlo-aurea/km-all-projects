CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Exists_ByIDCustomField]
@SubscriptionsExtensionMapperID int, 
@CustomField varchar(255)
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 SubscriptionsExtensionMapperID
		FROM SubscriptionsExtensionMapper WITH (NOLOCK)
		WHERE CustomField = @CustomField and SubscriptionsExtensionMapperID != @SubscriptionsExtensionMapperID
	) SELECT 1 ELSE SELECT 0

END
