CREATE PROCEDURE [e_ProductSubscription_Select_SubscriptionID]  
@SubscriptionID int,
@ClientDisplayName varchar(100)
AS 
BEGIN

	SET NOCOUNT ON
 
	SELECT ps.*, 
		P.PubCode,
		P.PubName,
		pt.PubTypeDisplayName,
		@ClientDisplayName as 'ClientName',
		ps.FirstName + ' ' + ps.LastName as 'FullName',
		case when ps.CountryID = 1 and ISNULL(ps.ZipCode,'')!='' AND ISNULL(ps.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(ps.ZipCode, ''))) + '-' + ps.Plus4 
			  when ps.CountryID = 2 and ISNULL(ps.ZipCode,'')!='' AND ISNULL(ps.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(ps.ZipCode, ''))) + ' ' + ps.Plus4 
			  else RTRIM(LTRIM(ISNULL(ps.ZipCode, ''))) + RTRIM(LTRIM(ISNULL(ps.Plus4,''))) end as 'FullZip',
		isnull(cty.PhonePrefix,1) as 'PhoneCode',
		RTRIM(LTRIM(ISNULL(ps.[Address1], ''))) + ', ' + RTRIM(LTRIM(ISNULL(ps.Address2,''))) + ', ' + RTRIM(LTRIM(ISNULL(ps.City, ''))) + ', ' + RTRIM(LTRIM(ISNULL(ps.RegionCode,''))) + ', ' + 
		(case when ps.CountryID = 1 and ISNULL(ps.ZipCode,'')!='' AND ISNULL(ps.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(ps.ZipCode, ''))) + '-' + ps.Plus4 
			  when ps.CountryID = 2 and ISNULL(ps.ZipCode,'')!='' AND ISNULL(ps.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(ps.ZipCode, ''))) + ' ' + ps.Plus4 
			  else RTRIM(LTRIM(ISNULL(ps.ZipCode, ''))) + RTRIM(LTRIM(ISNULL(ps.Plus4,''))) end)+ ', ' + 
		RTRIM(LTRIM(ISNULL(ps.Country, ''))) as 'FullAddress'
	FROM PubSubscriptions ps With(NoLock)   
		JOIN Pubs p With(NoLock)ON p.PubID = ps.PubID   
		JOIN PubTypes pt With(NoLock) ON pt.PubTypeID = p.PubTypeID 
		LEFT JOIN UAD_Lookup..Country cty with(nolock) on ps.CountryID = cty.CountryID
		--JOIN EmailStatus es With(NoLock) ON es.EmailStatusID = isnull(ps.EmailStatusID, 1)
	WHERE ps.SubscriptionID = @SubscriptionID

END