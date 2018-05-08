------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
--
--
--
------------------------------------------------------


CREATE  proc [dbo].[sp_Pharma_eNewsletters_Group_Update]
as

Begin
	--removed -- on 2/28/2013 -- select groupID, groupname from groups where groupID in (	32145,58482,53169,14094,103243,34851)
	set nocount on

	declare @currentdate datetime
	set @currentdate = getdate()

	/* fix for data from old subscription form*/
	insert into emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
	select distinct eg.emailID, groupdatafieldsID, 'FREE', getdate(), -1, null
	from emailgroups eg join (select groupID, groupdatafieldsID from groupdatafields where shortname = 'paidorfree' and groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060)) gdf on eg.groupID = gdf.groupID 
	where eg.groupID in (
	14092,14093,40065,14095,14096,14098,14099,14100,14101,14102,14104,14105,16201,16585,121238,154060) and 
		not exists (select emaildatavaluesID from emaildatavalues edv where edv.emailID = eg.emailID and edv.groupdatafieldsID = gdf.groupdatafieldsID) --and eg.subscribetypecode = 'S'

	insert into emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
	select distinct eg.emailID, groupdatafieldsID, eg.emailID, getdate(), -1, null
	from emailgroups eg join (select groupID, groupdatafieldsID from groupdatafields where shortname = 'Sub_Account_Number' and groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060)) gdf on eg.groupID = gdf.groupID 
	where eg.groupID in (14092,14093,40065,14095,14096,14098,14099,14100,14101,14102,14104,14105,16201,16585,121238,154060) and 
		not exists (select emaildatavaluesID from emaildatavalues edv where edv.emailID = eg.emailID and edv.groupdatafieldsID = gdf.groupdatafieldsID) and eg.subscribetypecode = 'S'

	insert into emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
	select distinct eg.emailID, groupdatafieldsID, convert(varchar(10),(case when lastchanged is null then createdon else lastchanged end),101), convert(varchar(10),(case when lastchanged is null then createdon else lastchanged end),101), -1, null
	from emailgroups eg join (select groupID, groupdatafieldsID from groupdatafields where shortname = 'Verification_Date' and groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060)) gdf on eg.groupID = gdf.groupID 
	where eg.groupID in (14092,14093,40065,14095,14096,14098,14099,14100,14101,14102,14104,14105,16201,16585,121238,154060) and 
		not exists (select emaildatavaluesID from emaildatavalues edv where edv.emailID = eg.emailID and edv.groupdatafieldsID = gdf.groupdatafieldsID) and eg.subscribetypecode = 'S'
	/* endfix for data from old subscription form*/

	declare @temails TABLE (emailID int, groupID int, SubscribeTypeCode varchar(50), createdon datetime)
	insert into @temails
	select	distinct eg.emailID, eg.groupID, eg.SubscribeTypeCode, eg.createdon from emailgroups eg join 
			groupdatafields gdf on eg.groupID = gdf.groupID join
			emaildatavalues edv on eg.emailID = edv.emailID and gdf.groupdatafieldsID = edv.groupdatafieldsID
	where	eg.groupID in (14092,14093,40065,14095,14096,14098,14104,14105,16201,16585,121238,154060) 
			and
			(CONVERT(VARCHAR(10),CreatedOn,101) =  CONVERT(VARCHAR(10),GETDATE()-1,101) or
			CONVERT(VARCHAR(10),LastChanged,101) =  CONVERT(VARCHAR(10),GETDATE()-1,101) or 
			CONVERT(VARCHAR(10),edv.ModifiedDate,101) =  CONVERT(VARCHAR(10),GETDATE()-1,101))
			
