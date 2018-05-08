CREATE PROCEDURE rpt_SourceFile_PubCodeSummary

	@SourceFileID int,
	@ProcessCode  varchar(50),
	@FileName varchar(100)

AS
BEGIN
	SET NOCOUNT ON

	DECLARE	@SourceFileID_Local int,
		@ProcessCode_Local  varchar(50),
		@FileName_Local varchar(100)

	SET @SourceFileID_Local = @SourceFileID
	SET @ProcessCode_Local = @ProcessCode
	SET @FileName_Local = @FileName

	SELECT @SourceFileID_Local as SourceFileID, 
		@FileName_Local as FileName, 
		x.ProcessCode, 
		LastUploaded,  
		TransformedPubCode, 
		ISNULL(OriginalCount,0) as 'OriginalCount', 
		ISNULL(TransformedCount,0) as 'TransformedCount', 
		ISNULL(ArchivedCount,0) as 'ArchivedCount', 
		ISNULL(InvalidCount,0) as 'InvalidCount', 
		ISNULL(FinalCount,0) as 'FinalCount', 
		ISNULL(LiveCount,0) as 'LiveCount',
		ISNULL(MasterCount,0) as 'MasterCount', 
		ISNULL(SubordinateCount,0) as 'SubordinateCount'
	FROM (
			SELECT so.SourceFileID, so.ProcessCode,  
				st.PubCode as 'TransformedPubCode', COUNT(SubscriberOriginalID) as 'OriginalCount',
				COUNT(SubscriberTransformedID) as 'TransformedCount',MAX(so.DateCreated) as 'LastUploaded'
			FROM dbo.SubscriberOriginal so with (NOLOCK) 
				left outer join dbo.subscriberTransformed st with (NOLOCK) on st.SourceFileID = so.SourceFileID and st.ProcessCode = so.ProcessCode and st.SORecordIdentifier = so.SORecordIdentifier
			WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
			GROUP BY so.SourceFileID, so.ProcessCode,st.PubCode
		)
		x
		LEFT OUTER JOIN (
			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'FinalCount'
			FROM dbo.SubscriberFinal so with (NOLOCK)
			WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
			) z on z.SourceFileID = x.SourceFileID and z.PubCode = x.TransformedPubCode and z.ProcessCode  = x.ProcessCode
		LEFT OUTER JOIN (
			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberInvalidID),0) as 'InvalidCount'
			FROM dbo.SubscriberInvalid so with (NOLOCK)
			WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
			) xx on xx.SourceFileID = x.SourceFileID and xx.PubCode = x.TransformedPubCode and xx.ProcessCode  = x.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
			FROM dbo.SubscriberArchive so with (NOLOCK)
			WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
			) y on y.SourceFileID = x.SourceFileID and y.PubCode = x.TransformedPubCode and y.ProcessCode  = x.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT P.pubcode, ISNULL(COUNT(ps.subscriptionID),0) as 'LiveCount'
			FROM	dbo.PubSubscriptions ps with (NOLOCK) 
					join dbo.Pubs p on p.pubID = ps.pubID
			GROUP BY P.pubcode
		) a on a.PubCode = z.PubCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'MasterCount'
			FROM dbo.SubscriberFinal so with (NOLOCK)
			WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
			AND so.IGrp_Rank = 'M'
			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
			) m on m.SourceFileID = x.SourceFileID and m.PubCode = x.TransformedPubCode and m.ProcessCode  = x.ProcessCode
		LEFT OUTER JOIN
		(
			SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'SubordinateCount'
			FROM dbo.SubscriberFinal so with (NOLOCK)
			WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
			AND so.IGrp_Rank = 'S'
			GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
			) s on s.SourceFileID = x.SourceFileID and s.PubCode = x.TransformedPubCode and s.ProcessCode  = x.ProcessCode
	ORDER BY OriginalCount ASC
	--CREATE PROCEDURE rpt_SourceFile_PubCodeSummary

	--	@SourceFileID int,
	--	@ProcessCode  varchar(50)

	--AS

	--SET NOCOUNT ON

	--DECLARE	@SourceFileID_Local int,
	--		@ProcessCode_Local  varchar(50)

	--SET @SourceFileID_Local = @SourceFileID
	--SET @ProcessCode_Local = @ProcessCode

	--SELECT	
	--	sf.SourceFileID, 
	--	sf.FileName, 
	--	x.ProcessCode, 
	--	LastUploaded,  
	--	TransformedPubCode, 
	--	ISNULL(OriginalCount,0) as 'OriginalCount', 
	--	ISNULL(TransformedCount,0) as 'TransformedCount', 
	--	ISNULL(ArchivedCount,0) as 'ArchivedCount', 
	--	ISNULL(InvalidCount,0) as 'InvalidCount', 
	--	ISNULL(FinalCount,0) as 'FinalCount', 
	--	ISNULL(LiveCount,0) as 'LiveCount',
	--	ISNULL(MasterCount,0) as 'MasterCount', 
	--	ISNULL(SubordinateCount,0) as 'SubordinateCount'
	--FROM 
	--	UAS.dbo.sourcefile sf WITH(NoLock)
	--JOIN
	--	(
	--		SELECT	so.SourceFileID, so.ProcessCode,  
	--				st.PubCode as 'TransformedPubCode', COUNT(SubscriberOriginalID) as 'OriginalCount',
	--				COUNT(SubscriberTransformedID) as 'TransformedCount',MAX(so.DateCreated) as 'LastUploaded'
	--		FROM	dbo.SubscriberOriginal so with (NOLOCK) 
	--				left outer join dbo.subscriberTransformed st with (NOLOCK) on st.SourceFileID = so.SourceFileID and st.ProcessCode = so.ProcessCode and st.SORecordIdentifier = so.SORecordIdentifier
	--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
	--		GROUP BY so.SourceFileID, so.ProcessCode,st.PubCode
	--	)
	--	x on x.SourceFileID = sf.SourceFileID
	--LEFT OUTER JOIN
	--	(
	--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'FinalCount'
	--		FROM dbo.SubscriberFinal so with (NOLOCK)
	--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
	--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
	--		)
	--	z on z.SourceFileID = x.SourceFileID and z.PubCode = x.TransformedPubCode and z.ProcessCode  = x.ProcessCode
	--LEFT OUTER JOIN
	--	(
	--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberInvalidID),0) as 'InvalidCount'
	--		FROM dbo.SubscriberInvalid so with (NOLOCK)
	--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
	--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
	--		)
	--	xx on xx.SourceFileID = x.SourceFileID and xx.PubCode = x.TransformedPubCode and xx.ProcessCode  = x.ProcessCode
	--LEFT OUTER JOIN
	--	(
	--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberArchiveID),0) as 'ArchivedCount'
	--		FROM dbo.SubscriberArchive so with (NOLOCK)
	--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
	--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
	--		)
	--	y on y.SourceFileID = x.SourceFileID and y.PubCode = x.TransformedPubCode and y.ProcessCode  = x.ProcessCode
	--LEFT OUTER JOIN
	--	(
	--		SELECT P.pubcode, ISNULL(COUNT(ps.subscriptionID),0) as 'LiveCount'
	--		FROM	dbo.PubSubscriptions ps with (NOLOCK) 
	--				join dbo.Pubs p on p.pubID = ps.pubID
	--		GROUP BY P.pubcode
	--	)
	--	a on a.PubCode = z.PubCode
	--LEFT OUTER JOIN
	--	(
	--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'MasterCount'
	--		FROM dbo.SubscriberFinal so with (NOLOCK)
	--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
	--		AND so.IGrp_Rank = 'M'
	--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
	--		)
	--	m on m.SourceFileID = x.SourceFileID and m.PubCode = x.TransformedPubCode and m.ProcessCode  = x.ProcessCode
	--LEFT OUTER JOIN
	--	(
	--		SELECT so.SourceFileID, ProcessCode,  so.PubCode, ISNULL(COUNT(SubscriberFinalID),0) as 'SubordinateCount'
	--		FROM dbo.SubscriberFinal so with (NOLOCK)
	--		WHERE so.SourceFileID = @SourceFileID_Local AND so.ProcessCode = @ProcessCode_Local
	--		AND so.IGrp_Rank = 'S'
	--		GROUP BY so.SourceFileID, so.ProcessCode,so.PubCode
	--		)
	--	s on s.SourceFileID = x.SourceFileID and s.PubCode = x.TransformedPubCode and s.ProcessCode  = x.ProcessCode
	--WHERE 
	--	sf.SourceFileID = @SourceFileID_Local 
	--ORDER BY 
	--	OriginalCount ASC

END