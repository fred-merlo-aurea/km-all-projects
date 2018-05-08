CREATE Proc [dbo].[e_ProductSubscription_BulkUpdate_WaveMailing]
(  
	@SubscriptionXML TEXT,
	@WaveMailingID int
) AS
BEGIN

	set nocount on

	CREATE TABLE #Subs
	(  
		RowID int IDENTITY(1, 1)
	  ,[PubSubscriptionID] int
	  ,[IMBSEQ] varchar(100)
	)

	DECLARE @docHandle int

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriptionXML  
	INSERT INTO #Subs 
	(
		 PubSubscriptionID, IMBSEQ
	)  
	SELECT [PubSubscriptionID], [IMBSEQ]
	FROM OPENXML(@docHandle,N'/XML/S')
	WITH
	(
		[PubSubscriptionID] int 'ID',
		[IMBSEQ] varchar(100) 'IMB'
	)

	UPDATE PubSubscriptions
	SET IsInActiveWaveMailing = 1, WaveMailingID = @WaveMailingID, IMBSEQ = ISNULL(s2.IMBSEQ, '0')
	FROM PubSubscriptions s
	INNER JOIN #Subs s2
	ON s.PubSubscriptionID = s2.PubSubscriptionID

	INSERT INTO WaveMailSubscriber (WaveMailingID, PubSubscriptionID, WaveNumber)
	SELECT wm.WaveMailingID, ps.PubSubscriptionID, wm.WaveNumber FROM #Subs s
	JOIN PubSubscriptions ps ON ps.PubSubscriptionID = s.PubSubscriptionID
	JOIN WaveMailing wm ON wm.WaveMailingID = ps.WaveMailingID

	DROP TABLE #Subs

END