
CREATE PROC dbo.sp_Pharma_eNewsletters_Group_Update_NEW

AS

SET NOCOUNT ON 

Begin
	--removed -- ON 2/28/2013 -- SELECT groupID, groupname FROM groups WHERE groupID in (	32145,58482,53169,14094,103243,34851)
	SET NOCOUNT ON

	DECLARE @currentdate DATETIME
	SET @currentdate = GETDATE()

	--ADDED ON 4/7/2014
	DECLARE @dt DATE
	SET @dt = GETDATE() - 1


	/* fix for data FROM old subscription form*/
	INSERT INTO emaildatavalues (
		EmailID,
		GroupDatafieldsID,
		DataValue,
		ModifiedDate,
		SurveyGridID,
		EntryID)
	SELECT DISTINCT 
		eg.emailID, 
		groupdatafieldsID, 
		'FREE', 
		getdate(), 
		-1, 
		null
	FROM 
		emailgroups eg  WITH (NOLOCK)
		JOIN (SELECT 
				groupID, 
				groupdatafieldsID 
			  FROM 
				groupdatafields WITH (NOLOCK)
			 WHERE 
				shortname = 'paidorfree' 
				and groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060)) gdf ON eg.groupID = gdf.groupID 
	WHERE 
		eg.groupID in (	14092,14093,40065,14095,14096,14098,14099,14100,14101,14102,14104,14105,16201,16585,121238,154060) 
		and not exists (SELECT
							emaildatavaluesID 
						FROM 
							emaildatavalues edv  WITH (NOLOCK)
						WHERE 
							edv.emailID = eg.emailID 
							and edv.groupdatafieldsID = gdf.groupdatafieldsID) 
		--and eg.subscribetypecode = 'S'

	INSERT INTO emaildatavalues (
		EmailID,
		GroupDatafieldsID,
		DataValue,
		ModifiedDate,
		SurveyGridID,
		EntryID)
	SELECT DISTINCT 
		eg.emailID, 
		groupdatafieldsID, 
		eg.emailID, 
		getdate(), 
		-1, 
		null
	FROM 
		emailgroups eg  WITH (NOLOCK)
		JOIN (SELECT 
				groupID, 
				groupdatafieldsID 
			  FROM 
				groupdatafields  WITH (NOLOCK) 
			  WHERE 
				shortname = 'Sub_Account_Number' 
				and groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060)) gdf ON eg.groupID = gdf.groupID 
	WHERE 
		eg.groupID in (14092,14093,40065,14095,14096,14098,14099,14100,14101,14102,14104,14105,16201,16585,121238,154060) 
		and not exists (SELECT 
							emaildatavaluesID 
						FROM 
							emaildatavalues edv  WITH (NOLOCK)
						WHERE 
							edv.emailID = eg.emailID 
							and edv.groupdatafieldsID = gdf.groupdatafieldsID) 
		and eg.subscribetypecode = 'S'

	INSERT INTO emaildatavalues (
		EmailID,
		GroupDatafieldsID,
		DataValue,
		ModifiedDate,
		SurveyGridID,
		EntryID)
	SELECT DISTINCT 
		eg.emailID, 
		groupdatafieldsID, 
		CONVERT(VARCHAR(10),(CASE WHEN lastchanged IS NULL THEN createdon ELSE lastchanged END),101), 
		CONVERT(VARCHAR(10),(CASE WHEN lastchanged IS NULL THEN createdon ELSE lastchanged END),101), 
		-1, 
		null
	FROM 
		emailgroups eg  WITH (NOLOCK)
		JOIN (SELECT 
				groupID, 
				groupdatafieldsID 
			  FROM 
				groupdatafields WITH (NOLOCK)
			  WHERE 
				shortname = 'Verification_Date' 
				and groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060)) gdf ON eg.groupID = gdf.groupID 
	WHERE 
		eg.groupID in (14092,14093,40065,14095,14096,14098,14099,14100,14101,14102,14104,14105,16201,16585,121238,154060) 
		and not exists (SELECT 
							emaildatavaluesID 
						FROM 
							emaildatavalues edv  WITH (NOLOCK)
						WHERE 
							edv.emailID = eg.emailID 
							and edv.groupdatafieldsID = gdf.groupdatafieldsID) 
		and eg.subscribetypecode = 'S'
	
	/* ENDfix for data FROM old subscription form*/

