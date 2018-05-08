create procedure job_QSourceValidation
@SourceFileID int,
@ProcessCode varchar(50)
as
BEGIN

	SET NOCOUNT ON

	CREATE TABLE #tmpInvalidQSourceID
	(
		SubscriberTransformedID INT,
		RowNumber INT,
		PubCode varchar(100),
		QSourceID int
	)
	CREATE CLUSTERED INDEX PK_tmpInvalidQSourceID_SubscriberTransformedID ON #tmpInvalidQSourceID(SubscriberTransformedID)
	
	INSERT INTO #tmpInvalidQSourceID
	SELECT DISTINCT SubscriberTransformedID,
		st.ImportRowNumber, 
		st.PubCode, 
		st.QSourceID
	FROM dbo.SubscriberTransformed st WITH (NOLOCK)
		LEFT OUTER JOIN UAD_Lookup..Code q WITH (NOLOCK) ON st.QSourceID = q.CodeId
	WHERE st.SourceFileID = @sourceFileID 
		AND st.ProcessCode = @ProcessCode 
		AND q.CodeId IS NULL
	---------------------------------------------------------------
	
	INSERT INTO ImportError (SourceFileID,RowNumber,FormattedException,ClientMessage,MAFField,BadDataRow,ThreadID,DateCreated,ProcessCode,IsDimensionError)
	
	SELECT @sourcefileID,t.RowNumber,'QSourceID:' + CAST(t.QSourceID as varchar(20)) + ' does not exist',
		'QSourceID:' + CAST(t.QSourceID as varchar(20)) + ' does not exist','QSourceID','',-1,GETDATE(),@ProcessCode,'false'
	FROM #tmpInvalidQSourceID t WITH(NOLOCK)
		
	DROP TABLE #tmpInvalidQSourceID

END
go