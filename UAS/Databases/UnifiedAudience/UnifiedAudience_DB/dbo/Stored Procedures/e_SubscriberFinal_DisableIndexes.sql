CREATE PROCEDURE [dbo].[e_SubscriberFinal_DisableIndexes]
WITH EXECUTE AS OWNER
AS
BEGIN

	SET NOCOUNT ON

	--	ALTER INDEX IDX_SubscriberFinal_Address ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_Email ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_IGrp_No ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_IGrp_Rank ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_Phone ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_ProcessCode ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_PubCode ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_SourceFileId ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_STRecordIdentifier ON SubscriberFinal DISABLE
	--	ALTER INDEX IDX_SubscriberFinal_SubscriberFinalID ON SubscriberFinal DISABLE
END
go