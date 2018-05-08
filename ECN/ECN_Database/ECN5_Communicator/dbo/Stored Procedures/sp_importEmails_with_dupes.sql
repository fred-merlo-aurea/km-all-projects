/****** Object:  StoredProcedure [dbo].[sp_importEmails_with_dupes]    Script Date: 01/28/2014 13:59:09 ******/

------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
--
--
--
------------------------------------------------------


CREATE proc [dbo].[sp_importEmails_with_dupes]
(
 @CustomerID int,  
 @GroupID int,  
 @xmlProfile TEXT,  
 @xmlUDF TEXT,
 @formattypecode varchar(50),
 @subscribetypecode varchar(50),
 @EmailAddressOnly bit,
 @compositekey varchar(50) = null,
 @overwritewithNULL bit = 0
)
as

Begin  

-- set @CustomerID = 1
-- set @GroupID = 52280
-- set @xmlProfile = '<XML>
-- <Emails>
--<emailaddress>sunil@knowledgemarketing.com</emailaddress>
--<title></title>
--<firstname></firstname>
--<lastname></lastname>
--<lastname></lastname>
--<company></company>
--<occupation></occupation>
--<address></address>
--<address2></address2>
--<city></city>
--<state></state>
--<zip></zip>
--<country></country>
--<voice></voice>
--<mobile></mobile>
--<fax></fax>
--<website></website>
--<age></age>
--<income></income>
--<gender></gender>
--<user1>11111</user1>
--<user2></user2>
--<user3></user3>
--<user4></user4>
--<user5></user5>
--<user6></user6>
--<birthdate></birthdate>
--<userevent1></userevent1>
--<userevent1date></userevent1date>
--<userevent2></userevent2>
--<userevent2date></userevent2date>
--<notes></notes>
--<formattypecode></formattypecode>
--<subscribetypecode></subscribetypecode>
--</Emails>
--<Emails>
--<emailaddress>sunil@knowledgemarketing.com</emailaddress>
--<title></title>
--<firstname></firstname>
--<lastname></lastname>
--<lastname></lastname>
--<company></company>
--<occupation></occupation>
--<address></address>
--<address2></address2>
--<city></city>
--<state></state>
--<zip></zip>
--<country></country>
--<voice></voice>
--<mobile></mobile>
--<fax></fax>
--<website></website>
--<age></age>
--<income></income>
--<gender></gender>
--<user1>22222</user1>
--<user2></user2>
--<user3></user3>
--<user4></user4>
--<user5></user5>
--<user6></user6>
--<birthdate></birthdate>
--<userevent1></userevent1>
--<userevent1date></userevent1date>
--<userevent2></userevent2>
--<userevent2date></userevent2date>
--<notes></notes>
--<formattypecode></formattypecode>
--<subscribetypecode></subscribetypecode>
--</Emails>
--<Emails>
--<emailaddress>sunil4@knowledgemarketing.com</emailaddress>
--<title></title>
--<firstname></firstname>
--<lastname></lastname>
--<lastname></lastname>
--<company></company>
--<occupation></occupation>
--<address></address>
--<address2></address2>
--<city></city>
--<state></state>
--<zip></zip>
--<country></country>
--<voice></voice>
--<mobile></mobile>
--<fax></fax>
--<website></website>
--<age></age>
--<income></income>
--<gender></gender>
--<user1>33333</user1>
--<user2></user2>
--<user3></user3>
--<user4></user4>
--<user5></user5>
--<user6></user6>
--<birthdate></birthdate>
--<userevent1></userevent1>
--<userevent1date></userevent1date>
--<userevent2></userevent2>
--<userevent2date></userevent2date>
--<notes></notes>
--<formattypecode></formattypecode>
--<subscribetypecode></subscribetypecode>
--</Emails>

--</XML>'

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

