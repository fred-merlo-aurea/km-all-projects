------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
-- 2015-01-15 MK Added logging For ImportEmailTracking and ImportEmailsLog


------------------------------------------------------

CREATE PROC [dbo].[e_EmailGroup_ImportEmailsWithDupes]
(
 @CustomerID int,  
 @GroupID int,  
 @xmlProfile TEXT,  
 @xmlUDF TEXT,
 @formattypecode varchar(50),
 @subscribetypecode varchar(50),
 @EmailAddressOnly bit,
 @compositekey varchar(50) = null,
 @overwritewithNULL bit = 0,
 @UserID int,
 @Source varchar(100) = 'Unknown'
)
as

Begin  

-- declare @CustomerID int = 1
-- declare @GroupID int = 292956
-- declare @xmlProfile varchar(MAX) = '<XML>
-- <Emails><emailaddress>sw212@somedomain.com</emailaddress><title>title</title><firstname>First</firstname><lastname>name</lastname><fullname>Fullname</fullname><company>John Doe</company><occupation>QA</occupation><address>489 Hwy BB</address><address2></address2><city>Dixon</city><state>MO</state><zip>65459-9102</zip><country>usa</country><voice></voice><mobile>5465467676575</mobile><fax>435435636</fax><website>54354656747</website><age>25</age><income>234324</income><gender>m</gender><user1>1</user1><user2>2</user2><user3>3</user3><user4>4</user4><user5>5</user5><user6>6</user6><birthdate>02/02/1989</birthdate><userevent1>e1</userevent1><userevent1date>04/04/2016</userevent1date><userevent2>e2</userevent2><userevent2date></userevent2date><notes>43543565</notes><password>password</password></Emails></XML>'

-- Declare @formattypecode varchar(50) = 'html'
-- Declare @subscribetypecode varchar(50) = 'S'
-- Declare @EmailAddressOnly bit = 0
-- Declare @compositekey varchar(50) = 'user1'
-- Declare @overwritewithNULL bit = 1
--declare @xmlUDF varchar(MAX) = '<XML></XML>'
-- set @xmlUDF = '<XML>
--<row>
--	<ea kv="11111">sunil@knowledgemarketing.com</ea>
--	<udf id="63736"><v>UDF value 1 - 11111</v></udf>
--	<udf id="63737"><v>UDF value 2 - 11111</v></udf>
--</row>
--<row>
--	<ea  kv="22222">sunil@knowledgemarketing.com</ea>
--	<udf id="63736"><v>UDF value 1 - 22222</v></udf>
--	<udf id="63737"><v>UDF value 2 - 22222</v></udf>
--</row>
--<row>
--	<ea  kv="33333">sunil4@knowledgemarketing.com</ea>
--	<udf id="63736"><v>UDF value 1 - 33333</v></udf>
--	<udf id="63737"><v>UDF value 2 - 33333</v></udf>
--</row>
--<row>
--	<ea>sunil5@knowledgemarketing.com</ea>
--	<udf id="63736"><v>UDF value 1 - 33333</v></udf>
--	<udf id="63737"><v>UDF value 2 - 33333</v></udf>
--</row>
--</XML>'


  
	set nocount on  
	
	declare @docHandle int,
			@dt datetime,
			@primaryKeyID int,
			@tUDF_Exists bit,
			@TrackingId int,
			@FileName Varchar(200)
	
	set @primaryKeyID = 0
	set @tUDF_Exists = 0
	set @dt = getdate()
 
  	SET @TrackingID = 0
	SET @FileName = NEWID()

	INSERT INTO ImportEmailTracking (
		CustomerID, 
		GroupID, 
		StartTime) 
	VALUES (
		@CustomerID, 
		@GroupID, 
		@dt)

	SELECT @TrackingID = @@IDENTITY
	
	declare @result TABLE  (Action varchar(100), Counts int)  

	declare @entryID TABLE (RowID int, DatafieldSetID int, entryID uniqueidentifier)  
  
  	print ('Step 1 : Create Temp Emails and Index = '  + convert(varchar,getdate(),109))

	create TABLE #emails   
	(  
		tmpEmailID int IDENTITY(1,1) ,EmailID int,  EmailGroupID int, Emailaddress varchar(255), Title varchar(50) , FirstName varchar(50), LastName varchar(50),  
		FullName varchar(50), Company varchar(100), Occupation varchar(50), Address varchar(255), Address2 varchar(255), City varchar(50),  
		State varchar(50), Zip varchar(50), Country varchar(50), Voice varchar(50), Mobile varchar(50), Fax varchar(50), Website varchar(50),  
		Age varchar(50), Income varchar(50), Gender varchar(50),   
		User1 varchar(255), User2 varchar(255), User3 varchar(255), User4 varchar(255), User5 varchar(255), User6 varchar(255), Birthdate datetime,  
		UserEvent1 varchar(50), UserEvent1Date datetime, UserEvent2 varchar(50), UserEvent2Date datetime,  notes varchar(1000), [password] varchar(25),
		formattypecode varchar(50),  
		subscribetypecode  varchar(1),  
		duplicateRecord bit,
		SkippedRecord bit ,
		ActionCode varchar(100),
		Reason varchar(100)
	)  

	CREATE INDEX EA_1 on #emails (Emailaddress)
	CREATE INDEX EA_2 on #emails (EmailID, SkippedRecord, duplicateRecord)


