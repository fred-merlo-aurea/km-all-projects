CREATE PROCEDURE [dbo].[e_SubscriberFinal_Select_ProcessCode]
@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberFinal With(NoLock)
	WHERE ProcessCode = @ProcessCode

END