CREATE PROCEDURE [dbo].[e_SubscriberFinal_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberFinal With(NoLock)

END