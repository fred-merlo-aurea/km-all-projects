CREATE PROCEDURE rpt_SourceFile_Summary
@SourceFileID int,
@ProcessCode  varchar(50),
@FileName varchar(100)
AS
BEGIN
	
	SET NOCOUNT ON

	SELECT  o.ProcessCode, @SourceFileID as SourceFileID, @FileName as FileName, LastUploaded,OriginalCount, 
		ISNULL(TransformedCount,0) as 'TransformedCount', ISNULL(ArchivedCount,0) as 'ArchivedCount', ISNULL(InvalidCount,0) as 'InvalidCount', 
		ISNULL(FinalCount,0) as 'FinalCount', ISNULL(MasterCount,0) as 'MasterCount', ISNULL(SubordinateCount,0) as 'SubordinateCount',
		ISNULL(SuppressedCount,0) as 'SuppressedCount'
	FROM
		(
			SELECT so.SourceFileID, so.ProcessCode, COUNT(SubscriberOriginalID) as 'OriginalCount', MIN(so.DateCreated) as 'LastUploaded'
			FROM subscriberoriginal so WITH(NoLock)
			GROUP BY so.sourcefileID, so.ProcessCode
		)
		o
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberTransformedID) as 'TransformedCount'
			FROM SubscriberTransformed so WITH(NoLock)
			GROUP BY so.sourcefileID, so.ProcessCode
			)
		t on t.SourceFileID = o.SourceFileID  and t.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
			FROM SubscriberArchive so with (NOLOCK)
			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
			)
		y on y.SourceFileID = o.SourceFileID and y.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberFinalID) as 'FinalCount'
			FROM SubscriberFinal so WITH(NoLock)
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		z on z.SourceFileID = o.SourceFileID and z.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberInvalidID) as 'InvalidCount'
			FROM SubscriberInvalid so WITH(NoLock)
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		i on i.SourceFileID = o.SourceFileID  and i.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'MasterCount'
			FROM SubscriberFinal so WITH(NoLock)
			WHERE so.IGrp_Rank = 'M'
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		r on r.SourceFileID = o.SourceFileID and r.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'SubordinateCount'
			FROM SubscriberFinal so WITH(NoLock)
			WHERE so.IGrp_Rank = 'S'
			GROUP BY so.SourceFileID, so.ProcessCode
			)
		s on s.SourceFileID = o.SourceFileID and s.ProcessCode  = o.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT  COUNT(sup.STRecordIdentifier) as 'SuppressedCount'
			FROM SubscriberFinal so WITH(NoLock)
				JOIN Suppressed sup with(Nolock) on so.SFRecordIdentifier = sup.SFRecordIdentifier
			WHERE so.ProcessCode = @ProcessCode 
			)
		x on s.SourceFileID = o.SourceFileID and s.ProcessCode  = o.ProcessCode
	WHERE o.ProcessCode = @ProcessCode 
	ORDER BY OriginalCount asc
--CREATE PROCEDURE rpt_SourceFile_Summary
--@SourceFileID int,
--@ProcessCode  varchar(50)
--AS
--	SELECT  o.ProcessCode, sf.SourceFileID, sf.FileName, LastUploaded,OriginalCount, 
--		ISNULL(TransformedCount,0) as 'TransformedCount', ISNULL(ArchivedCount,0) as 'ArchivedCount', ISNULL(InvalidCount,0) as 'InvalidCount', 
--		ISNULL(FinalCount,0) as 'FinalCount', ISNULL(MasterCount,0) as 'MasterCount', ISNULL(SubordinateCount,0) as 'SubordinateCount'
--	FROM UAS..sourcefile sf WITH(NoLock)  
--	JOIN
--		(
--			SELECT so.SourceFileID, so.ProcessCode, COUNT(SubscriberOriginalID) as 'OriginalCount', MIN(so.DateCreated) as 'LastUploaded'
--			FROM	subscriberoriginal so WITH(NoLock)
--			GROUP BY so.sourcefileID, so.ProcessCode
--		)
--		o on o.SourceFileID = sf.SourceFileID
--	LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberTransformedID) as 'TransformedCount'
--			FROM SubscriberTransformed so WITH(NoLock)
--			GROUP BY so.sourcefileID, so.ProcessCode
--			)
--		t on t.SourceFileID = o.SourceFileID  and t.ProcessCode  = o.ProcessCode
--		LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
--			FROM SubscriberArchive so with (NOLOCK)
--			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
--			)
--		y on y.SourceFileID = o.SourceFileID and y.ProcessCode  = o.ProcessCode
--	LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberFinalID) as 'FinalCount'
--			FROM SubscriberFinal so WITH(NoLock)
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		z on z.SourceFileID = o.SourceFileID and z.ProcessCode  = o.ProcessCode
--	LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(SubscriberInvalidID) as 'InvalidCount'
--			FROM SubscriberInvalid so WITH(NoLock)
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		i on i.SourceFileID = o.SourceFileID  and i.ProcessCode  = o.ProcessCode
--		LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'MasterCount'
--			FROM SubscriberFinal so WITH(NoLock)
--			WHERE so.IGrp_Rank = 'M'
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		r on r.SourceFileID = o.SourceFileID and r.ProcessCode  = o.ProcessCode
--		LEFT OUTER JOIN
--		(
--			SELECT so.SourceFileID, ProcessCode, COUNT(IGrp_Rank) as 'SubordinateCount'
--			FROM SubscriberFinal so WITH(NoLock)
--			WHERE so.IGrp_Rank = 'S'
--			GROUP BY so.SourceFileID, so.ProcessCode
--			)
--		s on s.SourceFileID = o.SourceFileID and s.ProcessCode  = o.ProcessCode
--	WHERE sf.SourceFileID = @SourceFileID AND o.ProcessCode = @ProcessCode 
--	ORDER BY OriginalCount asc
--GO

END