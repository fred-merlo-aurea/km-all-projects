------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
--
--
--
------------------------------------------------------

--DBCC FREEPROCCACHE WITH NO_INFOMSGS;

CREATE proc [dbo].[sp_importEmails_with_MERGE]
(  
 @CustomerID int,  
 @GroupID int,  
 @xmlProfile text,  
 @xmlUDF text,
 @formattypecode varchar(50),
 @subscribetypecode varchar(50),
 @EmailAddressOnly bit
)  
as  
Begin  
  
	set nocount on  

	declare		@docHandle int,
					@dt datetime,
					@primaryKeyID int,
					@tUDF_Exists bit
		
	set	@primaryKeyID = 0
	set	@tUDF_Exists = 0
	set	@dt = getdate()
 
	declare @result TABLE  (Action varchar(100), Counts int)  
	DECLARE @SummaryOfChanges TABLE(Change VARCHAR(20));
	
	declare @entryID TABLE (RowID int, DatafieldSetID int, entryID uniqueidentifier)  
  
  	
	/******************************* Step 2 : Step 1 : Create Temp [Emails] and Index *****************************************/
	create TABLE #emails   
	(  
		tmpEmailID int IDENTITY(1,1) ,EmailID int,  EmailGroupID int, Emailaddress varchar(255), Title varchar(50) , FirstName varchar(50), LastName varchar(50),  
		FullName varchar(50), Company varchar(100), Occupation varchar(50), Address varchar(255), Address2 varchar(255), City varchar(50),  
		State varchar(50), Zip varchar(50), Country varchar(50), Voice varchar(50), Mobile varchar(50), Fax varchar(50), Website varchar(50),  
		Age varchar(50), Income varchar(50), Gender varchar(50),   
		User1 varchar(255), User2 varchar(255), User3 varchar(255), User4 varchar(255), User5 varchar(255), User6 varchar(255), Birthdate datetime,  
		UserEvent1 varchar(50), UserEvent1Date datetime, UserEvent2 varchar(50), UserEvent2Date datetime,  notes varchar(1000), [password] varchar(25),
		formattypecode varchar(50), subscribetypecode  varchar(50), duplicateRecord bit, SkippedRecord bit  
	)  

	CREATE INDEX EA_1 on #emails (Emailaddress)
	CREATE INDEX EA_2 on #emails (EmailID, SkippedRecord, duplicateRecord)

   
	/******************************* Step 2 : IMPORT FROM XML TO TEMP TABLE *****************************************/
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlProfile  
	
	insert into #emails 
	(
		Emailaddress, Title, FirstName, LastName, FullName, Company,  
		Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, 
		UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, 
		notes, [password],  formattypecode, subscribetypecode, SkippedRecord, duplicateRecord
	)  
	SELECT 
		LTRIM(RTRIM(Emailaddress)), Title,  FirstName,  LastName,  FullName,  Company,  
		Occupation,  Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax, 
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, case WHEN ISDATE(Birthdate)=1 THEN Birthdate ELSE NULL END , 
		UserEvent1, case WHEN ISDATE(UserEvent1Date)=1 THEN UserEvent1Date ELSE NULL END , UserEvent2, case WHEN ISDATE(UserEvent2Date)=1 THEN UserEvent2Date ELSE NULL END ,  
		Notes, [Password],
		lower(case when isnull(formattypecode,'') not in ('html','text') then (case when len(rtrim(ltrim(@formattypecode))) = 0 then 'html' else @formattypecode end) else formattypecode end),  
		lower(case when isnull(subscribetypecode,'') not in ('S','U','P','D','B') then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then 'S' else @subscribetypecode end) else subscribetypecode end),
		case when isnull(Emailaddress,'') = '' then 1 else 0 end, 0
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
	
	EXEC sp_xml_removedocument @docHandle    

	/******************************* Step 3 : VALIDATE EMAIL ADDRESS AND MARK AS SKIPERD FOR BAD EMAILS  ***************************************/

	Update #emails
	set SkippedRecord = 1
	where 
		NOT  (   
				CHARINDEX ( ' ',Emailaddress) = 0  AND  -- No embedded spaces
				CHARINDEX ( '''',Emailaddress) = 0  AND  -- No single quotes  
				CHARINDEX ( '"',Emailaddress) = 0  AND  -- No single quotes  
				CHARINDEX ( '(',Emailaddress) = 0  AND  -- No ( brackets
				CHARINDEX ( ')',Emailaddress) = 0  AND  -- No ) brackets
				CHARINDEX ( '.',Emailaddress,CHARINDEX ( '@',Emailaddress)) - CHARINDEX ( '@',Emailaddress) > 1 AND  -- There must be a '.' after '@'
				LEFT(Emailaddress,1) <> '@' AND -- '@' can't be the first character of an email address  
				RIGHT(Emailaddress,1) <> '.' AND -- '.' can't be the last character of an email address
				LEN(Emailaddress) - LEN(REPLACE(Emailaddress,'@','')) = 1 AND   -- Only one '@' sign is allowed
				CHARINDEX ( '.',REVERSE(Emailaddress)) >= 3 AND    -- Domain name should end with at least 2 character extension
				CHARINDEX ('.@',Emailaddress) = 0 AND CHARINDEX ( '..',Emailaddress) = 0 -- can't have patterns like '.@' and '..'
			)  

	insert into @result
	select 'S', @@ROWCOUNT

	/******************************* Step 4 : Update Duplicates ***************************************/
	
	--update duplicate records in temp tables  
	update #emails  
	set duplicateRecord = 1  
	from	#emails e join   
			(select max(el.tmpEmailID) as tmpEmailID, el.emailaddress from #emails el group by el.emailaddress) el1 
			on  e.emailaddress = el1.emailaddress  
	where e.tmpemailID <> el1.tmpEmailID and SkippedRecord = 0 
	  
	insert into @result values ('D',@@ROWCOUNT)
	
	/******************************* Step 5 : Insert & Update Emails ***************************************/
	
	MERGE INTO [Emails] AS Target
	USING 
		(select   * from #emails where SkippedRecord = 0 and  duplicateRecord = 0) AS Source 
		ON Target.CustomerID = @CustomerID and Target.Emailaddress = Source.Emailaddress
		WHEN MATCHED THEN
			update 
				set   
					Title = isnull(Target.title, Source.Title),  
					FirstName = isnull(Target.FirstName, Source.FirstName),  
					LastName = isnull(Target.LastName, Source.LastName),  
					FullName = isnull(Target.FullName, Source.FullName),  
					Company = isnull(Target.Company, Source.Company),  
					Occupation = isnull(Target.Occupation, Source.Occupation),  
					Address = isnull(Target.Address, Source.Address),  
					Address2 = isnull(Target.Address2, Source.Address2),  
					City = isnull(Target.City, Source.City),  
					State = isnull(Target.State, Source.State),  
					Zip = isnull(Target.Zip, Source.Zip),  
					Country = isnull(Target.Country, Source.Country),  
					Voice = isnull(Target.Voice, Source.Voice),  
					Mobile = isnull(Target.Mobile, Source.Mobile),  
					Fax = isnull(Target.Fax, Source.Fax),  
					Website = isnull(Target.Website, Source.Website),  
					Age = isnull(Target.Age, Source.Age),  
					Income = isnull(Target.Income, Source.Income),  
					Gender = isnull(Target.Gender, Source.Gender),  
					User1 = isnull(Target.User1, Source.User1),  
					User2 = isnull(Target.User2, Source.User2),  
					User3 = isnull(Target.User3, Source.User3),  
					User4 = isnull(Target.User4, Source.User4),  
					User5 = isnull(Target.User5, Source.User5),  
					User6 = isnull(Target.User6, Source.User6),  
					Birthdate = isnull(Target.Birthdate, Source.Birthdate),  
					UserEvent1 = isnull(Target.UserEvent1, Source.UserEvent1),  
					UserEvent1Date = isnull(Target.UserEvent1Date, Source.UserEvent1Date),  
					UserEvent2 = isnull(Target.UserEvent2, Source.UserEvent2),  
					UserEvent2Date = isnull(Target.UserEvent2Date, Source.UserEvent2Date),  
					DateUpdated = @dt,
					notes = isnull(Target.notes, Source.notes),  
					password = isnull(Target.password, Source.password)
		WHEN NOT MATCHED BY TARGET THEN
			INSERT 
			(
					Emailaddress, CustomerID, Title, FirstName, LastName, FullName, Company,  
					Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
					Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, notes, password, DateAdded
			)  
			values 
			(
					Source.EmailAddress, @CustomerID, Source.Title, Source.FirstName, Source.LastName, Source.FullName, Source.Company,  
					Source.Occupation, Source.Address, Source.Address2, Source.City, Source.State, Source.Zip, Source.Country, Source.Voice, Source.Mobile, Source.Fax,  
					Source.Website, Source.Age, Source.Income, Source.Gender, Source.User1, Source.User2, Source.User3, Source.User4, Source.User5, Source.User6,   
					Source.Birthdate, Source.UserEvent1, Source.UserEvent1Date, Source.UserEvent2, Source.UserEvent2Date,Source.notes,Source.password, @dt 
			);
		
	/******************************* Step 5 : Insert & Update [EmailGroups] ***************************************/
	
	MERGE INTO [EmailGroups] AS Target
	USING 
		(
			select	e.EmailID, t.formattypecode, t.subscribetypecode
			from		#emails t Join 
						[Emails] e on t.emailaddress = e.emailaddress
			where 
						e.CustomerID = @CustomerID and
						t.SkippedRecord = 0 and 
						t.duplicateRecord = 0
		) 
		AS Source 
		ON Target.GroupID = @groupID and Target.EmailID = Source.EmailID 
		WHEN MATCHED AND  upper(Target.subscribetypecode) <> 'U' THEN
			update 
				set   
						FormatTypeCode = Source.formattypecode,   
						SubscribeTypeCode = Source.subscribetypecode,   
						LastChanged = @dt 
		WHEN NOT MATCHED BY TARGET THEN 
			INSERT 
			(
					EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn
			)  
			values 
			(
					Source.EmailID, @groupID, Source.formattypecode,  Source.subscribetypecode, @dt 
			)
			OUTPUT $action INTO @SummaryOfChanges;

	insert into @result
	SELECT	case when Change='update' then 'U' when Change='Insert' then 'I' end, 
			COUNT(*) AS CountPerChange
	FROM @SummaryOfChanges
	GROUP BY Change

	drop TABLE #emails

	/************************************************  
	[EMAILDATAVALUES] - UDF FIELD updates  
	************************************************/  
	
	if exists(select top 1 GroupDatafieldsID from GroupDatafields where GroupID = @GroupID)
	Begin
	
		create TABLE #UDF 
		(  
			EmailID int,    
			Emailaddress varchar(255),   
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
		  
		insert into #UDF (Emailaddress, GroupDataFieldsID, Datavalue, EntryID, DataFieldsetID, RowID, edvID)  
		SELECT o.Emailaddress, o.GroupDataFieldsid, o.datavalue, NULL, g.DataFieldsetID, RowID, 0  FROM   
		OPENXML(@docHandle, N'//row/udf')   
		WITH   
		(  
		  Emailaddress varchar(255) '../ea',  
		  GroupDataFieldsid int '@id',  
		  Datavalue varchar(500) 'v',  
		  Entrysid varchar(50) 'g',  
		  RowID int '@mp:parentid'  
		) o join Groupdatafields g on g.GroupDataFieldsid = o.GroupDataFieldsid  
		  
		EXEC sp_xml_removedocument @docHandle   
		  		  				
		if exists (select top 1 * from #UDF)   
		Begin  
		
			update #UDF  
			set EmailID = e.EmailID  
			from   
				#UDF u join 
				[Emails] e  with (NOLOCK) on u.emailaddress = e.emailaddress and customerID = @customerID join  
				[EmailGroups] eg with (NOLOCK)  on e.emailID = eg.emailID and eg.groupID = @groupID   
				
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
	
	
	select * from @result  
END