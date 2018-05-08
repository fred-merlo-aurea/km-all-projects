CREATE PROCEDURE [dbo].[e_ProductSubscriptionsExtensionMapper_Delete]
@PubSubscriptionsExtensionMapperID int,
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	declare @standardField varchar(255)
	
	select @standardField = StandardField 
	from  PubSubscriptionsExtensionMapper 
	where PubSubscriptionsExtensionMapperID = @PubSubscriptionsExtensionMapperID  
	
	exec ('update pse set [' + @standardField + '] = null from PubSubscriptionsExtension pse join pubsubscriptions ps  on pse.pubsubscriptionID = ps.pubsubscriptionID  where ps.PubID = ' +  @PubID  + ' and isnull(' + @standardField + ', '''') <> ''''')
	
	DELETE 
	FROM PubSubscriptionsExtensionMapper 
	WHERE PubSubscriptionsExtensionMapperID = @PubSubscriptionsExtensionMapperID  

End
