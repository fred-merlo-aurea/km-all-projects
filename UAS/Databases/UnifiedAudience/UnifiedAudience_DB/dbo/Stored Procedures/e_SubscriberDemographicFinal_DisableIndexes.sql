CREATE PROCEDURE [dbo].[e_SubscriberDemographicFinal_DisableIndexes]
WITH EXECUTE AS OWNER
AS
BEGIN

	SET NOCOUNT ON

	--	ALTER INDEX IDX_SubscriberDemographicFinal_SFRecordIdentifier ON SubscriberDemographicFinal DISABLE
	--	ALTER INDEX IDX_SubscriberDemographicFinal_SORecordIdentifier ON SubscriberDemographicFinal DISABLE

END
go