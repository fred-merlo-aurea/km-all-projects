CREATE PROCEDURE e_SubscriberArchive_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberArchive With(NoLock)

END