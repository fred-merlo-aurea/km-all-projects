CREATE PROCEDURE [dbo].[e_SubscriberTransformed_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)

END