CREATE PROCEDURE e_SubscriptionStatusMatrix_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM SubscriptionStatusMatrix With(NoLock)

END