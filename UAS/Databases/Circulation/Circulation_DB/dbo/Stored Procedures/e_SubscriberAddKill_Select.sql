CREATE PROCEDURE [dbo].[e_SubscriberAddKill_Select]
AS
SELECT *
FROM SubscriberAddKill s With(NoLock)
