CREATE PROCEDURE [dbo].[e_SubscriberDemographicTransformed_Select_SORecordIdentifier]
@SORecordIdentifier uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicTransformed With(NoLock)
	WHERE SORecordIdentifier = @SORecordIdentifier

END
GO