CREATE PROCEDURE [dbo].[o_FileAudit_SelectDistinctProcessCodePerSourceFile]
AS
BEGIN

	SET NOCOUNT ON

	Select distinct SourceFileID, ProcessCode from
	(
		Select distinct SourceFileID, ProcessCode 
		from SubscriberOriginal With(NoLock)
		union
		Select distinct SourceFileID, ProcessCode 
		from SubscriberTransformed With(NoLock)
		union
		Select distinct SourceFileID, ProcessCode 
		from SubscriberInvalid With(NoLock)
		union
		Select distinct SourceFileID, ProcessCode 
		from SubscriberArchive With(NoLock)
		union
		Select distinct SourceFileID, ProcessCode 
		from SubscriberFinal With(NoLock)
	) as A

END