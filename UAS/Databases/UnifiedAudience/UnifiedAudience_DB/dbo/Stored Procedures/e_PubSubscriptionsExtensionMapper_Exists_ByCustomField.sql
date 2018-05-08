CREATE PROCEDURE [dbo].[e_PubSubscriptionsExtensionMapper_Exists_ByCustomField]
	@CustomField varchar(255)
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS (
		SELECT TOP 1 PubSubscriptionsExtensionMapperID
		FROM PubSubscriptionsExtensionMapper WITH (NOLOCK)
		WHERE CustomField = @CustomField 
	) SELECT 1 ELSE SELECT 0

END