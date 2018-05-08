CREATE PROCEDURE [dbo].[e_SubscriberDemographicArchive_Select_SORecordIdentifier]
	@SARecordIdentifier uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM SubscriberDemographicArchive With(NoLock)
	WHERE SARecordIdentifier = @SARecordIdentifier
	ORDER BY MAFField

END
GO