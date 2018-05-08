CREATE PROCEDURE [dbo].[o_SubscriptionSearchResult_Select_SubscriberID_Multiple]
@xml xml
AS
BEGIN
	
	SET NOCOUNT ON	

	DECLARE @docHandle int
    
	CREATE TABLE #import
	(  
      [SubscriptionID] int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 [SubscriptionID]
	)  
	
	SELECT [SubscriptionID]
	FROM OPENXML(@docHandle,N'/XML/SubscriptionSearchResult')
	WITH
	(
		[SubscriptionID] int 'SubscriptionID'
	)
	
	EXEC sp_xml_removedocument @docHandle


	SELECT s.SubscriptionID,s.Sequence,RTRIM(LTRIM(ISNULL(s.Address, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(s.MailStop,''))) + ', ' + RTRIM(LTRIM(ISNULL(s.City, ''))) + ', ' + 
		RTRIM(LTRIM(ISNULL(s.State,''))) + ', ' + 
		(case when s.CountryID = 1 and ISNULL(s.Zip,'')!='' AND ISNULL(s.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(s.Zip, ''))) + '-' + s.Plus4 
			  when s.CountryID = 2 and ISNULL(s.Zip,'')!='' AND ISNULL(s.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(s.Zip, ''))) + ' ' + s.Plus4 
			  else RTRIM(LTRIM(ISNULL(s.Zip, ''))) + RTRIM(LTRIM(ISNULL(s.Plus4,''))) end)+ ', ' + 
		RTRIM(LTRIM(ISNULL(s.Country, ''))) as 'FullAddress',
		   '' as ProductCode,s.Phone,s.Email,ps.PubID as ProductID,p.ClientID as ClientID,ps.PubSubscriptionID,
		   '' as ClientName,s.Company,s.IsSubscribed,s.AccountNumber,ISNULL(uc.PhonePrefix,'0') AS PhoneCode,
		   s.SubscriptionStatusID
	FROM Subscriptions s With(NoLock) 
		join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID
		join Pubs p on ps.PubID = p.PubID
		LEFT JOIN UAD_Lookup..Country uc WITH(Nolock) ON s.CountryID = uc.CountryID
	WHERE s.SubscriptionID in (Select [SubscriptionID] from #import)

END