--	DECLARE @temails TABLE (emailID  INT, groupID INT, SubscribeTypeCode VARCHAR(50), createdon DATETIME)
	CREATE TABLE #temails (emailID  INT, groupID INT, SubscribeTypeCode VARCHAR(50), createdon DATETIME)

	CREATE INDEX IDX_Temail_EmailId_GroupId ON #temails(EmailID, GroupID)
		
	INSERT INTO #temails
	SELECT	DISTINCT 
		eg.emailID, 
		eg.groupID, 
		eg.SubscribeTypeCode,
		eg.createdon 
	FROM 
		emailgroups eg  WITH (NOLOCK)
		JOIN groupdatafields gdf  WITH (NOLOCK) ON eg.groupID = gdf.groupID 
		JOIN emaildatavalues edv  WITH (NOLOCK) ON eg.emailID = edv.emailID AND gdf.groupdatafieldsID = edv.groupdatafieldsID
	WHERE	
		eg.groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060) 
		and	(
			CONVERT(Date,Isnull(LastChanged, CreatedOn)) = @dt or
			CONVERT(Date,edv.ModifiedDate) =  @dt
			)



--	CHANGED ON 4/7/2014
--			(CONVERT(VARCHAR(10),CreatedOn,101) =  CONVERT(VARCHAR(10),GETDATE()-1,101) or
--			CONVERT(VARCHAR(10),LastChanged,101) =  CONVERT(VARCHAR(10),GETDATE()-1,101) or 
--			CONVERT(VARCHAR(10),edv.ModifiedDate,101) =  CONVERT(VARCHAR(10),GETDATE()-1,101))

	print '1. completed'

	/***************************************************************************************************
	CREATE SUBACCOUNT NUMBERS IN EACH NEWSLETTER GROUPS
	***************************************************************************************************/
--	DECLARE @subaccount TABLE (emailID INT, groupdatafieldsID int)
	CREATE TABLE #subaccount (emailID INT, groupdatafieldsID int)
	CREATE INDEX IDX_subaccount_EmailId_GDFId ON #subaccount(EmailID, groupdatafieldsID)

	-- get emailID AND groupdatafieldsID for sub account number
	INSERT INTO #subaccount 
	SELECT 
		t.emailID, 
		gdf.groupdatafieldsID  
	FROM 
		#temails t 
		JOIN groupdatafields gdf  WITH (NOLOCK) ON gdf.groupID = t.groupID
	WHERE	
		shortname = 'Sub_Account_Number' 

	-- DELETE existing subaccount numbers
	DELETE FROM emaildatavalues 
	WHERE 
		emaildatavaluesID in (SELECT 
								emaildatavaluesID 
							  FROM 
								emaildatavalues edv  WITH (NOLOCK)
								JOIN #subaccount s ON edv.groupdatafieldsID= s.groupdatafieldsID AND edv.emailID = s.emailID 
							  WHERE 
								CONVERT(VARCHAR(100),edv.emailID) <> Datavalue)

	-- insert subaccount number if not exists
	INSERT INTO emaildatavalues (
		EmailID,
		GroupDatafieldsID,
		DataValue,
		ModifiedDate,
		SurveyGridID,
		EntryID)
	SELECT 
		s.emailID, 
		s.groupdatafieldsID, 
		s.emailID,
		@currentdate, 
		-1, 
		null 
	FROM 
		#subaccount s
		LEFT OUTER JOIN (SELECT 
							EmailDataValuesID, 
							edv.emailID,
							edv.groupdatafieldsID 
						 FROM 
							emaildatavalues edv  WITH (NOLOCK)
							JOIN  #subaccount s ON edv.groupdatafieldsID= s.groupdatafieldsID AND edv.emailID = s.emailID) inn ON s.emailID = inn.emailID AND s.groupdatafieldsID = inn.groupdatafieldsID
	WHERE 
		inn.emaildatavaluesID IS NULL

	/***************************************************END SUB ACCOUNT NUMBER*************************************************/


	/***************************************************************************************************
	-- COPY CURRENT PROMOCODE UDF TO ORIGINAL PROMOCODE UDF.
	***************************************************************************************************/
	
	--DECLARE @PromoCode TABLE (
	--	emailID INT, 
	--	EffortCodegdfID INT, 
	--	EffortCodeedvID BIGINT ,--int, 
	--	EffortCodedv VARCHAR(255), 
	--	OrgEffortCodegdfID INT, 
	--	OrgEffortCodeedvID BIGINT ,--int, 
	--	OrgEffortCodedv VARCHAR(255))

	CREATE TABLE #PromoCode (
		emailID INT, 
		EffortCodegdfID INT, 
		EffortCodeedvID BIGINT ,--int, 
		EffortCodedv VARCHAR(255), 
		OrgEffortCodegdfID INT, 
		OrgEffortCodeedvID BIGINT ,--int, 
		OrgEffortCodedv VARCHAR(255))

	CREATE INDEX IDX_PromoCode_EmailEffortCodes ON #PromoCode (EmailId,EffortCodegdfID,OrgEffortCodegdfID)
	
	INSERT INTO #PromoCode (
		emailID, 
		EffortCodegdfID, 
		OrgEffortCodegdfID)
	SELECT 
		t.emailID, 
		gdf.groupdatafieldsID, 
		(SELECT gdf1.groupdatafieldsID FROM groupdatafields gdf1 WHERE shortname = 'Original_Effort_Code' AND groupID = gdf.groupID)
	FROM 
		#temails t 
		JOIN groupdatafields gdf  WITH (NOLOCK) ON t.groupID = gdf.groupID
	WHERE
		shortname = 'Effort_Code'

	UPDATE 
		#PromoCode
	SET 
		EffortCodedv = datavalue,
		EffortCodeedvID = emaildatavaluesID	
	FROM 
		#PromoCode p 
		JOIN emaildatavalues edv  WITH (NOLOCK) ON p.emailID = edv.emailID AND p.EffortCodegdfID = edv.groupdatafieldsID

	DELETE FROM #PromoCode WHERE isnull(EffortCodedv,'') = ''

	UPDATE 
		#PromoCode
	SET 
		OrgEffortCodedv = datavalue,
		OrgEffortCodeedvID = emaildatavaluesID
	FROM 
		#PromoCode p 
		JOIN emaildatavalues edv  WITH (NOLOCK) ON p.emailID = edv.emailID AND p.OrgEffortCodegdfID = edv.groupdatafieldsID

	DELETE FROM #PromoCode WHERE EffortCodedv = OrgEffortCodedv

	UPDATE 
		emaildatavalues
	SET 
		datavalue = EffortCodedv
	FROM 
		emaildatavalues edv  WITH (NOLOCK)
		JOIN #PromoCode p ON edv.emaildatavaluesID = p.OrgEffortCodeedvID

	INSERT INTO emaildatavalues (
		EmailID,
		GroupDatafieldsID,
		DataValue,
		ModifiedDate,
		SurveyGridID,
		EntryID)
	SELECT 
		emailID, 
		OrgEffortCodegdfID, 
		EffortCodedv, 
		@currentdate,
		 -1, 
		 null 
	FROM 
		#PromoCode 
	WHERE 
		OrgEffortCodeedvID IS NULL 

	/*********************END PROMOCODE UPDATE*********************/

