CREATE PROCEDURE [dbo].[e_SubscriptionStatus_Select]
AS    
BEGIN

	set nocount on

	SELECT *
	FROM SubscriptionStatus With(NoLock)

END