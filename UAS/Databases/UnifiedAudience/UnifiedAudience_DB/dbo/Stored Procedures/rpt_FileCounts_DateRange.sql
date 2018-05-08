CREATE PROCEDURE rpt_FileCounts_DateRange
@StartDate date,
@EndDate date
AS
BEGIN 
	
	SET NOCOUNT ON

	SELECT s.ClientID,c.ClientName,so.SourceFileID,s.FileName,CAST(so.DateCreated as date) as 'DateCreated',
		Count(so.SORecordIdentifier) as 'OriginalCount',
		Count(si.SIRecordIdentifier) as 'InvalidCount',
		Count(st.STRecordIdentifier) as 'TransformedCount',
		Count(sa.SARecordIdentifier) as 'ArchivedCount',
		Count(sf.SFRecordIdentifier) as 'FinalCount'
	FROM SubscriberOriginal so With(NoLock)
		JOIN UAS..SourceFile s With(NoLock) ON so.SourceFileID = s.SourceFileID
		JOIN UAS..Client c With(NoLock) ON s.ClientID = c.ClientID
		LEFT JOIN SubscriberInvalid si With(NoLock) ON so.SORecordIdentifier = si.SORecordIdentifier
		LEFT JOIN SubscriberTransformed st With(NoLock) ON so.SORecordIdentifier = st.SORecordIdentifier
		LEFT JOIN SubscriberFinal sf With(NoLock) ON st.STRecordIdentifier = sf.STRecordIdentifier
		LEFT JOIN SubscriberArchive sa With(NoLock) ON sf.SFRecordIdentifier = sa.SFRecordIdentifier
	WHERE CAST(so.DateCreated as date) BETWEEN @StartDate AND @EndDate
	GROUP BY s.ClientID,c.ClientName,so.SourceFileID,s.FileName,CAST(so.DateCreated as date)
	ORDER BY c.ClientName,CAST(so.DateCreated as date),s.FileName

END