/*
	select	distinct eg.emailID, eg.groupID, eg.SubscribeTypeCode, eg.createdon from emailgroups eg join 
			groupdatafields gdf on eg.groupID = gdf.groupID join
			emaildatavalues edv on eg.emailID = edv.emailID and gdf.groupdatafieldsID = edv.groupdatafieldsID
	where	eg.groupID in (14092,14093,40065,14095,14096,14098,14099,14100,14101,14102,14104,14105,16201,16585,121238,154060) 
			and
			gdf.shortname = 'paidorfree' and edv.datavalue = 'free'
*/


	print '1. completed'
	/***************************************************************************************************
	CREATE SUBACCOUNT NUMBERS IN EACH NEWSLETTER GROUPS
	***************************************************************************************************/
	declare @subaccount TABLE (emailID int, groupdatafieldsID int)

	-- get emailID and groupdatafieldsID for sub account number
	insert into @subaccount 
	select t.emailID, gdf.groupdatafieldsID  from @temails t join groupdatafields gdf on gdf.groupID = t.groupID
	where	shortname = 'Sub_Account_Number' 

	-- delete existing subaccount numbers
	delete from emaildatavalues 
	where emaildatavaluesID in (select emaildatavaluesID from emaildatavalues edv join @subaccount s on edv.groupdatafieldsID= s.groupdatafieldsID and edv.emailID = s.emailID where convert(varchar(100),edv.emailID) <> Datavalue)

	-- insert subaccount number if not exists
	insert into emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
	select s.emailID, s.groupdatafieldsID, s.emailID,@currentdate,  -1, null from @subaccount s
	left outer join 
	(	
		select EmailDataValuesID, edv.emailID, edv.groupdatafieldsID from emaildatavalues edv join  @subaccount s on edv.groupdatafieldsID= s.groupdatafieldsID and edv.emailID = s.emailID
	) inn
	on s.emailID = inn.emailID and s.groupdatafieldsID = inn.groupdatafieldsID
	where inn.emaildatavaluesID is null

	/***************************************************END SUB ACCOUNT NUMBER*************************************************/


	/***************************************************************************************************
	-- COPY CURRENT PROMOCODE UDF TO ORIGINAL PROMOCODE UDF.
	***************************************************************************************************/
	
	declare @PromoCode TABLE (
		emailID int, 
		EffortCodegdfID int, 
		EffortCodeedvID BIGINT ,--int, 
		EffortCodedv varchar(255), 
		OrgEffortCodegdfID int, 
		OrgEffortCodeedvID BIGINT ,--int, 
		OrgEffortCodedv varchar(255))

	insert into @PromoCode (emailID, EffortCodegdfID, OrgEffortCodegdfID)
	select t.emailID, gdf.groupdatafieldsID, (select gdf1.groupdatafieldsID from groupdatafields gdf1 where shortname = 'Original_Effort_Code' and groupID = gdf.groupID)
	from @temails t join groupdatafields gdf on t.groupID = gdf.groupID
	where shortname = 'Effort_Code'

	update @promocode
	set EffortCodedv = datavalue,
		EffortCodeedvID = emaildatavaluesID	
	from @promocode p join emaildatavalues edv on p.emailID = edv.emailID and p.EffortCodegdfID = edv.groupdatafieldsID

	delete from @promocode where isnull(EffortCodedv,'') = ''

	update @promocode
	set OrgEffortCodedv = datavalue,
		OrgEffortCodeedvID = emaildatavaluesID
	from @promocode p join emaildatavalues edv on p.emailID = edv.emailID and p.OrgEffortCodegdfID = edv.groupdatafieldsID

	delete from @promocode where EffortCodedv = OrgEffortCodedv

	update emaildatavalues
	set datavalue = EffortCodedv
	from emaildatavalues edv join @promocode p on edv.emaildatavaluesID = p.OrgEffortCodeedvID

	insert into emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
	select emailID, OrgEffortCodegdfID, EffortCodedv, @currentdate, -1, null from @promocode 
	where OrgEffortCodeedvID is null 

	/*********************END PROMOCODE UPDATE*********************/

	/***************************************************************************************************
	-- COPY CREATE DATE TO SUBDATE
	***************************************************************************************************/
	
