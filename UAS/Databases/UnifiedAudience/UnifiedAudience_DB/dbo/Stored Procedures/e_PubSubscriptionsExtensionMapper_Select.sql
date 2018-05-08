CREATE PROCEDURE [dbo].[e_PubSubscriptionsExtensionMapper_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM PubSubscriptionsExtensionMapper With(NoLock) 

END