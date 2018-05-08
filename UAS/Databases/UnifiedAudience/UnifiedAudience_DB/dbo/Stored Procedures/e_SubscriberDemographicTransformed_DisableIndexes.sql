﻿CREATE PROCEDURE [dbo].[e_SubscriberDemographicTransformed_DisableIndexes]
WITH EXECUTE AS OWNER
AS
BEGIN

	SET NOCOUNT ON

	--	ALTER INDEX IDX_SubscriberDemographicTransformed_MAFField ON SubscriberDemographicTransformed DISABLE
	--	ALTER INDEX IDX_SubscriberDemographicTransformed_MAFField_PubID_SubscriberDemographicTransformedID_STRecordIdentifier ON SubscriberDemographicTransformed DISABLE
	--	ALTER INDEX IDX_SubscriberDemographicTransformed_SORecordIdentifier ON SubscriberDemographicTransformed DISABLE
	--	ALTER INDEX IDX_SubscriberDemographicTransformed_STRecordIdentifier ON SubscriberDemographicTransformed DISABLE

END
go