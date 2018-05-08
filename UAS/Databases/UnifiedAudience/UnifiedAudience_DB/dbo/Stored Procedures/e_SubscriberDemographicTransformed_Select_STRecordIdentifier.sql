CREATE PROCEDURE [dbo].[e_SubscriberDemographicTransformed_Select_STRecordIdentifier]
@STRecordIdentifier uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicTransformed With(NoLock)
	WHERE STRecordIdentifier = @STRecordIdentifier

END
GO