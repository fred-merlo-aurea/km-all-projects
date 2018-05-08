CREATE proc [dbo].[rpt_SubSrcSummary]
(
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000), 
	@PrintColumns varchar(4000),
	@Download char(1),
	@WaveMail varchar(100) = ''  
)
AS
BEGIN
	
	SET NOCOUNT ON

	BEGIN --SET UP

		CREATE TABLE #SubscriptionID (SubscriptionID int, Copies int)  
	
		INSERT INTO #SubscriptionID   
		EXEC rpt_GetSubscriptionIDs_Copies_From_Filter 
		@PublicationID, 
		@CategoryIDs,
		@CategoryCodes,
		@TransactionIDs,
		@TransactionCodes,
		@QsourceIDs,
		@StateIDs,
		@CountryIDs,
		@Email,
		@Phone,
		@Mobile,
		@Fax,
		@ResponseIDs,
		@Demo7,		
		@Year,
		@startDate,		
		@endDate,
		@AdHocXML

		CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	END

	BEGIN

		SELECT ISNULL(NULLIF(SubscriberSourceCode, ''), 'ZZ. No Response') as 'SUBSRC', COUNT(*) as TOTAL
		FROM PubSubscriptions ps 
			JOIN #SubscriptionID s ON ps.SubscriptionID = s.SubscriptionID 
		WHERE PubID = @PublicationID
		GROUP BY ISNULL(NULLIF(SubscriberSourceCode, ''), 'ZZ. No Response')
		ORDER BY ISNULL(NULLIF(SubscriberSourceCode, ''), 'ZZ. No Response')

		DROP TABLE #SubscriptionID

	END

END