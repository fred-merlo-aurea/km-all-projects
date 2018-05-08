CREATE PROCEDURE rpt_FileCounts_ClientID
@ClientID int,
@ClientName varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON

	SELECT @ClientID, @ClientName as 'ClientName',so.SourceFileID,s.FileName,CAST(so.DateCreated as date) as 'DateCreated',
		Count(so.SORecordIdentifier) as 'OriginalCount',
		Count(si.SIRecordIdentifier) as 'InvalidCount',
		Count(st.STRecordIdentifier) as 'TransformedCount',
		Count(sa.SARecordIdentifier) as 'ArchivedCount',
		Count(sf.SFRecordIdentifier) as 'FinalCount'
	FROM SubscriberOriginal so With(NoLock)
		JOIN UAS..SourceFile s With(NoLock) ON so.SourceFileID = s.SourceFileID
		LEFT JOIN SubscriberInvalid si With(NoLock) ON so.SORecordIdentifier = si.SORecordIdentifier
		LEFT JOIN SubscriberTransformed st With(NoLock) ON so.SORecordIdentifier = st.SORecordIdentifier
		LEFT JOIN SubscriberFinal sf With(NoLock) ON st.STRecordIdentifier = sf.STRecordIdentifier
		LEFT JOIN SubscriberArchive sa With(NoLock) ON sf.SFRecordIdentifier = sa.SFRecordIdentifier
	WHERE s.ClientID = @ClientID
	GROUP BY s.ClientID, so.SourceFileID,s.FileName,CAST(so.DateCreated as date)
	ORDER BY 'ClientName',CAST(so.DateCreated as date),s.FileName

END
GO