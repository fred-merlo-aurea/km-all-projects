CREATE PROCEDURE e_SubscriberOriginal_Select_ProcessCode
@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberOriginal With(NoLock)
	WHERE ProcessCode = @ProcessCode

END