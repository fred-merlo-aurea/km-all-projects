CREATE PROCEDURE [dbo].[e_SubscriberArchive_Select_ProcessCode]
@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberArchive With(NoLock)
	WHERE ProcessCode = @ProcessCode

END
GO