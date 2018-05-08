CREATE PROCEDURE [dbo].[e_SubscriberTransformed_EnableIndexes]
WITH EXECUTE AS OWNER
AS
BEGIN

	SET NOCOUNT ON

	--	ALTER INDEX IDX_Address ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_Email ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_IGrp_No ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_IGrp_Rank ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_Phone ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_PubCode ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_SourceFileID ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_SubscriberTransformed_ProcessCode ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_SubscriberTransformed_SORecordIdentifier ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_SubscriberTransformed_SourceFileID_ProcessCode_STRecordIdentifier_SORecordIdentifier ON SubscriberTransformed REBUILD
	--	ALTER INDEX IDX_SubscriberTransformed_STRecordIdentifier_SourceFileID_ProcessCode_ImportRowNumber ON SubscriberTransformed REBUILD
END
go