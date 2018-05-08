CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Select_ID]
@SubscriptionsExtensionMapperID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM SubscriptionsExtensionMapper With(NoLock)
	WHERE SubscriptionsExtensionMapperID = @SubscriptionsExtensionMapperID

END
