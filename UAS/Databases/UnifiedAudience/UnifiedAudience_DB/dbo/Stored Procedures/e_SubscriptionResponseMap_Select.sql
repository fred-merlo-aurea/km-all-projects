CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Select]
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT *
	FROM SubscriptionResponseMap With(NoLock)

END
GO