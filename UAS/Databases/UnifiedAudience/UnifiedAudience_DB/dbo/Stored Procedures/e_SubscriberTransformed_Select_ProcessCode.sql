CREATE PROCEDURE [dbo].[e_SubscriberTransformed_Select_ProcessCode]
	@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberTransformed With(NoLock)
	WHERE ProcessCode = @ProcessCode

END
GO