
CREATE PROCEDURE e_SubscriberTransformed_AddressValidation_Paging
@CurrentPage int, 
@PageSize int,
@ProcessCode varchar(50) = '',
@SourceFileID int = 0,
@IsLatLonValid bit = 'false'
AS
BEGIN
	-- The number of rows affected by the different commands-- does not interest the application, so turn NOCOUNT ON
	SET NOCOUNT ON
	-- Determine the first record and last record 
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);

	CREATE TABLE #TempResult 
	(
		RowNum int,
		[SubscriberTransformedID] [int] NOT NULL, [SORecordIdentifier] [uniqueidentifier] NOT NULL, [SourceFileID] [int] NOT NULL, 
		[PubCode] [varchar](100) NULL, [Sequence] [int] NOT NULL, [FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,
		[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](50) NULL,[Plus4] [varchar](50) NULL,
		[ForZip] [varchar](50) NULL,[County] [varchar](100) NULL,[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,
		[FaxExists] [bit] NULL,[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [datetime] NULL,[QDate] [datetime] NULL,
		[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](100) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](1) NULL,[Demo31] [bit] NULL,
		[Demo32] [bit] NULL,[Demo33] [bit] NULL,[Demo34] [bit] NULL,[Demo35] [bit] NULL,[Demo36] [bit] NULL,[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,[IGrp_No] [uniqueidentifier] NULL,
		[IGrp_Cnt] [int] NULL,[CGrp_No] [int] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,[Gender] [varchar](1024) NULL,
		[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,
		[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,
		[Score] [int] NULL,[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,
		[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,
		[UpdateInLiveDate] [datetime] NULL,[STRecordIdentifier] [uniqueidentifier] NOT NULL,[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,
		[UpdatedByUserID] [datetime] NULL,ProcessCode varchar(50) NOT NULL, ImportRowNumber int NULL, IsActive bit null
	)

	IF LEN(@ProcessCode) > 0
		BEGIN

			INSERT INTO #TempResult
			SELECT ROW_NUMBER() OVER(ORDER BY st.SubscriberTransformedID ASC) as 'RowNum'
			  ,st.*
			FROM SubscriberTransformed st WITH (NOLOCK)
			WHERE ProcessCode = @ProcessCode
			AND SourceFileID = @SourceFileID
			AND IsLatLonValid = @IsLatLonValid
		END
	ELSE
		BEGIN
			IF @SourceFileID > 0
				BEGIN
					INSERT INTO #TempResult
					SELECT ROW_NUMBER() OVER(ORDER BY st.SubscriberTransformedID ASC) as 'RowNum'
					  ,st.*
					FROM SubscriberTransformed st WITH (NOLOCK)
					WHERE SourceFileID = @SourceFileID
					AND IsLatLonValid = @IsLatLonValid
				END
			ELSE
				BEGIN
					INSERT INTO #TempResult
					SELECT ROW_NUMBER() OVER(ORDER BY st.SubscriberTransformedID ASC) as 'RowNum'
					  ,st.*
					FROM SubscriberTransformed st WITH (NOLOCK)
					WHERE IsLatLonValid = @IsLatLonValid
				END
		END

	------------------Return results
	SELECT top (@LastRec-1) *
	FROM #TempResult
	WHERE RowNum > @FirstRec 
	AND RowNum < @LastRec
	-- Turn NOCOUNT back OFF
	SET NOCOUNT OFF
	DROP TABLE #TempResult

END
GO