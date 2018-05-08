﻿
CREATE proc [dbo].[getEnewsDataForDQM]
(
	@clientName Varchar(50),
	@IncludeTransUDFs BIT = 0
)
as
Begin
	declare 
		@GroupID VARCHAR(4000),
		@ClientID int 
		
		 
	--declare
	--	@GroupID VARCHAR(4000),    
	--	@FromDate varchar(10),
	--	@ToDate varchar(10)

	--BEGIN
	--	set @GroupID = 49449
	--	set @FromDate = ''
	--	set @ToDate = ''


/** Query to get clientID

SELECT BasechannelID
  FROM [ECN5_ACCOUNTS].[dbo].[Customer] 
  where CustomerID in
  (
  select CustomerID  
    FROM [ECN5_COMMUNICATOR].[dbo].[Groups]
    where GroupID in 
    (
    --'178643','178639','178646','178642','178641','178640','178645','178644'
    '28791','158255','28641','28791','158255','28641','165825','167364'
    )
  )  
**/  		 
	set NOCOUNT ON  
	set @clientName = upper(@clientName)
	if @clientName = 'AB' 
	Begin
		set @ClientID = 15
		set @GroupID = '28641,167364,220992,28682,221013,28808,221008,247484,248894';--167295,28791,167295,158255,165825,189632
	End	
	else if @clientName = 'WATT'
	Begin
		set @ClientID = 34
		--new list as of 03/24/14
		set @GroupID = '32532,32533,33989,33991,33993,33995,33996,34367,35421,35473,35474,40489,41451,45851,48976,40936,147473,151354,175802,178200,192199,227064,268950';
		--old list as of 03/23/14
		--set @GroupID = '32532,32533,33989,33990,33991,33993,33994,33995,33996,34367,35421,35473,35474,40489,41451,45851,46592,48976,51949,51951,56166,125666,138320,147473,192199,178200,151354,175802'
	End	
	else if @clientName = 'CANON'
	Begin
		set @ClientID = 42
		set @GroupID = '14093,14105,15282,17865,17866,17867,17868,17870,17871,17876,17877,17879,18049,18050,18095,18096,18097,18098,18100,18144,18159,18204,18261,18361,18362,18364,18410,18474,18507,18510,18511,18512,18559,18587,18731,19972,20959,20960,20961,21143,21623,21624,21724,21726,21809,22547,22548,22549,22906,23228,23229,24185,31047,31695,32332,34667,34669,34670,34671,34672,34673,34674,34675,35108,35735,35736,35736,35737,35738,35739,35740,36013,36447,36448,36739,36950,38248,38943,39024,46324,46981,46982,56350,56613,56614,56615,56616,71230,137867,160782,160863,160864,160865,160867,160869,160870,160871,160872,160873,162550,162617,164052,164174,181954,9814,37508,37509,51667,73112,84451,152233,152497,169008,169009,169010,169011,181955,181957,181958,190738'
			End	
	else if @clientName = 'MEDTECH'
	Begin
		set @ClientID = 55
		set @GroupID = '48309,48310,48608,48914,49449,49644,49676,49792,49804,50492,50493,50511,51529,53136,53138,53140,53853,53854,53855,53856,53857,55457,55458,57084,57454,57855,57856,90115,97640,100100,105007,112880,116654,117054,119869,121757,121758,122549,130882,136276,138406,147291,147292';--,173458,175972,175970,174414
	End	
	else if @clientName = 'UPI'
	Begin
		set @ClientID = 63
	   set @GroupID = '87610,87611,87612,87613,95030,97015,97016,97017,105106,105108,105109,123533,123534,123538,125654,126742,128989,128990,128991,128992,129003,129633,129640,129645,129693,129711,129833,130913,133730,133801,141370,148647,148649,148650,150837,162551,189468,215595,215596,215599'

	End	
	else if @clientName = 'ADVANSTAR'
	Begin
		set @ClientID = 65
		set @GroupID = '115518,115982,118323,118337,118339,118361,118362,118363,118438,118439,118482,118492,118565,118570,118571,118572,118573,118577,118579,118580,118581,118589,131009,131010,131011,150469,153318,153319,153320,153321,157550,234329,179797,234330'
	End	
	else if @clientName = 'NASFT'
	Begin
		set @ClientID = 71
		set @GroupID = '142899,144128'
	End	
	else if @clientName = 'SCRANTON'
	Begin
		set @ClientID = 75
		set @GroupID = '164451,164452,164453,164456,167097,167098,167099,167370,167371,167372,167397,167433,167434,167434,167434,167435,167436,167437,167562,167855,167856,167861,167862,167863,167864,167910,167912,168557,168558,169012,169013,169215,169216,169217,169219,169221,169222,169223,169228,169231,169232,169233,169234,169236,169238,169239,169242,169244,169245,169248,169251,169252,169253,169256,169257,169259,169260,169261,169262,169266,169268,169271,169272,169273,169274,169275,169276,169278,169279,169280,169281,169284,169295,169297,169299,169300,169301,169315,169316,169317,169319,169320,169321,169322,169323,169325,169327,169449,169721,171602,171605,171610,171611,171614,171615,171617,171618,171619,171620,171621,171622,171623,171624,171626,171632,171661,171663,171755,171758,171764,171767,171768,171772,171774,171780,171784,163969,166820,167093,167368,167431,167561,167587,167624,167754,167892,167902,168555,169472,169312,180454,180453,182013,177919,190669,170471,170472,170473,205846,170475,170476,170477,170478,211021,170479,223327,177230,170480,170481,213961,170482,170485,170486,170288,260464,260468,260719,260721,260475,260483,260476,260485,260501,260502,260479,260486,260716,260720,253368,275532,263752,261048,253363,275534,263756,261051'
		End	
	else if @clientName = 'VCAST'
	Begin
		set @ClientID = 77
		set @GroupID = '178639,178640,178641,178642,178643,178644,178645,178646'
	End	
	else if @clientName = '10MISSIONS'
	Begin
		set @ClientID = 39
		set @GroupID = '39753,167963,213239,213240,159633,48429,254828,81515,130235,229892,247346,180428,183366,181983,93854,156597,199116,261653,180197,211288,211845,233660,270610'
	End	
	else if @clientName = 'STAMATS'
	Begin
		set @ClientID = 73
		set @GroupID = '157769,157770,157771,157772,157773,157774,157775,157776,157797,157798,157799,157800,157801,157802,157814,157815,157816,157828,157831,157832,157834,157835,164655,164657,164661,164662,164664,164666,164668,164670,164674,164675,169460'
	End	
	else if @clientName = 'MEISTER'
	Begin
		set @ClientID = 82
		set @GroupID = '184451,184552,185483,185868,185386,184563,185657,184550,185434,185621,184554,183933,181966,181967,181965,189125,193397'--248617
	End
		else if @clientName = 'FRANCE'
	Begin
		set @ClientID = 80
		set @GroupID = '186789,186785,186786,186792,186795,186790,186793,186791,186782,186784,186783,186788,186787'
	End			
		else if @clientName = 'ATCOM'
	Begin
		set @ClientID = 72
		set @GroupID = '157049,167182,167183,167184,167721,167186,167185,167187,167180,167188'
	End	
		else if @clientName = 'TRADEPRESS' 
	Begin
		set @ClientID = 89
		set @GroupID = '210025,210026,210068,210051,210069,210024,210028,210029,210072,210052,210071,210070,210030,210047,210048,210046,210031,210053,210073,210032,210049,210035,210054,210036,210061,210049,210027,210033,210034,210074,210075,210076,210077,210078,210079,210080,210081,210082,210083,210084,210085,210086,210087,210089,210090,210091,210092,210093,210094,210095,210097,210098,210100,210101,210096,210099,213660,209556,209557,206011,252268,303783'
	End	
		else if @clientName = 'HEALTHFORUM' 
	Begin
		set @ClientID = 131
		set @GroupID = '226020,226019,226021,226017,226018,233689,234619'
	End	
		else if @clientName = 'NORTHSTAR' 
	Begin
		set @ClientID = 112
		set @GroupID = '250443,267170,264577,264333,264335,268201,268200,268199,268415,267171,267173,268413,268416,268414,264568,264570,264571,264572,264574,264565,267172,265032,264567,264568,264570,267565,264571,264572,265031,264573,265036,265034,264574,264575,265033,265035,264576'
	End	
 		else if @clientName = 'TMB' 
	Begin
		set @ClientID = 134
		set @GroupID = '253581, 253583, 253582'
	End	
        else if @clientName = 'HVCB' 
    Begin
        set @ClientID = 132
        set @GroupID = '248920,248921,248919,248922,248918,248316,248062,307695'
    End   
        else if @clientName = 'ANTHEM' 
    Begin
        set @ClientID = 84
        set @GroupID = '192460'
    End   
        else if @clientName = 'BABCOX' 
    Begin
        set @ClientID = 137
        set @GroupID = '267386,267385,267384,267383,267316,267376,267378,267379,267380,267381,265524,267317,267318,267375,267319,267320,267382,267322,267377,267323,267374,267676,267315,267372,267373,276245,276253'
    End   		
        else if @clientName = 'BRIEFMEDIA' 
    Begin
        set @ClientID = 86
		set @GroupID = '279714,278892,278988,279715,279716,278991,278992,278993,278995,278996,278997,278998,278999,279002,279003,279004,279006,279007,279008,279009,279010,279011,279012,279013,279014,279015,279018,279019,281622,279021,279022,279023,279024,279025,279026,279027,279028,279029,279030,279031,279032,279033,279034,279035,279037,279040,279043,279044,279045,279046,279047,279048,279049,279050,279051,279052,279053,279054,279055,279056,279057,279058,279059,279060,279061,279062,279063,279064,279065,279066,279646,279648,279649,279650,279651,279652,279653,279654,279655,279656,279658,279660,279661,279662,279665,279669,279675,279676,279677,278894,279678,279679,279680,279681,279682,279683,279684,279685,279686,279687,279688,279689,291285,279690,290768,279691,279692,279693,279694,279695,291284,279696,279697,279698,279699,279700,279701,279702,279703,279704,279706,279707,280373,283388,285068,284604,281635,312912,312911'
	End
        else if @clientName = 'KM' 
    Begin
        set @ClientID = 93
		set @GroupID = '222659,223549,225623,227452,228212,228579,228582'
	End
        else if @clientName = 'HWPUBLISHING' 
    Begin
        set @ClientID = 142
		set @GroupID = '288901,288902'
	End
        else if @clientName = 'CATERSOURCE'
    Begin
        set @ClientID = 106
		set @GroupID = '305803,306266,306271,306273,307618,307620,307630'
	End
        else if @clientName = 'KM' 
    Begin
        set @ClientID = 93
		set @GroupID = '222659,223549,225623,227452,228212,228579,228582'
	End
        else if @clientName = 'HWPUBLISHING' 
    Begin
        set @ClientID = 142
		set @GroupID = '288901,288902'
	End
        else if @clientName = 'CATERSOURCE'
    Begin
        set @ClientID = 106
		set @GroupID = '305803,306266,306271,306273,307618,307620,307630'
	End

			DECLARE @StandAloneUDFs VARCHAR(MAX)
			DECLARE @TransactionalUDFs VARCHAR(MAX)
			declare @sColumns varchar(MAX),
					@tColumns varchar(MAX),
					@standAloneQuery varchar(MAX),
					@TransactionalQuery varchar(MAX)
					
			set @StandAloneUDFs = ''
			set @TransactionalUDFs = ''
			set @sColumns = ''
			set @tColumns = ''
			set @standAloneQuery = ''
			set @TransactionalQuery = ''
			
			
			SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE  ShortName in 
			(select fieldname from ecn_temp..EnewsExportFields) and 
			ShortName not in (select ShortName from GroupDatafields where GroupID in (select items from dbo.fn_Split(@GroupID, ',')) and 
			DatafieldSetID > 0 ) and GroupID in (select items from dbo.fn_Split(@GroupID, ',')) AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
				
			IF @IncludeTransUDFs = 1
			BEGIN				
			SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE  ShortName in 
			(select fieldname from ecn_temp..EnewsExportFields) and GroupID in (select items from dbo.fn_Split(@GroupID, ',')) AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
			END
			
					
			if LEN(@standaloneUDFs) > 0
			Begin
				set @sColumns = ', SAUDFs.* '
				set @standAloneQuery= ' left outer join			
					(
						SELECT *
						 FROM
						 ( 
							SELECT gdf.groupID as SUDF_GID, edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
							from	EmailDataValues edv with (NOLOCK)  join  
									Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							where 
									gdf.GroupID in (' + @GroupID + ')  and datafieldsetID is null
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
					) 
					SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID  and emailgroups.groupID = SAUDFs.SUDF_GID '
			End
-- Task 12634 commented out, then subsequently re-added
			if LEN(@TransactionalUDFs) > 0
			Begin

				set @tColumns = ', TUDFs.* '
				set @TransactionalQuery= '  left outer join
				(
					SELECT *
					 FROM
					 (
						SELECT gdf.groupID As TUDF_GID, edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
						from	EmailDataValues edv with (NOLOCK)  join  
								Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID in (' + @GroupID + ')  and datafieldsetID > 0
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
				) 
				TUDFs on Emails.emailID = TUDFs.tmp_EmailID1  and emailgroups.groupID = TUDFs.TUDF_GID '
			End

			BEGIN TRY
					exec ('drop table ECN_Temp..MAF_' + @ClientName + '_Enews');
			END TRY
			BEGIN CATCH
			END CATCH; 
		
		
--		print (' select ' + convert(varchar(10),@ClientID) + ' as Client_ID, ''' + @clientName + ''' as ClientName, Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
--				' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
--				' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
--				' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode, Emailgroups.GroupId ' + @sColumns + @tColumns +  
--				' into ECN_Temp..MAF_' + @ClientName + '_Enews ' +
--				' from Emails with (NOLOCK) join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
--				' where EmailGroups.GroupID IN (' + @GroupID + ')')

----				'  EmailGroups.SubscribeTypeCode = ''S'''+  				
				


		EXEC (' select ' + @ClientID + ' as Client_ID, ''' + @clientName + ''' as ClientName, Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
				' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
				' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
				' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode, Emailgroups.GroupId ' + @sColumns + @tColumns +  
				' into ECN_Temp..MAF_' + @ClientName + '_Enews ' +
				' from Emails with (NOLOCK) join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
				' where EmailGroups.GroupID IN (' + @GroupID + ')')
				--				' EmailGroups.SubscribeTypeCode = ''S'''+  
												

									
												
		--exec ('select *  from ECN_Temp..MAF_' + @ClientName + '_Enews')
End


GO
GRANT ALTER
    ON OBJECT::[dbo].[getEnewsDataForDQM] TO [webuser]
    AS [dbo];


GO
GRANT CONTROL
    ON OBJECT::[dbo].[getEnewsDataForDQM] TO [webuser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[getEnewsDataForDQM] TO [webuser]
    AS [dbo];