--	declare @SubDate TABLE (emailID int, gdfID int, createdon datetime, edvID BIGINT) --int)
--
--	insert into @SubDate
--	select  t.emailID, gdf.groupdatafieldsID, t.createdon, edv.emaildatavaluesID
--	from	@temails t JOIN 
--			groupdatafields gdf on gdf.groupID = t.groupID left outer JOIN
--			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID and edv.emailID = t.emailID
--	where	gdf.shortname ='SubDate'
--
--	update emaildatavalues
--	set datavalue = createdon
--	from emaildatavalues edv join @SubDate s on edv.emaildatavaluesID = s.edvID
--
--	insert into emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)
--	select emailID, gdfID, convert(varchar,convert(datetime,createdon),101) + ' ' + convert(varchar,convert(datetime,createdon),108) , @currentdate, -1, null from @SubDate where edvID is null 
	 
	/********************* END COPY CREATE DATE TO SUBDATE *********************/

	declare @Pharma_eNewsletters_Group_14038 TABLE (EmailID	int, GroupID int, scode varchar(10), GdfID int)

	insert into @Pharma_eNewsletters_Group_14038
	select EmailID, groupID, SubscribeTypeCode,
		case	when groupID = 14092 then 13069
				when groupID = 14093 then 13070
				when groupID = 40065 then 46779
				when groupID = 14094 then 13071
				when groupID = 14095 then 13072
				when groupID = 14096 then 13073
				when groupID = 53169 then 68141
				when groupID = 58482 then 79132
				when groupID = 14098 then 13075
				when groupID = 103243 then 189109
				when groupID = 14104 then 13085
				when groupID = 14105 then 13086 
				when groupID = 16201 then 17057 
				when groupID = 16585 then 17709 
				when groupID = 121238 then 204097
				when groupID = 154060 then 358521
				when groupID = 34851 then 36629 
				when groupID = 32145 then 34537
		end
	from @temails

	/* INSERT INTO EMAIL GROUPS */
	insert into EmailGroups (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)
	select distinct p.EmailID, 14038, 'html', 'S', getdate(), NULL 
	from	@Pharma_eNewsletters_Group_14038 p left outer join
			emailgroups eg on eg.emailID = p.emailID and eg.groupID = 14038
	where	eg.emailgroupID is null

	print ('Insert Emailgroups : ' + Convert(varchar(100), @@ROWCOUNT))

	-- Newsletter group to master group -- update
	update emaildatavalues
	set datavalue = case when scode = 'S' then 'X' else '' end, ModifiedDate = getdate()
	from emaildatavalues edv join @Pharma_eNewsletters_Group_14038 p on edv.emailID = p.emailID and edv.groupdatafieldsID = p.GdfID

	print ('update EDV : ' + Convert(varchar(100), @@ROWCOUNT))

	-- Newsletter group to master group -- insert

	insert into emaildatavalues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, SurveyGridID,EntryID)
	select p.emailID, p.gdfID, 'X', getdate(), NULL, NULL 
	from @Pharma_eNewsletters_Group_14038 p left outer join  emaildatavalues edv  on edv.emailID = p.emailID and edv.groupdatafieldsID = p.GdfID
	where edv.EmaildatavaluesID is null and scode = 'S'

	print ('Insert EDV : ' + Convert(varchar(100), @@ROWCOUNT))

	declare @gdv TABLE ( emailID int, gdfID int, datavalue varchar(255) )
	-- SUBDATE
	insert into @gdv 
	select	t.emailID, 
			case when t.groupID = 14092 then 13883
				when t.groupID = 14093 then 13868
				when t.groupID = 40065 then 46782
				when t.groupID = 14094 then 13869
				when t.groupID = 14095 then 13870
				when t.groupID = 14096 then 13871
				when t.groupID = 53169 then 68144
				when t.groupID = 58482 then 79135
				when t.groupID = 14098 then 13873
				when t.groupID = 103243 then 189112
				when t.groupID = 14104 then 13879
				when t.groupID = 14105 then 13880 
				when t.groupID = 16201 then 17058 
				when t.groupID = 16585 then 17710 
				when t.groupID = 121238 then 204100 
				when t.groupID = 154060 then 358524 
				when t.groupID = 34851 then 36632 
				when t.groupID = 32145 then 34540
			end, 
			convert(varchar(10), convert(datetime,datavalue), 101) 
	from 
			@temails t join 
			groupdatafields  gdf on t.groupID = gdf.groupID join	
			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID  and edv.emailID = t.emailID
	where 
			shortname = 'Verification_Date' and isnull(datavalue,'') <> ''
	   
	print ('Insert in @gdv subscribe date: ' + Convert(varchar(100), @@ROWCOUNT))

	-- NOofTrials
	insert into @gdv 
	select	t.emailID, 
			case when t.groupID = 14092 then 26921
				when t.groupID = 14093 then 26908
				when t.groupID = 40065 then 46780
				when t.groupID = 14094 then 26909
				when t.groupID = 14095 then 26910
				when t.groupID = 14096 then 26911
				when t.groupID = 53169 then 68142
				when t.groupID = 58482 then 79133
				when t.groupID = 14098 then 26913
				when t.groupID = 103243 then 189110
				when t.groupID = 14104 then 26919
				when t.groupID = 14105 then 26920
				when t.groupID = 16201 then 26922
				when t.groupID = 16585 then 26923
				when t.groupID = 121238 then 204098
				when t.groupID = 154060 then 358522
				when t.groupID = 34851 then 36630
				when t.groupID = 32145 then 34538
			end, 
			datavalue
	from 
			@temails t join 
			groupdatafields  gdf on t.groupID = gdf.groupID join	
			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID  and edv.emailID = t.emailID
	where 
			shortname = 'Nooftrials' and isnull(datavalue,'') <> ''
	   
	print ('Insert in @gdv No of Trials: ' + Convert(varchar(100), @@ROWCOUNT))

	-- PAIDORFREE
	insert into @gdv 
	select	t.emailID, 
			case when gdf.groupID = 14092 then 24209
				when gdf.groupID = 14093 then 24196
				when gdf.groupID = 40065 then 46781
				when gdf.groupID = 14094 then 24197
				when gdf.groupID = 14095 then 24198
				when gdf.groupID = 14096 then 24199
				when gdf.groupID = 53169 then 68143
				when gdf.groupID = 58482 then 79134
				when gdf.groupID = 14098 then 24201
				when gdf.groupID = 103243 then 189111
				when gdf.groupID = 14104 then 24207
				when gdf.groupID = 14105 then 24208 
				when gdf.groupID = 16201 then 24210
				when gdf.groupID = 16585 then 24211 
				when gdf.groupID = 121238 then 204099 
				when gdf.groupID = 154060 then 358523 
				when gdf.groupID = 34851 then 36631 
				when gdf.groupID = 32145 then 34539
			end, 
			datavalue
	from 
			@temails t join 
			groupdatafields  gdf on t.groupID = gdf.groupID join	
			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID  and edv.emailID = t.emailID
	where 
			shortname = 'PAIDORFREE' and isnull(datavalue,'') <> ''

