CREATE Proc [dbo].[e_Subscriber_BulkUpdate_WaveMailing]
(  
	@SubscriberXML TEXT,
	@WaveMailingID int
) AS

CREATE TABLE #Subs
(  
	RowID int IDENTITY(1, 1)
  ,[SubscriberID] int
)

DECLARE @docHandle int

EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriberXML  
INSERT INTO #Subs 
(
	 SubscriberID
)  
SELECT [SubscriberID]
FROM OPENXML(@docHandle,N'/XML/Subscriber')
WITH
(
	[SubscriberID] int 'SubID'
)

UPDATE Subscriber
SET IsInActiveWaveMailing = 1, WaveMailingID = @WaveMailingID
FROM Subscriber s
INNER JOIN #Subs s2
ON s.SubscriberID = s2.SubscriberID

DROP TABLE #Subs
