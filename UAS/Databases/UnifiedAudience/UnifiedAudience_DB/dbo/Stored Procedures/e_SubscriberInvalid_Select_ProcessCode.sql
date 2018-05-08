CREATE PROCEDURE [dbo].[e_SubscriberInvalid_Select_ProcessCode]
@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberInvalid With(NoLock)
	WHERE ProcessCode = @ProcessCode

END
GO