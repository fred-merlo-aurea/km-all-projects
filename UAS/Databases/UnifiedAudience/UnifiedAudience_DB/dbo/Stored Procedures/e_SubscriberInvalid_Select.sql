CREATE PROCEDURE e_SubscriberInvalid_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberInvalid With(NoLock)

END