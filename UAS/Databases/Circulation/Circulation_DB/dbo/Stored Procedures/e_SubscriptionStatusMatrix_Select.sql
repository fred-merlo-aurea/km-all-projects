CREATE PROCEDURE e_SubscriptionStatusMatrix_Select
AS
	SELECT *
	FROM SubscriptionStatusMatrix With(NoLock)
