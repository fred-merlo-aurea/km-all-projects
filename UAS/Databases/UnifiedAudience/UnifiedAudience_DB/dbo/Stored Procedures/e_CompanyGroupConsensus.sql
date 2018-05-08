CREATE PROCEDURE [dbo].[e_CompanyGroupConsensus]   
AS
BEGIN

	SET NOCOUNT ON
	
	--SET CGRP_NO = NULL IN SUBSCRIPTIONS TABLE FOR ALL RECORDS

	UPDATE Subscriptions 
	SET CGRP_NO = NULL

	--SET CGRP_NO FOR RECORDS WITH COMPANY & MORE THAN ONE MATCH ON COMPANY, ADDRESS, CITY, STATE, AND COUNTRYID
	----THIS WILL UPDATE RECORDS WITH COMPANY DATA REGARDLESS IF DATA IS PRESENT IN ADDRESS FIELDS SO LONG AS EVERYTHING MATCHES

	UPDATE Subscriptions
	SET CGRP_NO = ID
	FROM Subscriptions s 
		JOIN (
			SELECT DISTINCT 
				ISNULL(COMPANY, '') COMPANY,
				ISNULL(ADDRESS,'') ADDRESS,
				ISNULL(CITY,'') CITY,
				ISNULL(State,'') STATE,
				ISNULL(CountryID,'') CTRY,
				NEWID() AS ID
			FROM Subscriptions
			WHERE ISNULL(COMPANY, '') <> ''
			GROUP BY 
				Company,
				ADDRESS,
				CITY,
				STATE,
				CountryID
			HAVING COUNT(*) > 1) x 
		ON
			ISNULL(s.COMPANY, '') = ISNULL(x.COMPANY, '') AND
			ISNULL(LEFT(s.ADDRESS,15),'') = ISNULL(LEFT(x.ADDRESS,15),'') AND
			ISNULL(s.CITY,'') = ISNULL(x.city,'') AND
			ISNULL(s.STATE,'') = ISNULL(x.state,'') AND
			ISNULL(s.CountryID,'') = ISNULL(x.ctry,'')

	--SET CGRP_NO FOR RECORDS WITHOUT COMPANY, BUT ADDRESS INFORMATION IS A MATCH
	----THIS WILL ONLY UPDATE RECORDS WHERE ADDRESS DATA IS PRESENT IN ADDRESS, CITY, STATE, AND COUNTRYID AND HAS A MATCH
	UPDATE Subscriptions
	SET CGRP_NO = ID
	FROM Subscriptions s 
		JOIN (
			SELECT DISTINCT 
				ISNULL(COMPANY, '') COMPANY,
				ISNULL(ADDRESS,'') ADDRESS,
				ISNULL(CITY,'') CITY,
				ISNULL(State,'') STATE,
				ISNULL(CountryID,'') CTRY,
				NEWID() AS id 
			FROM subscriptions
			WHERE 
				ISNULL(COMPANY, '') = '' 
				AND CGRP_NO IS NULL 
				AND LEN(ADDRESS) > 7 
				AND ISNULL(city,'') <>'' 
				AND ISNULL(countryid,'') <>''
			GROUP BY 
				company,
				ADDRESS,
				CITY,
				STATE,
				CountryID
			HAVING 
				COUNT(*) > 1) X 
		ON 
			ISNULL(s.COMPANY, '') = ISNULL(x.COMPANY, '') AND 
			ISNULL(LEFT(s.ADDRESS,15),'') = ISNULL(LEFT(x.ADDRESS,15),'') AND
			ISNULL(s.CITY,'') = ISNULL(x.city,'') AND
			ISNULL(s.STATE,'') = ISNULL(x.state,'') AND
			ISNULL(s.CountryID,'') = ISNULL(x.ctry,'')

	--SET CGRP_NO FOR UNIQUE RECORDS THAT DID NOT MATCH ABOVE 2 CONDITIONS
	--ALL RECORDS WILL NOW HAVE CGRP_NO POPULATED

	UPDATE Subscriptions
	SET CGRP_NO = NEWID()
	WHERE CGRP_NO IS NULL 

	--CLEAR CompanyGroupDetails TABLE
	TRUNCATE TABLE CompanyGroupDetails

	--Build consensus for Cgrp_no
	INSERT INTO CompanyGroupDetails(
		CGRP_NO,
		MASTERID
		)
	SELECT DISTINCT s.cgrp_no, cmb.masterID
	FROM Subscriptions s 
		JOIN PubSubscriptionDetail psd ON s.SubscriptionID = psd.SubscriptionID 
		JOIN CodeSheet_Mastercodesheet_Bridge cmb WITH (NOLOCK) ON psd.CodesheetID = cmb.CodeSheetID

	--CLEAR CompanyGroupMasterValues TABLE
	TRUNCATE TABLE CompanyGroupMasterValues

	--Build MasterValues for Cgrp_no
	INSERT INTO CompanyGroupMasterValues (
		MasterGroupID,
		CGRP_NO,
		CombinedValues
		)
	SELECT MasterGroupID, cgrp_no , 
		STUFF((
			SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
			FROM CompanyGroupDetails sd1 WITH (NOLOCK)  
				JOIN Mastercodesheet mc1 WITH (NOLOCK) ON sd1.MasterID = mc1.MasterID  
			WHERE (sd1.cgrp_no = Results.cgrp_no AND mc1.MasterGroupID = Results.MasterGroupID) 
			FOR XML PATH ('')),1,1,'') AS CombinedValues
	FROM 
		(SELECT DISTINCT t.cgrp_no, mc.MasterGroupID
		FROM CompanyGroupDetails t WITH (NOLOCK)
			JOIN Mastercodesheet mc WITH (NOLOCK) ON t.MasterID = mc.MasterID                    
		 ) Results
	GROUP BY cgrp_no, MasterGroupID
	ORDER BY cgrp_no    

END