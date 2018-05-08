CREATE  PROC [dbo].[e_SubscriptionsExtensionMapper_Exists_ByIDName] 
(
	@SubscriptionsExtensionMapperID int, 
	@Name varchar(255)
)
AS 
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 SubscriptionsExtensionMapperID
		FROM SubscriptionsExtensionMapper WITH (NOLOCK)
		WHERE CustomField = @Name and SubscriptionsExtensionMapperID != @SubscriptionsExtensionMapperID
	) SELECT 1 ELSE SELECT 0

END