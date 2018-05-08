CREATE PROCEDURE [dbo].[e_SubscriberFinal_EnableIndexes]
WITH EXECUTE AS OWNER
AS
BEGIN

	SET NOCOUNT ON

	--	ALTER INDEX IDX_SubscriberFinal_Address ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_Email ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_IGrp_No ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_IGrp_Rank ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_Phone ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_ProcessCode ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_PubCode ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_SourceFileId ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_STRecordIdentifier ON SubscriberFinal REBUILD
	--	ALTER INDEX IDX_SubscriberFinal_SubscriberFinalID ON SubscriberFinal REBUILD

END
go