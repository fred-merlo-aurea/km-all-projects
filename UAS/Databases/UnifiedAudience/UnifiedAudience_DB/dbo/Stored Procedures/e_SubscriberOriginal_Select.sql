CREATE PROCEDURE e_SubscriberOriginal_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberOriginal With(NoLock)

END