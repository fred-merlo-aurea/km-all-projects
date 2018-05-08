CREATE PROCEDURE [dbo].[e_SubscriberDemographicFinal_EnableIndexes]
WITH EXECUTE AS OWNER
AS
BEGIN

	SET NOCOUNT ON

	--	ALTER INDEX IDX_SubscriberDemographicFinal_SFRecordIdentifier ON SubscriberDemographicFinal REBUILD
	--	ALTER INDEX IDX_SubscriberDemographicFinal_SORecordIdentifier ON SubscriberDemographicFinal REBUILD

END
go