CREATE PROCEDURE [dbo].[o_SubscriptionSearchResult_Select_SubscriberID_Multiple]
@xml xml
AS
	SET NOCOUNT ON  	

	DECLARE @docHandle int
    
	CREATE TABLE #import
	(  
      [SubscriberID] int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 [SubscriberID]
	)  
	
	SELECT [SubscriberID]
	FROM OPENXML(@docHandle,N'/XML/SubscriptionSearchResult')
	WITH
	(
		[SubscriberID] int 'SubscriberID'
	)
	
	EXEC sp_xml_removedocument @docHandle


	SELECT s.SubscriptionID,s.SequenceID,RTRIM(LTRIM(ISNULL(sr.Address1, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(sr.Address2,''))) + ', ' + RTRIM(LTRIM(ISNULL(sr.City, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(sr.RegionCode,''))) + ', ' + 
		(case when sr.CountryID = 1 and ISNULL(sr.ZipCode,'')!='' AND ISNULL(sr.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + '-' + sr.Plus4 
			  when sr.CountryID = 2 and ISNULL(sr.ZipCode,'')!='' AND ISNULL(sr.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + ' ' + sr.Plus4 
			  else RTRIM(LTRIM(ISNULL(sr.ZipCode, ''))) + RTRIM(LTRIM(ISNULL(sr.Plus4,''))) end)+ ', ' + 
		RTRIM(LTRIM(ISNULL(sr.Country, ''))) as 'FullAddress',
		   '' as ProductCode,sr.Phone,sr.Email,s.PublicationID as ProductID,s.PublisherID as ClientID,s.SubscriberID,
		   '' as ClientName,sr.Company,s.IsSubscribed,s.AccountNumber,ISNULL(uc.PhonePrefix,'0') AS PhoneCode,
		   s.SubscriptionStatusID
	FROM Subscription s With(NoLock)
	JOIN Subscriber sr With(NoLock) ON sr.SubscriberID = s.SubscriberID
	--JOIN Publication cation With(NoLock) ON s.PublicationID = cation.PublicationID
	--JOIN Publisher p With(NoLock) ON cation.PublisherID = p.PublisherID
	LEFT JOIN UAS..Country uc WITH(Nolock) ON sr.CountryID = uc.CountryID
	WHERE s.SubscriberID in (Select [SubscriberID] from #import)
