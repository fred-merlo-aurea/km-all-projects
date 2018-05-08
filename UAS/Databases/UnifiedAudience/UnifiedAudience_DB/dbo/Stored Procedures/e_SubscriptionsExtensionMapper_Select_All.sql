CREATE PROCEDURE [dbo].[e_SubscriptionsExtensionMapper_Select_All]
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT * 
	FROM SubscriptionsExtensionMapper With(NoLock)

END