--	DECLARE @Pharma_eNewsletters_Group_14038 TABLE (EmailID	 INT, GroupID INT, scode VARCHAR(10), GdfID int)
	CREATE TABLE #Pharma_eNewsletters_Group_14038 (EmailID	 INT, GroupID INT, scode VARCHAR(10), GdfID int)
	CREATE INDEX IDX_PharmaTmp_EmailGDF ON #Pharma_eNewsletters_Group_14038 (EmailId, GdfID)

	INSERT INTO #Pharma_eNewsletters_Group_14038
	SELECT 
		EmailID, 
		groupID, 
		SubscribeTypeCode,
		case WHEN groupID = 14092 THEN 13069
			 WHEN groupID = 14093 THEN 13070
			 WHEN groupID = 40065 THEN 46779
			 WHEN groupID = 14094 THEN 13071
			 WHEN groupID = 14095 THEN 13072
			 WHEN groupID = 14096 THEN 13073
			 WHEN groupID = 53169 THEN 68141
			 WHEN groupID = 58482 THEN 79132
			 WHEN groupID = 14098 THEN 13075
			 WHEN groupID = 103243 THEN 189109
			 WHEN groupID = 14104 THEN 13085
			 WHEN groupID = 14105 THEN 13086 
			 WHEN groupID = 16201 THEN 17057 
			 WHEN groupID = 16585 THEN 17709 
			 WHEN groupID = 121238 THEN 204097
			 WHEN groupID = 154060 THEN 358521
			 WHEN groupID = 34851 THEN 36629 
			 WHEN groupID = 32145 THEN 34537 end
	FROM 
		#temails

	/* INSERT INTO EMAIL GROUPS */
	INSERT INTO EmailGroups (
		EmailID,
		GroupID,
		FormatTypeCode,
		SubscribeTypeCode,
		CreatedOn,
		LastChanged)
	SELECT DISTINCT 
		p.EmailID, 
		14038, 
		'html', 
		'S', 
		getdate(),
		NULL 
	FROM
		#Pharma_eNewsletters_Group_14038 p 
		LEFT OUTER JOIN emailgroups eg  WITH (NOLOCK) ON eg.emailID = p.emailID AND eg.groupID = 14038
	WHERE
		eg.emailgroupID IS NULL

	print ('Insert Emailgroups : ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

	-- Newsletter group to master group -- UPDATE
	UPDATE
		emaildatavalues
	SET 
		datavalue = CASE WHEN scode = 'S' THEN 'X' ELSE '' END, 
		ModifiedDate = getdate()
	FROM 
		emaildatavalues edv  WITH (NOLOCK)
		JOIN #Pharma_eNewsletters_Group_14038 p ON edv.emailID = p.emailID AND edv.groupdatafieldsID = p.GdfID

	print ('UPDATE EDV : ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

	-- Newsletter group to master group -- insert

	INSERT INTO emaildatavalues (
		EmailID, 
		GroupDatafieldsID, 
		DataValue, 
		ModifiedDate, 
		SurveyGridID,
		EntryID)
	SELECT 
		p.emailID, 
		p.gdfID, 
		'X', 
		getdate(), 
		NULL, 
		NULL 
	FROM 
		#Pharma_eNewsletters_Group_14038 p 
		LEFT OUTER JOIN emaildatavalues edv  WITH (NOLOCK) ON edv.emailID = p.emailID AND edv.groupdatafieldsID = p.GdfID
	WHERE 
		edv.EmaildatavaluesID IS NULL 
		AND scode = 'S'

	print ('Insert EDV : ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

--	DECLARE @gdv TABLE ( emailID INT, gdfID INT, datavalue VARCHAR(255) )
	CREATE TABLE #gdv ( emailID INT, gdfID INT, datavalue VARCHAR(255) )
	CREATE INDEX IDX_gdv_EmailID_GdfID ON #gdv(EmailID, GdfID)

	-- SUBDATE
	INSERT INTO #gdv 
	SELECT	
		t.emailID, 
		CASE 
			WHEN t.groupID = 14092 THEN 13883
			WHEN t.groupID = 14093 THEN 13868
			WHEN t.groupID = 40065 THEN 46782
			WHEN t.groupID = 14094 THEN 13869
			WHEN t.groupID = 14095 THEN 13870
			WHEN t.groupID = 14096 THEN 13871
			WHEN t.groupID = 53169 THEN 68144
			WHEN t.groupID = 58482 THEN 79135
			WHEN t.groupID = 14098 THEN 13873
			WHEN t.groupID = 103243 THEN 189112
			WHEN t.groupID = 14104 THEN 13879
			WHEN t.groupID = 14105 THEN 13880 
			WHEN t.groupID = 16201 THEN 17058 
			WHEN t.groupID = 16585 THEN 17710 
			WHEN t.groupID = 121238 THEN 204100 
			WHEN t.groupID = 154060 THEN 358524 
			WHEN t.groupID = 34851 THEN 36632 
			WHEN t.groupID = 32145 THEN 34540 END, 
			CONVERT(VARCHAR(10), CONVERT(DATETIME,datavalue), 101) 
	FROM 
		#temails t 
		JOIN groupdatafields gdf  WITH (NOLOCK) ON t.groupID = gdf.groupID 
		JOIN emaildatavalues edv  WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID  AND edv.emailID = t.emailID
	WHERE 
		shortname = 'Verification_Date' 
		and isnull(datavalue,'') <> ''
	  
	print ('Insert in #gdv subscribe date: ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

	-- NOofTrials
	INSERT INTO #gdv 
	SELECT
		t.emailID, 
		CASE 
			WHEN t.groupID = 14092 THEN 26921
			WHEN t.groupID = 14093 THEN 26908
			WHEN t.groupID = 40065 THEN 46780
			WHEN t.groupID = 14094 THEN 26909
			WHEN t.groupID = 14095 THEN 26910
			WHEN t.groupID = 14096 THEN 26911
			WHEN t.groupID = 53169 THEN 68142
			WHEN t.groupID = 58482 THEN 79133
			WHEN t.groupID = 14098 THEN 26913
			WHEN t.groupID = 103243 THEN 189110
			WHEN t.groupID = 14104 THEN 26919
			WHEN t.groupID = 14105 THEN 26920
			WHEN t.groupID = 16201 THEN 26922
			WHEN t.groupID = 16585 THEN 26923
			WHEN t.groupID = 121238 THEN 204098
			WHEN t.groupID = 154060 THEN 358522
			WHEN t.groupID = 34851 THEN 36630
			WHEN t.groupID = 32145 THEN 34538 END, 
			datavalue
	FROM 
		#temails t 
		JOIN groupdatafields gdf  WITH (NOLOCK) ON t.groupID = gdf.groupID 
		JOIN emaildatavalues edv  WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID  AND edv.emailID = t.emailID
	WHERE 
		shortname = 'Nooftrials' 
		and isnull(datavalue,'') <> ''
	   
	print ('Insert in #gdv No of Trials: ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

	-- PAIDORFREE
	INSERT INTO #gdv 
	SELECT	
		t.emailID, 
		CASE WHEN gdf.groupID = 14092 THEN 24209
			WHEN gdf.groupID = 14093 THEN 24196
			WHEN gdf.groupID = 40065 THEN 46781
			WHEN gdf.groupID = 14094 THEN 24197
			WHEN gdf.groupID = 14095 THEN 24198
			WHEN gdf.groupID = 14096 THEN 24199
			WHEN gdf.groupID = 53169 THEN 68143
			WHEN gdf.groupID = 58482 THEN 79134
			WHEN gdf.groupID = 14098 THEN 24201
			WHEN gdf.groupID = 103243 THEN 189111
			WHEN gdf.groupID = 14104 THEN 24207
			WHEN gdf.groupID = 14105 THEN 24208 
			WHEN gdf.groupID = 16201 THEN 24210
			WHEN gdf.groupID = 16585 THEN 24211 
			WHEN gdf.groupID = 121238 THEN 204099 
			WHEN gdf.groupID = 154060 THEN 358523 
			WHEN gdf.groupID = 34851 THEN 36631 
			WHEN gdf.groupID = 32145 THEN 34539 END, 
		datavalue
	FROM 
		#temails t 
		JOIN groupdatafields gdf WITH (NOLOCK) ON t.groupID = gdf.groupID 
		JOIN emaildatavalues edv WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID  AND edv.emailID = t.emailID
	WHERE 
		shortname = 'PAIDORFREE' 
		and isnull(datavalue,'') <> ''

print ('Insert in #gdv PAIDORFREE: ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

	INSERT INTO #gdv
	SELECT 
		edv.emailID, 
		CASE 
			WHEN inn.shortname = 'business' THEN 13855 
			WHEN inn.shortname = 'responsibility' THEN 13856 
			WHEN inn.shortname = 'Original_Effort_Code' THEN 14143 
			WHEN inn.shortname = 'Effort_Code' THEN 14144 
			WHEN inn.shortname = 'Verification_Date' THEN 14146 ELSE null END as gdfID, 
		edv.datavalue
	FROM 
		emaildatavalues edv 
		join(
			SELECT
				shortname,	
				max(emaildatavaluesID) as edvID 
			FROM	
				#temails t 
				JOIN groupdatafields gdf WITH (NOLOCK) ON t.groupID = gdf.groupID 
				JOIN emaildatavalues edv WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID  AND edv.emailID = t.emailID
			WHERE 
				shortname in ('business','responsibility','Effort_Code','Original_Effort_Code','Verification_Date') 
				and isnull(datavalue,'') <> '' 
			group by 
				t.emailID, 
				shortname) inn ON inn.edvID = edv.emaildatavaluesID
	union
	
	SELECT 
		emailID, 
		14145, 
		CONVERT(VARCHAR(100),emailID) 
	FROM 
		#temails -- subaccount numbers.

print ('Insert in #gdv UDFs: ' + CONVERT(VARCHAR(100), @@ROWCOUNT))
	
	UPDATE
		emaildatavalues
	set 
		datavalue = g.datavalue
	FROM 
		emaildatavalues edv  WITH (NOLOCK)
		JOIN #gdv g ON edv.emailID = g.emailID AND edv.groupdatafieldsID = g.gdfID

	print ('udpate EDV : ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

	INSERT INTO emaildatavalues (
		EmailID, 
		GroupDatafieldsID, 
		DataValue, 
		ModifiedDate, 
		SurveyGridID,
		EntryID)
	SELECT 
		g.emailID, 
		g.gdfID,
		g.datavalue, 
		getdate(), 
		NULL, 
		NULL 
	FROM 
		#gdv g 
		LEFT OUTER JOIN  emaildatavalues edv WITH (NOLOCK)  ON edv.emailID = g.emailID AND edv.groupdatafieldsID = g.GdfID
	WHERE 
		edv.EmaildatavaluesID IS NULL 

	print ('Insert EDV : ' + CONVERT(VARCHAR(100), @@ROWCOUNT))

--	DECLARE @Tgdv TABLE ( emailID INT, gdfID  INT, datavalue VARCHAR(255), entryID uniqueidentifier )
	CREATE TABLE #Tgdv ( emailID INT, gdfID  INT, datavalue VARCHAR(255), entryID uniqueidentifier )
	CREATE INDEX IDX_tgdv_EmailID_GdfID ON #Tgdv(EmailID, GdfID)
	
	INSERT INTO #Tgdv	
	SELECT
		t.emailID, 
		CASE WHEN shortname='startdate'		then 23532
			 WHEN shortname='enddate'		then 23533
			 WHEN shortname='amountpaid'	then 23534
			 WHEN shortname='earnedamount'	then 23535
			 WHEN shortname='Deferredamount'then 23536
			 WHEN shortname='TotalSent'		then 23537
			 WHEN shortname='PromoCode'		then 23538
			 WHEN shortname='SubType'		then 23539
			 WHEN shortname='TransactionID'	then 23540
			 WHEN shortname='PaymentMethod'	then 23541
			 WHEN shortname='CardType'		then 23542
			 WHEN shortname='CardNumber'	then 23543 	end,
		datavalue, 
		entryID
	FROM
		#temails t 
		JOIN groupdatafields gdf WITH (NOLOCK) ON t.groupID = gdf.groupID 
		JOIN emaildatavalues edv WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID  AND edv.emailID = t.emailID
	WHERE
		DatafieldsetID > 0 
		and isnull(datavalue,'') <> '' 
		and entryid is not null 
		and	gdf.shortname in ('startdate','enddate','amountpaid','earnedamount','Deferredamount','TotalSent','PromoCode','SubType','TransactionID','PaymentMethod','CardType','CardNumber')
	
	INSERT INTO #Tgdv	
	SELECT
		t.emailID, 
		CASE WHEN shortname='TransEntryID'	then 26934
			 WHEN shortname='AdjDate'		then 26935
			 WHEN shortname='AdjType'		then 26936
			 WHEN shortname='AdjAmount'		then 26937
			 WHEN shortname='AdjExpDate'	then 26938
			 WHEN shortname='AdjDesc'		then 26939	end,
		datavalue, 
		entryID
	FROM	
		#temails t 
		JOIN groupdatafields gdf WITH (NOLOCK) ON t.groupID = gdf.groupID 
		JOIN emaildatavalues edv WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID  AND edv.emailID = t.emailID
	WHERE
		DatafieldsetID > 0 
		and isnull(datavalue,'') <> '' 
		and entryid is not null 
		and	gdf.shortname in ('TransEntryID','AdjDate','AdjType','AdjAmount','AdjExpDate','AdjDesc')

	INSERT INTO #Tgdv
	SELECT DISTINCT 
		t.emailID, 
		23567, 
		g.groupname, 
		entryID
	FROM
		#temails t 
		JOIN groupdatafields gdf WITH (NOLOCK) ON t.groupID = gdf.groupID 
		JOIN groups g WITH (NOLOCK) ON g.groupID = gdf.groupID 
		JOIN emaildatavalues edv WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID  AND edv.emailID = t.emailID
	WHERE
		DatafieldsetID > 0 
		and isnull(datavalue,'') <> '' 
		and entryid is not null

	INSERT INTO emaildatavalues (
		EmailID, 
		GroupDatafieldsID, 
		DataValue, 
		ModifiedDate, 
		SurveyGridID,
		EntryID)
	SELECT	
		g.emailID, 
		g.gdfID, 
		g.datavalue, 
		getdate(), 
		NULL, 
		g.EntryID 
	FROM
		#Tgdv g 
		LEFT OUTER JOIN emaildatavalues edv WITH (NOLOCK)  ON edv.emailID = g.emailID AND edv.groupdatafieldsID = g.GdfID AND edv.entryID = g.entryID
	WHERE
		edv.EmaildatavaluesID IS NULL 

	print ('Insert EDV for Paid: ' + CONVERT(VARCHAR(100), @@ROWCOUNT))
End
