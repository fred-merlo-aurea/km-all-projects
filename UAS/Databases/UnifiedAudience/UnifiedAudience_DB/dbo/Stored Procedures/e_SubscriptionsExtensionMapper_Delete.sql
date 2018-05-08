CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Delete]
	@SubscriptionsExtensionMapperID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	declare @standardField varchar(255)
	
	select @standardField = StandardField 
	from SubscriptionsExtensionMapper 
	where  SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperID  
	
	exec ('update SubscriptionsExtension set ' + @standardField + ' = null where isnull(' + @standardField + ', '''') <> ''''')
	
	DELETE 
	FROM SubscriptionsExtensionMapper 
	WHERE SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperID  

End