-- set @formattypecode = 'html'
-- set @subscribetypecode = 'S'
-- set @EmailAddressOnly = 0
-- set @compositekey = 'user1'
-- set @overwritewithNULL = 0
  
	set nocount on  

	declare @docHandle int,
			@dt datetime,
			@primaryKeyID int,
			@tUDF_Exists bit
	
	set @primaryKeyID = 0
	set @tUDF_Exists = 0
	set @dt = getdate()
 

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
		SkippedRecord bit  
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
    
	print ('Step 2 check : '  + @insertcount )
	
	EXEC sp_xml_removedocument @docHandle    

	print ('Step 3 : VALIDATE EMAIL ADDRESS AND MARK AS SKIPERD FOR BAD EMAILS : '  + convert(varchar,getdate(),109))  

	Update #emails
	set SkippedRecord = 1
	where dbo.fn_ValidateEmailAddress(Emailaddress) = 0
	--where 
	--	NOT  (   
	--			LEN(ltrim(rtrim(Emailaddress))) <> 0 AND 		
	--			CHARINDEX ( ' ',Emailaddress) = 0  AND  -- No embedded spaces
	--			CHARINDEX ( '''',Emailaddress) = 0  AND  -- No single quotes  
	--			CHARINDEX ( '"',Emailaddress) = 0  AND  -- No single quotes  
	--			CHARINDEX ( '(',Emailaddress) = 0  AND  -- No ( brackets
	--			CHARINDEX ( ')',Emailaddress) = 0  AND  -- No ) brackets
	--			CHARINDEX ( '.',Emailaddress,CHARINDEX ( '@',Emailaddress)) - CHARINDEX ( '@',Emailaddress) > 1 AND  -- There must be a '.' after '@'
	--			LEFT(Emailaddress,1) <> '@' AND -- '@' can't be the first character of an email address  
	--			RIGHT(Emailaddress,1) <> '.' AND -- '.' can't be the last character of an email address
	--			LEN(Emailaddress) - LEN(REPLACE(Emailaddress,'@','')) = 1 AND   -- Only one '@' sign is allowed
	--			CHARINDEX ( '.',REVERSE(Emailaddress)) >= 3 AND    -- Domain name should end with at least 2 character extension
	--			CHARINDEX ('.@',Emailaddress) = 0 AND CHARINDEX ( '..',Emailaddress) = 0 -- can't have patterns like '.@' and '..'
	--		)  

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
		set duplicateRecord = 1  
		from	#emails e join   
				(select max(el.tmpEmailID) as tmpEmailID, el.emailaddress from #emails el group by el.emailaddress) el1 
				on  e.emailaddress = el1.emailaddress  
		where e.tmpemailID <> el1.tmpEmailID and SkippedRecord = 0 
	
	End	  
	insert into @result values ('D',@@ROWCOUNT)
	
	print ('Step 5 : Update EmailID & EmailgroupID = '  + convert(varchar,getdate(),109))  

	--#2 Check if emailaddress already exists in the group , if yes, update EmailID and EmailGroup ID   
	--[added as Patch to fix the duplicate issue with KMPS emails - multiple emailaddress exists and when updating  OLD query picks the top emailaddress from different group]
	
	if (len(@compositekey) > 0)
	Begin
		exec ('update #emails  
		set		EmailID = e.EmailID, 
				EmailGroupID = eg.emailgroupID  
		from   
			#emails el with (NOLOCK) join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress and el.' + @compositekey + ' = e.' + @compositekey + ' join EmailGroups eg with (NOLOCK) on eg.EmailID = e.EmailID
		where  
			customerID = ' + @customerID +  ' and eg.groupID = ' + @groupID + ' and SkippedRecord = 0 and el.emailID is null')
	End
	Else
	Begin
		update #emails  
		set		EmailID = e.EmailID, 
				EmailGroupID = eg.emailgroupID  
		from   
			#emails el with (NOLOCK) join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress  join EmailGroups eg with (NOLOCK) on eg.EmailID = e.EmailID
		where  
			customerID = @customerID and eg.groupID = @groupID and SkippedRecord = 0 and el.emailID is null --and Isnull(el.duplicateRecord,0) <> 1  
	End
	
	print ('Step 6 : Update EmailID '  + convert(varchar,getdate(),109))  

	if (len(@compositekey) > 0)
	Begin
		exec ('update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress and el.' + @compositekey + ' = e.' + @compositekey + ' 
		where  
			customerID = ' + @customerID +  ' and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1 and el.emailID is null')
	End
	Else
	Begin
		update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress   
		where  
			customerID = @customerID and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1 and el.emailID is null
	End


	if @EmailAddressOnly = 0
	Begin
		print ('Step 7 : Update #Emails = '  + convert(varchar,getdate(),109))  

		-- if emailID is not null - update emails and insert/update in emailgroups table  
		update emails   
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
			Birthdate = case when @overwritewithNULL = 1 then el.title else  isnull(el.Birthdate, e.Birthdate) end,  
			UserEvent1 = case when @overwritewithNULL = 1 then el.title else  isnull(el.UserEvent1, e.UserEvent1) end,  
			UserEvent1Date = case when @overwritewithNULL = 1 then el.title else  isnull(el.UserEvent1Date, e.UserEvent1Date) end,  
			UserEvent2 = case when @overwritewithNULL = 1 then el.title else  isnull(el.UserEvent2, e.UserEvent2) end,  
			UserEvent2Date = case when @overwritewithNULL = 1 then el.title else  isnull(el.UserEvent2Date, e.UserEvent2Date) end,  
			DateUpdated = @dt,
			notes = isnull(el.notes, e.notes) ,  
			password = isnull(el.password, e.password)
		from   
			#emails el with (NOLOCK) join emails e with (NOLOCK) on el.emailID = e.emailID --and customerID = @customerID NOT NEEDED
		where  
			el.emailID is not null and Isnull(duplicateRecord,0) <> 1  and SkippedRecord = 0
 	end
	 
	print ('Step 8 : Insert into Emails  = '  + convert(varchar,getdate(),109))  

	-- if emailID is null #emails table - insert into emails and emailgroups  
	insert into emails (Emailaddress, CustomerID, Title, FirstName, LastName, FullName, Company,  
		Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, notes, password, DateAdded)  
	SELECT el.EmailAddress, @CustomerID, el.Title, el.FirstName, el.LastName, el.FullName, el.Company,  
		el.Occupation, el.Address, el.Address2, el.City, el.State, el.Zip, el.Country, el.Voice, el.Mobile, el.Fax,  
		el.Website, el.Age, el.Income, el.Gender, el.User1, el.User2, el.User3, el.User4, el.User5, el.User6,   
		el.Birthdate, el.UserEvent1, el.UserEvent1Date, el.UserEvent2, el.UserEvent2Date,el.notes,el.password, @dt  
	from #emails el where emailID is null and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1  

	print ('Step 9 : Update EmailID again for New Emails  = '  + convert(varchar,getdate(),109))  
	  
	-- update EmailID and EmailGroup ID  
	if (len(@compositekey) > 0)
	Begin
		exec ('update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress and el.' + @compositekey + ' = e.' + @compositekey + '   
		where  
			customerID = ' + @customerID +  ' and SkippedRecord = 0 and el.emailID is null ')
	End
	Else
	Begin
		update #emails  
		set EmailID = e.EmailID  
		from   
			#emails el with (NOLOCK) join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress   
		where  
			customerID = @customerID and SkippedRecord = 0 and el.emailID is null --and Isnull(el.duplicateRecord,0) <> 1  
	
	End
		 
	print ('Step 10 : Update EmailGroupID = '  + convert(varchar,getdate(),109))  

	-- UPDATE @EMAIL WITH EMAILGROUPID
	update #emails  
	set EmailGroupID = eg.emailgroupID  
	from   
		#emails el with (NOLOCK) join emailgroups eg with (NOLOCK) on el.emailID = eg.emailID   
	where Isnull(el.duplicateRecord,0) <> 1 and SkippedRecord = 0 and eg.groupID = @groupID 

	insert into @result values ('U',@@ROWCOUNT) 

	print ('Step 11 : Update EmailGroups = '  + convert(varchar,getdate(),109))  

	--UPDATE [EmailGroups]
	UPDATE  [EmailGroups] 
	SET		FormatTypeCode = case when isnull(e.formattypecode,'') = '' then eg.formattypecode else lower(e.formattypecode) end,   
			SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) > 0 and eg.SubscribeTypeCode <> 'U' then upper(@subscribetypecode) else eg.subscribetypecode end) else upper(e.subscribetypecode) end,
			--SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then eg.subscribetypecode else upper(@subscribetypecode) end) else upper(e.subscribetypecode) end, 
			LastChanged = @dt
	from #emails e with (NOLOCK) join [EmailGroups] eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID 
	where Isnull(duplicateRecord,0) <> 1 and SkippedRecord = 0 and upper(eg.subscribetypecode) <> 'M'
	  
	----UPDATE EMAILGROUPS
	--UPDATE  EmailGroups 
	--SET		FormatTypeCode = case when isnull(e.formattypecode,'') = '' then eg.formattypecode else lower(e.formattypecode) end,   
	--		SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then eg.subscribetypecode else upper(e.subscribetypecode) end,   
	--		LastChanged = @dt 
	--from #emails e with (NOLOCK) join emailgroups eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID  
	--where Isnull(duplicateRecord,0) <> 1 and SkippedRecord = 0 and upper(eg.subscribetypecode) <> 'U'
	  
	print ('Step 12 : Insert into EmailGroups = '  + convert(varchar,getdate(),109))  

	--INSERT INTO EMAILGROUPS
	insert into EmailGroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn)  
	select	EmailID, @groupID,   
			case when isnull(formattypecode,'') = '' then 'html' else lower(formattypecode) end,   
			--case when isnull(subscribetypecode,'') = '' then 'S' else upper(subscribetypecode) end, 
			case when len(rtrim(ltrim(subscribetypecode))) > 0 then subscribetypecode else (case when len(rtrim(ltrim(@subscribetypecode))) > 0 then upper(@subscribetypecode) else 'S' end) end,  
			@dt 
	from #emails   
	where emailgroupID is null and Isnull(duplicateRecord,0) <> 1 and SkippedRecord = 0 

	insert into @result values ('I',@@ROWCOUNT)

	  
	/************************************************  
	EMAILDATAVALUES - UDF FIELD updates  
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
			edvID BIGINT --int  
		)  
		
		CREATE INDEX UDF_1 on #UDF (EmailID, GroupDataFieldsID)
		CREATE INDEX UDF_2 on #UDF (EmailID, GroupDataFieldsID, entryID)
		CREATE INDEX UDF_3 on #UDF (entryID)
	
		--print ('Step 2 : Create Temp UDF = '  + convert(varchar,getdate(),109))  
		
		select @tUDF_Exists = case when DatafieldSetID > 0 then 1 else 0 end from GroupDatafields where GroupID = @GroupID and isnull(DatafieldSetID,0) > 0
		  
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
		
			if (len(@compositekey) > 0)
			Begin
				exec ('update #UDF  
				set EmailID = e.EmailID  
				from   
					#UDF u join 
					Emails e  with (NOLOCK) on u.emailaddress = e.emailaddress  and u.CompositeKeyValue = e.' + @compositekey + ' and customerID = ' + @customerID + ' join  
					emailgroups eg with (NOLOCK)  on e.emailID = eg.emailID and eg.groupID = '+ @groupID)
			End
			Else
			Begin
				update #UDF  
				set EmailID = e.EmailID  
				from   
					#UDF u join 
					Emails e  with (NOLOCK) on u.emailaddress = e.emailaddress and customerID = @customerID join  
					emailgroups eg with (NOLOCK)  on e.emailID = eg.emailID and eg.groupID = @groupID   
			
			End
			
			--select * from #udf order by emailID
							
			------------------------------------------------------
			------------------ Transactional UDFs ------------------
			------------------------------------------------------
				
			if @tUDF_Exists = 1 -- IF TRANSACTION udf EXISTS
			Begin

				select @primaryKeyID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and DatafieldSetID > 0 and IsPrimaryKey = 1
				
				if @primaryKeyID > 0 -- IF PRIMARY KEY EXISTS
				Begin
					insert into @entryID 
					select RowID, DatafieldSetID, edv.EntryID  
					from #UDF u join EmailDataValues edv
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
						emaildatavalues e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID and e.entryID = u.entryID
					where u.EmailID is not null and u.EntryID is not null
						
					
					update EmailDataValues
					set DataValue = u.Datavalue, ModifiedDate = @dt
					from   
						emaildatavalues e join #UDF u  on e.EmailDataValuesID = u.edvID
					where u.edvID > 0
				End	
					
				-- New UDF Records  - Transactions  
				Insert into Emaildatavalues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, EntryID)  
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
				emaildatavalues e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID
			where u.EmailID is not null and u.EntryID is null
						
		  
			-- Existing UDF Records   
			update emaildatavalues  
			set DataValue = u.datavalue,  
				ModifiedDate = @dt 
			from   
				emaildatavalues e  with (NOLOCK) join #UDF u with (NOLOCK) on e.EmailDataValuesID = u.edvID 
			where   
				u.EntryID is null  
		  
			-- New UDF Records    
			Insert into Emaildatavalues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, EntryID)  
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
	
	drop TABLE #emails
	
	select * from @result  
END