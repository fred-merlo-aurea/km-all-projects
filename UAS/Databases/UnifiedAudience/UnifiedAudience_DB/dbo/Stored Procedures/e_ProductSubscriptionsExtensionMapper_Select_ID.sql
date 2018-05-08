CREATE PROCEDURE [dbo].[e_ProductSubscriptionsExtensionMapper_Select_ID]
@PubSubscriptionsExtensionMapperID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM PubSubscriptionsExtensionMapper With(NoLock)
	WHERE PubSubscriptionsExtensionMapperID = @PubSubscriptionsExtensionMapperID

END