--,  CONSTRAINT PK PRIMARY KEY (tmpEmailID, Emailaddress)  
 
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlProfile  
  
  	print ('Step 2 : Import to Temp = '  + convert(varchar,getdate(),109))
 
	-- IMPORT FROM XML TO TEMP TABLE
	insert into #emails 
	(
		Emailaddress, Title, FirstName, LastName, FullName, Company,  
		Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, 
		UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, 
		notes, [password],
		formattypecode, subscribetypecode, SkippedRecord
	)  
	SELECT 
		LTRIM(RTRIM(Emailaddress)), Title,  FirstName,  LastName,  FullName,  Company,  
		Occupation,  Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax, 
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, case WHEN ISDATE(Birthdate)=1 THEN Birthdate ELSE NULL END , 
		UserEvent1, case WHEN ISDATE(UserEvent1Date)=1 THEN UserEvent1Date ELSE NULL END , UserEvent2, case WHEN ISDATE(UserEvent2Date)=1 THEN UserEvent2Date ELSE NULL END ,  
		Notes, [Password],
		case when isnull(formattypecode,'') not in ('html','text') then (case when len(rtrim(ltrim(@formattypecode))) = 0 then '' else @formattypecode end) else formattypecode end,  
		case when isnull(subscribetypecode,'') not in ('S','U','P','D','B') then '' else subscribetypecode end,
		--case when isnull(subscribetypecode,'') not in ('S','U','P','D','B') then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then '' else @subscribetypecode end) else subscribetypecode end,
		case when isnull(Emailaddress,'') = '' then 1 else 0 end
	FROM OPENXML(@docHandle, N'/XML/Emails')   
	WITH   
	(  
		Emailaddress varchar(255) 'emailaddress', Title varchar(50) 'title', FirstName varchar(50) 'firstname', LastName varchar(50) 'lastname',  
		FullName varchar(50) 'fullname', Company varchar(100) 'company', Occupation varchar(50) 'occupation', Address varchar(255) 'address',  
		Address2 varchar(255) 'address2', City varchar(50) 'city', State varchar(50) 'state', Zip varchar(50) 'zip', Country varchar(50) 'country',  
		Voice varchar(50) 'voice', Mobile varchar(50) 'mobile', Fax varchar(50) 'fax', Website varchar(50) 'website', Age varchar(50) 'age',  
		Income varchar(50) 'income', Gender varchar(50) 'gender', User1 varchar(255) 'user1',User2 varchar(255) 'user2', User3 varchar(255) 'user3', 
		User4 varchar(255) 'user4', User5 varchar(255) 'user5', User6 varchar(255) 'user6', Birthdate varchar(50) 'birthdate',  
		UserEvent1 varchar(50) 'userevent1', UserEvent1Date varchar(50) 'userevent1date', UserEvent2 varchar(50) 'userevent2',  
		UserEvent2Date varchar(50) 'userevent2date', notes varchar(1000) 'notes',  password varchar(25) 'password', 
		formattypecode varchar(50) 'formattypecode', subscribetypecode varchar(50) 'subscribetypecode'  
	)  
	
	insert into @result values ('T',@@ROWCOUNT)  
    declare @insertcount varchar(50)
    set @insertcount = (select COUNT(*) from #emails)
    
	UPDATE ImportEmailTracking SET EmailCount = (select COUNT(*) from #emails) WHERE TrackingID = @TrackingID

	print ('Step 2 check : '  + @insertcount )
	
	EXEC sp_xml_removedocument @docHandle    

	print ('Step 3 : VALIDATE EMAIL ADDRESS AND MARK AS SKIPERD FOR BAD EMAILS : '  + convert(varchar,getdate(),109))  

	Update #emails
	set 
		SkippedRecord = 1,
		Reason='Skipped', 
		ActionCode='S'
	where dbo.fn_ValidateEmailAddress(Emailaddress) = 0
	
	insert into @result
	select 'S', @@ROWCOUNT

	print ('Step 4 : Update Duplicates = '  + convert(varchar,getdate(),109))  
	
	--update duplicate records in temp tables  
	if (len(@compositekey) > 0)
	Begin
		exec ('update #emails  
		set duplicateRecord = 1  
		from	#emails e join   
				(select max(el.tmpEmailID) as tmpEmailID, el.emailaddress, el.' + @compositekey + ' from #emails el group by el.emailaddress, el.' + @compositekey + ') el1 
				on  e.emailaddress = el1.emailaddress  and e.' + @compositekey + ' = el1.' + @compositekey + ' where e.tmpemailID <> el1.tmpEmailID and SkippedRecord = 0 ')

	end
	else
	Begin
		update #emails  
		set 
			duplicateRecord = 1 ,
			Reason='DuplicateRecord', 
			ActionCode='D'  
		from	#emails e join   
				(select max(el.tmpEmailID) as tmpEmailID, el.emailaddress from #emails el group by el.emailaddress) el1 
				on  e.emailaddress = el1.emailaddress  
		where e.tmpemailID <> el1.tmpEmailID and SkippedRecord = 0 
	
	End	  
	insert into @result values ('D',@@ROWCOUNT)
	
	print ('Step 5 : Update EmailID & EmailgroupID = '  + convert(varchar,getdate(),109))  

	--#2 Check if emailaddress already exists in the group , if yes, update EmailID and [EmailGroups] ID   
	--[added as Patch to fix the duplicate issue with KMPS [Emails] - multiple emailaddress exists and when updating  OLD query picks the top emailaddress from different group]
	
	if (len(@compositekey) > 0)
	Begin
		exec ('update #emails  
		set		EmailID = e.EmailID, 
				EmailGroupID = eg.emailgroupID  
		from   
			#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress and el.' + @compositekey + ' = e.' + @compositekey + ' join [EmailGroups] eg with (NOLOCK) on eg.EmailID = e.EmailID
		where  
			customerID = ' + @customerID +  ' and eg.groupID = ' + @groupID + ' and SkippedRecord = 0 and el.emailID is null')
	End
	Else
	Begin
		update #emails  
		set		EmailID = e.EmailID, 
				EmailGroupID = eg.emailgroupID  
		from   
			#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress  join [EmailGroups] eg with (NOLOCK) on eg.EmailID = e.EmailID
		where  
			customerID = @customerID and eg.groupID = @groupID and SkippedRecord = 0 and el.emailID is null --and Isnull(el.duplicateRecord,0) <> 1  
	End
	
	print ('Step 6 : Update EmailID '  + convert(varchar,getdate(),109))  

	if (len(@compositekey) > 0)
	Begin
		exec ('update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress and el.' + @compositekey + ' = e.' + @compositekey + ' 
		where  
			customerID = ' + @customerID +  ' and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1 and el.emailID is null')
	End
	Else
	Begin
		update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress   
		where  
			customerID = @customerID and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1 and el.emailID is null
	End


	if @EmailAddressOnly = 0
	Begin
		print ('Step 7 : Update #emails = '  + convert(varchar,getdate(),109))  

		-- if emailID is not null - update [Emails] and insert/update in [EmailGroups] table  
		update [Emails]   
		set   
			Title = case when @overwritewithNULL = 1 then el.title else isnull(el.title, e.Title) end,  
			FirstName = case when @overwritewithNULL = 1 then el.FirstName else  isnull(el.FirstName, e.FirstName) end,  
			LastName = case when @overwritewithNULL = 1 then el.LastName else  isnull(el.LastName, e.LastName) end,  
			FullName = case when @overwritewithNULL = 1 then el.FullName else  isnull(el.FullName, e.FullName) end,  
			Company = case when @overwritewithNULL = 1 then el.Company else  isnull(el.Company, e.Company) end,  
			Occupation = case when @overwritewithNULL = 1 then el.Occupation else  isnull(el.Occupation, e.Occupation) end,  
			Address = case when @overwritewithNULL = 1 then el.Address else  isnull(el.Address, e.Address) end,  
			Address2 = case when @overwritewithNULL = 1 then el.Address2 else  isnull(el.Address2, e.Address2) end,  
			City = case when @overwritewithNULL = 1 then el.City else  isnull(el.City, e.City) end,  
			State = case when @overwritewithNULL = 1 then el.State else  isnull(el.State, e.State) end,  
			Zip = case when @overwritewithNULL = 1 then el.Zip else  isnull(el.Zip, e.Zip) end,  
			Country = case when @overwritewithNULL = 1 then el.Country else  isnull(el.Country, e.Country) end,  
			Voice = case when @overwritewithNULL = 1 then el.Voice else  isnull(el.Voice, e.Voice) end,  
			Mobile = case when @overwritewithNULL = 1 then el.Mobile else  isnull(el.Mobile, e.Mobile) end,  
			Fax = case when @overwritewithNULL = 1 then el.Fax else  isnull(el.Fax, e.Fax) end,  
			Website = case when @overwritewithNULL = 1 then el.Website else  isnull(el.Website, e.Website) end,  
			Age = case when @overwritewithNULL = 1 then el.Age else  isnull(el.Age, e.Age) end,  
			Income = case when @overwritewithNULL = 1 then el.Income else  isnull(el.Income, e.Income) end,  
			Gender = case when @overwritewithNULL = 1 then el.Gender else  isnull(el.Gender, e.Gender) end,  
			User1 = case when @overwritewithNULL = 1 then el.User1 else  isnull(el.User1, e.User1) end,  
			User2 = case when @overwritewithNULL = 1 then el.User2 else  isnull(el.User2, e.User2) end,  
			User3 = case when @overwritewithNULL = 1 then el.User3 else  isnull(el.User3, e.User3) end,  
			User4 = case when @overwritewithNULL = 1 then el.User4 else  isnull(el.User4, e.User4) end,  
			User5 = case when @overwritewithNULL = 1 then el.User5 else  isnull(el.User5, e.User5) end,  
			User6 = case when @overwritewithNULL = 1 then el.User6 else  isnull(el.User6, e.User6) end,  
			Birthdate = case when @overwritewithNULL = 1 then el.Birthdate else  isnull(el.Birthdate, e.Birthdate) end,  
			UserEvent1 = case when @overwritewithNULL = 1 then el.UserEvent1 else  isnull(el.UserEvent1, e.UserEvent1) end,  
			UserEvent1Date = case when @overwritewithNULL = 1 then el.UserEvent1Date else  isnull(el.UserEvent1Date, e.UserEvent1Date) end,  
			UserEvent2 = case when @overwritewithNULL = 1 then el.UserEvent2 else  isnull(el.UserEvent2, e.UserEvent2) end,  
			UserEvent2Date = case when @overwritewithNULL = 1 then el.UserEvent2Date else  isnull(el.UserEvent2Date, e.UserEvent2Date) end,  
			DateUpdated = @dt,
			notes = isnull(el.notes, e.notes) ,  
			password = isnull(el.password, e.password)
		from   
			#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailID = e.emailID --and customerID = @customerID NOT NEEDED
		where  
			el.emailID is not null and Isnull(duplicateRecord,0) <> 1  and SkippedRecord = 0
 	end
	 
	print ('Step 8 : Insert into [Emails]  = '  + convert(varchar,getdate(),109))  

	-- if emailID is null #emails table - insert into [Emails] and [EmailGroups]  
	insert into [Emails] (Emailaddress, CustomerID, Title, FirstName, LastName, FullName, Company,  
		Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, notes, password, DateAdded)  
	SELECT el.EmailAddress, @CustomerID, el.Title, el.FirstName, el.LastName, el.FullName, el.Company,  
		el.Occupation, el.Address, el.Address2, el.City, el.State, el.Zip, el.Country, el.Voice, el.Mobile, el.Fax,  
		el.Website, el.Age, el.Income, el.Gender, el.User1, el.User2, el.User3, el.User4, el.User5, el.User6,   
		el.Birthdate, el.UserEvent1, el.UserEvent1Date, el.UserEvent2, el.UserEvent2Date,el.notes,el.password, @dt  
	from #emails el where emailID is null and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1  

	print ('Step 9 : Update EmailID again for New [Emails]  = '  + convert(varchar,getdate(),109))  
	  
	-- update EmailID and [EmailGroups] ID  
	if (len(@compositekey) > 0)
	Begin
		exec ('update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress and el.' + @compositekey + ' = e.' + @compositekey + '   
		where  
			customerID = ' + @customerID +  ' and SkippedRecord = 0 and el.emailID is null ')
	End
	Else
	Begin
		update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress   
		where  
			customerID = @customerID and SkippedRecord = 0 and el.emailID is null --and Isnull(el.duplicateRecord,0) <> 1  
	
	End
		 
	print ('Step 10 : Update EmailGroupID = '  + convert(varchar,getdate(),109))  

	-- UPDATE @EMAIL WITH EMAILGROUPID
	update #emails  
	set 
		EmailGroupID = eg.emailgroupID ,
		Reason='UpdatedRecord' , 
		ActionCode='U'
	from   
		#emails el with (NOLOCK) join [EmailGroups] eg with (NOLOCK) on el.emailID = eg.emailID   
	where Isnull(el.duplicateRecord,0) <> 1 and SkippedRecord = 0 and eg.groupID = @groupID 

	insert into @result values ('U',@@ROWCOUNT) 

	print ('Step 11 : Update [EmailGroups] = '  + convert(varchar,getdate(),109))  

	--UPDATE [EmailGroups]
	UPDATE  [EmailGroups] 
	SET		FormatTypeCode = case when isnull(e.formattypecode,'') = '' then eg.formattypecode else lower(e.formattypecode) end,   
			SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) > 0 and eg.SubscribeTypeCode <> 'U' then upper(@subscribetypecode) else eg.subscribetypecode end) else upper(e.subscribetypecode) end,
			--SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then eg.subscribetypecode else upper(@subscribetypecode) end) else upper(e.subscribetypecode) end, 
			LastChanged = @dt,
			LastChangedSource = @Source
	from #emails e with (NOLOCK) join [EmailGroups] eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID 
	where Isnull(duplicateRecord,0) <> 1 and SkippedRecord = 0 and upper(eg.subscribetypecode) <> 'M'

	----UPDATE [EmailGroups]
	--UPDATE  [EmailGroups] 
	--SET		FormatTypeCode = case when isnull(e.formattypecode,'') = '' then eg.formattypecode else lower(e.formattypecode) end,   
	--		SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then eg.subscribetypecode else upper(e.subscribetypecode) end,   
	--		LastChanged = @dt 
	--from #emails e with (NOLOCK) join [EmailGroups] eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID  
	--where Isnull(duplicateRecord,0) <> 1 and SkippedRecord = 0 and upper(eg.subscribetypecode) <> 'U'
	  
	print ('Step 12 : Insert into [EmailGroups] = '  + convert(varchar,getdate(),109))  

	--INSERT INTO [EmailGroups]
	insert into [EmailGroups] (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn,CreatedSource)  
	select	EmailID, @groupID,   
			case when isnull(formattypecode,'') = '' then 'html' else lower(formattypecode) end,   
			--case when isnull(subscribetypecode,'') = '' then 'S' else upper(subscribetypecode) end,  
			case when len(rtrim(ltrim(subscribetypecode))) > 0 then subscribetypecode else (case when len(rtrim(ltrim(@subscribetypecode))) > 0 then upper(@subscribetypecode) else 'S' end) end, 
			@dt ,
			@Source
	from #emails   
	where emailgroupID is null and Isnull(duplicateRecord,0) <> 1 and SkippedRecord = 0 

	insert into @result values ('I',@@ROWCOUNT)

	  
	/************************************************  
	[EMAILDATAVALUES] - UDF FIELD updates  
	************************************************/  
	
	if exists(select top 1 GroupDatafieldsID from GroupDatafields where GroupID = @GroupID)
	Begin
	
		create TABLE #UDF 
		(  
			EmailID int,    
			Emailaddress varchar(255),  
			CompositeKeyValue varchar(255), 
			GroupDataFieldsID int ,   
			Datavalue varchar(500),   
			DatafieldSetID int,  
			EntryID uniqueIdentifier,  
			RowID int,
			edvID BIGINT--int  
		)  
		
		CREATE INDEX UDF_1 on #UDF (EmailID, GroupDataFieldsID)
		CREATE INDEX UDF_2 on #UDF (EmailID, GroupDataFieldsID, entryID)
		CREATE INDEX UDF_3 on #UDF (entryID)
	
		--print ('Step 2 : Create Temp UDF = '  + convert(varchar,getdate(),109))  
		
		select @tUDF_Exists = case when DatafieldSetID > 0 then 1 else 0 end from GroupDatafields where GroupID = @GroupID and IsDeleted = 0 and isnull(DatafieldSetID,0) > 0
		  
		EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlUDF  
		  
		insert into #UDF (Emailaddress, CompositeKeyValue, GroupDataFieldsID, Datavalue, EntryID, DataFieldsetID, RowID, edvID)  
		SELECT o.Emailaddress, o.CompositeKeyValue ,o.GroupDataFieldsid, o.datavalue, NULL, g.DataFieldsetID, RowID, 0  FROM   
		OPENXML(@docHandle, N'//row/udf')   
		WITH   
		(  
		  Emailaddress varchar(255) '../ea',  
		  CompositeKeyValue varchar(255) '../ea/@kv', 
		  GroupDataFieldsid int '@id',  
		  Datavalue varchar(500) 'v',  
		  Entrysid varchar(50) 'g',  
		  RowID int '@mp:parentid'  
		) o join Groupdatafields g on g.GroupDataFieldsid = o.GroupDataFieldsid  
		  
		EXEC sp_xml_removedocument @docHandle   
		  		  				
		if exists (select top 1 * from #UDF)   
		Begin  
		
		--COMMENTING OUT BECAUSE STANDALONE WEREN'T PART OF INITIAL REQUEST
		--This is for doing standalone udf default values
			--insert into #UDF (Emailaddress, GroupDataFieldsID, Datavalue, EntryID, DataFieldsetID, RowID, edvID, SurveyGridID)  
			--select distinct udf.Emailaddress, gdf2.GroupDatafieldsID, dbo.fn_GetGDFDValue(gdfd.DataValue, gdfd.SystemValue),null,gdf2.DatafieldSetID, udf.RowID, 0,null
			--from #UDF udf
			--join GroupDatafields gdf2 with(nolock) on gdf2.GroupID = @GroupID
			--join GroupDataFieldsDefault gdfd with(nolock) on gdf2.GroupDatafieldsID = gdfd.GDFID
			--where gdfd.GDFID is not null 
			--	and gdf2.GroupDatafieldsID not in (Select GroupDataFieldsID from #UDF) 
			--	and gdf2.DatafieldSetID is null 
			--	and gdf2.IsDeleted = 0
			--  and ISNULL(udf.Datavalue,'') <> ''
			
			--This is for doing transactional udf default values
			insert into #UDF (Emailaddress,CompositeKeyValue, GroupDataFieldsID, Datavalue, EntryID, DataFieldsetID, RowID, edvID)  
			select distinct udf.Emailaddress,udf.CompositeKeyValue, gdf2.GroupDatafieldsID, dbo.fn_GetGDFDValue(gdfd.DataValue, gdfd.SystemValue),null,gdf2.DatafieldSetID,udf.RowID, 0
			from #UDF udf
			join GroupDatafields gdf2 with(nolock) on gdf2.GroupID = @GroupID 
			join GroupDataFieldsDefault gdfd with(nolock) on gdf2.GroupDatafieldsID = gdfd.GDFID
			where gdfd.GDFID is not null 
				  and  gdf2.GroupDatafieldsID not in (Select GroupDataFieldsID from #UDF) 
				  and  gdf2.DatafieldSetID is not null 
				  and  gdf2.IsDeleted = 0
				  and gdf2.DatafieldSetID in (select distinct DataFieldSetID from #UDF)
				  and ISNULL(udf.Datavalue,'') <> ''
				  
			if (len(@compositekey) > 0)
			Begin
				exec ('update #UDF  
				set EmailID = e.EmailID  
				from   
					#UDF u join 
					[Emails] e  with (NOLOCK) on u.emailaddress = e.emailaddress  and u.CompositeKeyValue = e.' + @compositekey + ' and customerID = ' + @customerID + ' join  
					[EmailGroups] eg with (NOLOCK)  on e.emailID = eg.emailID and eg.groupID = '+ @groupID)
			End
			Else
			Begin
				update #UDF  
				set EmailID = e.EmailID  
				from   
					#UDF u join 
					[Emails] e  with (NOLOCK) on u.emailaddress = e.emailaddress and customerID = @customerID join  
					[EmailGroups] eg with (NOLOCK)  on e.emailID = eg.emailID and eg.groupID = @groupID   
			
			End
			
			--select * from #udf order by emailID
							
			------------------------------------------------------
			------------------ Transactional UDFs ------------------
			------------------------------------------------------
				
			if @tUDF_Exists = 1 -- IF TRANSACTION udf EXISTS
			Begin

				select @primaryKeyID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and DatafieldSetID > 0 and IsDeleted = 0 and IsPrimaryKey = 1
				
				if @primaryKeyID > 0 -- IF PRIMARY KEY EXISTS
				Begin
					insert into @entryID 
					select RowID, DatafieldSetID, edv.EntryID  
					from #UDF u join [EMAILDATAVALUES] edv
					on u.GroupDataFieldsID = edv.GroupDataFieldsID and u.EmailID = edv.EmailID and u.Datavalue = edv.DataValue
					where u.DataFieldsetID > 0 and  edv.GroupDataFieldsID = @primaryKeyID
				End

				-- Add Entry ID if need to maintain History.  
				insert into @entryID  
				select RowID, DatafieldSetID, newID() as EntryID from #UDF ud 
				where isnull(datafieldsetID,0) <> 0 and ROWid NOT IN (Select RowID from @entryID)
				group by RowID, DatafieldSetID  

				update #UDF  
				set EntryID = inn1.EntryID  
				from #UDF u join @entryID inn1  
				on inn1.rowID = u.rowID and inn1.DatafieldSetID = u.DatafieldSetID  
			
				if @primaryKeyID > 0 -- DO TRANSACTIONAL UPDATES ONLY IF PRIMARY KEY EXISTS
				Begin
					update #UDF
					set edvID = e.EmailDataValuesID
					from   
						#UDF u  with (NOLOCK)  join 
						[EMAILDATAVALUES] e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID and e.entryID = u.entryID
					where u.EmailID is not null and u.EntryID is not null
						
					
					update EmailDataValues
					set DataValue = u.Datavalue, ModifiedDate = @dt
					from   
						[EMAILDATAVALUES] e join #UDF u  on e.EmailDataValuesID = u.edvID
					where u.edvID > 0
				End	
					
				-- New UDF Records  - Transactions  
				Insert into [EMAILDATAVALUES] (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, EntryID)  
				select EmailID, GroupDatafieldsID, DataValue, @dt, EntryID   
				from #UDF 
				where EmailID is not null and  EntryID is not null and Isnull(DataValue,'') <> ''  and edvID = 0
	
			End
			
			------------------------------------------------------
			------------------ stand alone UDFs ------------------
			------------------------------------------------------
		  
			update #UDF
				set edvID = e.EmailDataValuesID
			from   
				#UDF u  with (NOLOCK)  join 
				[EMAILDATAVALUES] e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID
			where u.EmailID is not null and u.EntryID is null
						
		  
			-- Existing UDF Records   
			update [EMAILDATAVALUES]  
			set DataValue = u.datavalue,  
				ModifiedDate = @dt 
			from   
				[EMAILDATAVALUES] e  with (NOLOCK) join #UDF u with (NOLOCK) on e.EmailDataValuesID = u.edvID 
			where   
				u.EntryID is null  
		  
			-- New UDF Records    
			Insert into [EMAILDATAVALUES] (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, EntryID)  
			select u.EmailID, u.GroupDatafieldsID, MAX(u.DataValue), @dt, NULL   
			from	#UDF u  with (NOLOCK) 
			where 
					u.edvID = 0 and  --u.edvID is null and 
					u.EmailID is not null and 
					u.EntryID is null and 
					Isnull(u.DataValue,'') <> ''  
			group by u.EmailID, u.GroupDatafieldsID
			
		End  
		drop TABLE #UDF
	End
	
	update 
		#emails 
	set 
		ActionCode='I', 
		Reason=''
	WHERE 
		ActionCode is null

	insert into ImportEmailsLog(
		Emailaddress, 
		GroupID,
		ActionCode, 
		[FileName], 
		Reason)
	select  
		Emailaddress,
		@GroupID, 
		ActionCode, 
		@filename, 
		Reason 
	from 
		#emails

	drop TABLE #emails

	UPDATE ImportEmailTracking SET EndTime = GETDATE() WHERE TrackingID = @TrackingID

	select * from @result  
END