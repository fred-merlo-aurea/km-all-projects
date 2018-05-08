CREATE PROCEDURE [dbo].[e_Subscriptions_GetInValidLatLon_Range]
@Start int,
@End int
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT	SubscriptionID,
			ADDRESS,
			CITY,
			State,
			ltrim(rtrim(ZIP)) as zip,
			--substring(ltrim(rtrim(ZIP)),1,6)  as  zip,
			COUNTRY,
			Latitude,
			Longitude,
			IsLatLonValid,
			LatLonMsg 
	FROM Subscriptions WITH(NOLOCK)
	WHERE isnull(IsLatLonValid,0) = 0
		AND (LatLonMsg='not done' OR LatLonMsg = '' OR LatLonMsg IS NULL) 
		and ((STATE <> 'FO' OR LEN(isnull(State,'')) > 0) OR LEN(isnull(zip,'')) > 0)
		and LEN(ADDRESS) > 0
		and ADDRESS NOT LIKE 'PO BOX%'
		and CountryID in (1,2)
		AND SubscriptionID BETWEEN @Start AND @End

END