print ('Insert in @gdv PAIDORFREE: ' + Convert(varchar(100), @@ROWCOUNT))

	insert into @gdv
	select edv.emailID, 
		case 
			when inn.shortname = 'business' then 13855 
			when inn.shortname = 'responsibility' then 13856 
			when inn.shortname = 'Original_Effort_Code' then 14143 
			when inn.shortname = 'Effort_Code' then 14144 
			when inn.shortname = 'Verification_Date' then 14146 else null
		end 
		as gdfID, edv.datavalue
	from emaildatavalues edv join
		(
		select	shortname,	max(emaildatavaluesID) as edvID 
		from	@temails t join 
				groupdatafields  gdf on t.groupID = gdf.groupID join	
				emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID  and edv.emailID = t.emailID
		where 
				shortname in ('business','responsibility','Effort_Code','Original_Effort_Code','Verification_Date') 
		and isnull(datavalue,'') <> '' 
		group by t.emailID, shortname
		) inn on inn.edvID = edv.emaildatavaluesID
	union
	select emailID, 14145, Convert(varchar(100),emailID) from @temails -- subaccount numbers.

print ('Insert in @gdv UDFs: ' + Convert(varchar(100), @@ROWCOUNT))
	
	update emaildatavalues
	set datavalue = g.datavalue
	from emaildatavalues edv join @gdv g on edv.emailID = g.emailID and edv.groupdatafieldsID = g.gdfID

	print ('udpate EDV : ' + Convert(varchar(100), @@ROWCOUNT))

	insert into emaildatavalues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, SurveyGridID,EntryID)
	select g.emailID, g.gdfID, g.datavalue, getdate(), NULL, NULL 
	from @gdv g left outer join  emaildatavalues edv  on edv.emailID = g.emailID and edv.groupdatafieldsID = g.GdfID
	where edv.EmaildatavaluesID is null 

	print ('Insert EDV : ' + Convert(varchar(100), @@ROWCOUNT))

	declare @Tgdv TABLE ( emailID int, gdfID int, datavalue varchar(255), entryID uniqueidentifier )
	
	insert into @Tgdv	
	select	t.emailID, 
			case when shortname='startdate'		then 23532
				 when shortname='enddate'		then 23533
				 when shortname='amountpaid'	then 23534
				 when shortname='earnedamount'	then 23535
				 when shortname='Deferredamount'then 23536
				 when shortname='TotalSent'		then 23537
				 when shortname='PromoCode'		then 23538
				 when shortname='SubType'		then 23539
				 when shortname='TransactionID'	then 23540
				 when shortname='PaymentMethod'	then 23541
				 when shortname='CardType'		then 23542
				 when shortname='CardNumber'	then 23543 
			end,
			datavalue, 
			entryID
	from	@temails t join 
			groupdatafields  gdf on t.groupID = gdf.groupID join	
			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID  and edv.emailID = t.emailID
	where	DatafieldsetID > 0 and isnull(datavalue,'') <> '' and entryid is not null and
			gdf.shortname in ('startdate','enddate','amountpaid','earnedamount','Deferredamount','TotalSent','PromoCode','SubType','TransactionID','PaymentMethod','CardType','CardNumber')

	
	insert into @Tgdv	
	select	t.emailID, 
			case when shortname='TransEntryID'	then 26934
				 when shortname='AdjDate'		then 26935
				 when shortname='AdjType'		then 26936
				 when shortname='AdjAmount'		then 26937
				 when shortname='AdjExpDate'	then 26938
				 when shortname='AdjDesc'		then 26939
			end,
			datavalue, 
			entryID
	from	@temails t join 
			groupdatafields  gdf on t.groupID = gdf.groupID join	
			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID  and edv.emailID = t.emailID
	where	DatafieldsetID > 0 and isnull(datavalue,'') <> '' and entryid is not null and
			gdf.shortname in ('TransEntryID','AdjDate','AdjType','AdjAmount','AdjExpDate','AdjDesc')

	insert into @Tgdv
	select distinct t.emailID, 23567, g.groupname, entryID
	from	@temails t join 
			groupdatafields  gdf on t.groupID = gdf.groupID join	
			groups g on g.groupID = gdf.groupID join
			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID  and edv.emailID = t.emailID
	where	DatafieldsetID > 0 and isnull(datavalue,'') <> '' and entryid is not null

	insert into emaildatavalues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, SurveyGridID,EntryID)
	select	g.emailID, g.gdfID, g.datavalue, getdate(), NULL, g.EntryID 
	from	@Tgdv g left outer join  
			emaildatavalues edv  on edv.emailID = g.emailID and edv.groupdatafieldsID = g.GdfID and edv.entryID = g.entryID
	where edv.EmaildatavaluesID is null 

	print ('Insert EDV for Paid: ' + Convert(varchar(100), @@ROWCOUNT))
End