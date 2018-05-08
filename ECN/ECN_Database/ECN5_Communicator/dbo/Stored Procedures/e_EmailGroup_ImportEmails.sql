------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
-- 2015-06-17 Added While loop for Inserts and updates to Emails, EmailGroups and EmaildataValuse
--			  Reordered steps 5 & 6
-- 2016-01-22 WGH - Updated proc for recording Source and getting the correct emailid for exisiting email(emailgroups dupe prevention)
-- 2016-02-04 WGH - Updated proc to allow JF to have special code around suppression
--
------------------------------------------------------

CREATE proc [dbo].[e_EmailGroup_ImportEmails]
(  
 @CustomerID int,  
 @GroupID int,  
 @xmlProfile text,  
 @xmlUDF text,
 @formattypecode varchar(50),
 @subscribetypecode varchar(50),
 @EmailAddressOnly bit,
 @UserID int, 
 @filename varchar(200) ='',
 @source varchar(200) = 'Unknown',
 @insertMS bit = 0
)  
as  
Begin  
	set nocount on  	

	declare @docHandle int,
			@dt datetime,
			@primaryKeyID int,
			@tUDF_Exists bit,
			@skippedcount int,
			@TrackingID int
	
	set @primaryKeyID = 0
	set @tUDF_Exists = 0
	set @dt = getdate()
	set @skippedcount = 0
	
	SET @TrackingID = 0
	INSERT INTO ImportEmailTracking (CustomerID, GroupID, StartTime, Source) VALUES (@CustomerID, @GroupID, @dt, @source)
	SELECT @TrackingID = @@IDENTITY

	if @GroupID = 0
		return

	declare @result TABLE  (Action varchar(100), Counts int)  

	declare @entryID TABLE (RowID int, DatafieldSetID int, entryID uniqueidentifier)  
  
  	--print ('Step 1 : Create Temp [Emails] and Index = '  + convert(varchar,getdate(),109))

	create TABLE #emails 
	(  
		tmpEmailID int IDENTITY(1,1) ,EmailID int,  EmailGroupID int, Emailaddress varchar(255), Title varchar(50) , FirstName varchar(50), LastName varchar(50),  
		FullName varchar(50), Company varchar(100), Occupation varchar(50), Address varchar(255), Address2 varchar(255), City varchar(50),  
		State varchar(50), Zip varchar(50), Country varchar(50), Voice varchar(50), Mobile varchar(50), Fax varchar(50), Website varchar(50),  
		Age varchar(50), Income varchar(50), Gender varchar(50),   
		User1 varchar(255), User2 varchar(255), User3 varchar(255), User4 varchar(255), User5 varchar(255), User6 varchar(255), Birthdate datetime,  
		UserEvent1 varchar(50), UserEvent1Date datetime, UserEvent2 varchar(50), UserEvent2Date datetime,  notes varchar(1000), [password] varchar(25),
		formattypecode varchar(50),  
		subscribetypecode  varchar(1), smsenabled varchar(50),  
		duplicateRecord bit,
		SkippedRecord bit  ,
		ActionCode varchar(100),
		Reason varchar(100)
	)  

	CREATE INDEX EA_1 on #emails (Emailaddress)
	CREATE INDEX EA_2 on #emails (EmailID, SkippedRecord, duplicateRecord)


--,  CONSTRAINT PK PRIMARY KEY (tmpEmailID, Emailaddress)  
 
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlProfile  
  
  	--print ('Step 2 : Import to Temp = '  + convert(varchar,getdate(),109))
 
	-- IMPORT FROM XML TO TEMP TABLE
	insert into #emails 
	(
		Emailaddress, Title, FirstName, LastName, FullName, Company,  
		Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, 
		UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, 
		notes, [password],
		formattypecode, subscribetypecode,smsenabled, SkippedRecord
	)  
	SELECT 
		LTRIM(RTRIM(Emailaddress)), Title,  FirstName,  LastName,  FullName,  Company,  
		Occupation,  Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax, 
		Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, case WHEN ISDATE(Birthdate)=1 THEN Birthdate ELSE NULL END , 
		UserEvent1, case WHEN ISDATE(UserEvent1Date)=1 THEN UserEvent1Date ELSE NULL END , UserEvent2, case WHEN ISDATE(UserEvent2Date)=1 THEN UserEvent2Date ELSE NULL END ,  
		Notes, 
		case when ltrim(rtrim(isnull([Password],''))) = '' then NULL else ltrim(rtrim([Password])) end,
		case when isnull(formattypecode,'') not in ('html','text') then (case when len(rtrim(ltrim(@formattypecode))) = 0 then '' else @formattypecode end) else formattypecode end,  
		case when isnull(subscribetypecode,'') not in ('S','U','P','D','B','M') then '' else subscribetypecode end,
		--case when isnull(subscribetypecode,'') not in ('S','U','P','D','B') then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then '' else @subscribetypecode end) else subscribetypecode end,
		case when isnull(smsenabled,'') not in ('True','False') then ('True') else smsenabled end,
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
		formattypecode varchar(50) 'formattypecode', subscribetypecode varchar(50) 'subscribetypecode', smsenabled varchar(50) 'smsenabled'  
	) 
	
	insert into @result values ('T',@@ROWCOUNT)  
    declare @insertcount varchar(50)
    set @insertcount = (select COUNT(*) from #emails)
    
    UPDATE ImportEmailTracking SET EmailCount = (select COUNT(*) from #emails) WHERE TrackingID = @TrackingID
    
	--print ('Step 2 check : '  + @insertcount )
	
	EXEC sp_xml_removedocument @docHandle    

	--print ('Step 3 : VALIDATE EMAIL ADDRESS AND MARK AS SKIPERD FOR BAD [Emails] : '  + convert(varchar,getdate(),109))  

	Update #emails
	set SkippedRecord = 1, Reason='Skipped', ActionCode='S'
	where dbo.fn_ValidateEmailAddress(Emailaddress) = 0 or ISNULL(subscribetypecode,'') = 'M'

	insert into @result
	select 'S', @@ROWCOUNT

	--print ('Step 4 : Update Duplicates = '  + convert(varchar,getdate(),109))  
	
	--update duplicate records in temp tables  
	update #emails  
	set duplicateRecord = 1, Reason='DuplicateRecord', ActionCode='D'  
	from	#emails e join   
			(select max(el.tmpEmailID) as tmpEmailID, el.emailaddress from #emails el group by el.emailaddress) el1 
			on  e.emailaddress = el1.emailaddress  
	where e.tmpemailID <> el1.tmpEmailID and SkippedRecord = 0 
	  
	insert into @result values ('D',@@ROWCOUNT)
	
	
	-- sunil - swapped 5 & 6 - 6/16/2015
	--print ('Step 6 : Update EmailID '  + convert(varchar,getdate(),109))  

	update #emails  
	set EmailID = e.EmailID , EmailGroupID = eg.emailgroupID 
	from   
		#emails el with (NOLOCK) 
		JOIN [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress   
		JOIN EmailGroups eg with (NOLOCK) on e.EmailID = eg.EmailID 
	where  
		GroupID = @GroupID and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1 	
	
	--print ('Step 6 : Update EmailID '  + convert(varchar,getdate(),109))  
	
	update #emails  
	set EmailID = e.EmailID  
	from   
		#emails el with (NOLOCK) join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress   
	where  
		isnull(el.emailID,0) = 0 and customerID = @customerID and SkippedRecord = 0 and Isnull(el.duplicateRecord,0) <> 1 
		
	--print ('Step 5 : Update EmailID & EmailgroupID = '  + convert(varchar,getdate(),109))  

	--#2 Check if emailaddress already exists in the group , if yes, update EmailID and [EmailGroups] ID   
	--[added as Patch to fix the duplicate issue with KMPS [Emails] - multiple emailaddress exists and when updating payment OLD query picks the top emailaddress from different group]
	
	--commented by Sunil - 01/21/2016 - added new logic (step 5) to verify the email in group & update emailID and emailgroupID in Temp table
	--update #emails  
	--set EmailGroupID = eg.emailgroupID  
	--from   
	--	#emails el with (NOLOCK) join [EmailGroups] eg with (NOLOCK) on eg.EmailID = el.EmailID
	--where  
	--	eg.groupID = @groupID 
		

	--print ('Step 6.5 : VALIDATE EMAIL ADDRESS AND MARK AS SKIPERD MASTER SUPPRESSION : '  + convert(varchar,getdate(),109)) 
	
	declare @SuppGroupID int, @BaseChannelID int
	select @SuppGroupID = groupid from [Groups] WITH (NOLOCK) where CustomerID = @CustomerID and MasterSupression = 1
	select @BaseChannelID = BaseChannelID from [ECN5_ACCOUNTS].[DBO].Customer WITH (NOLOCK) where customerID = @CustomerID and IsDeleted = 0
	
	--from sp_importemails for JF 1/29/2016 to still insert if in suppression
	IF @insertMS = 1
	BEGIN
		--channel master suppression
		UPDATE 
			#emails  
		SET 
			 subscribetypecode = 'M'  
		FROM 
			#emails e 
			JOIN ChannelMasterSuppressionList cmsl WITH (NOLOCK) on e.Emailaddress = cmsl.EmailAddress
		WHERE 
			SkippedRecord = 0 
			AND cmsl.BaseChannelID = @BaseChannelID	
			AND ISNULL(cmsl.IsDeleted, 0) = 0  

		INSERT INTO @Result VALUES ('M',@@ROWCOUNT)

		--global master suppression
		UPDATE 
			#emails  
		SET 
			 subscribetypecode = 'M'  
		FROM 
			#emails e 
			join GlobalMasterSuppressionList gmsl WITH (NOLOCK) on e.Emailaddress = gmsl.EmailAddress
		WHERE 
			SkippedRecord = 0 and 
			gmsl.IsDeleted = 0	  

		INSERT INTO @Result VALUES ('M',@@ROWCOUNT)
	
		--master suppression group
		UPDATE 
			#emails  
		SET 
			 subscribetypecode = 'M'  
		FROM 
			#emails e 
			JOIN EmailGroups eg WITH (NOLOCK) on e.EmailID = eg.EmailID
		WHERE 
			SkippedRecord = 0 
			AND eg.GroupID = @SuppGroupID
	
		SET @skippedcount = @@ROWCOUNT
	
		IF NOT EXISTS (SELECT * FROM @Result WHERE Action = 'M')
			INSERT INTO @Result VALUES ('M',@skippedcount)
		else
			UPDATE @result SET Counts = Counts + @skippedcount WHERE Action = 'M'
	END
	--regular code which skips if in suppression
	ELSE
	BEGIN
		--channel master suppression
		update #emails  
		set SkippedRecord = 1  , Reason='MasterSuppressed'  , ActionCode='M'
		from #emails e 
			join ChannelMasterSuppressionList cmsl WITH (NOLOCK) on e.Emailaddress = cmsl.EmailAddress
		where SkippedRecord = 0 and cmsl.BaseChannelID = @BaseChannelID and cmsl.IsDeleted = 0

		SELECT @skippedcount = @@ROWCOUNT
		if not exists (select * from @result where Action = 'M')
			insert into @result values ('M',@skippedcount)
		else
			update @result set Counts = Counts + @skippedcount where Action = 'M'

		--global master suppression
		update #emails  
		set SkippedRecord = 1  , Reason='MasterSuppressed'  , ActionCode='M'
		from #emails e 
			join GlobalMasterSuppressionList gmsl WITH (NOLOCK) on e.Emailaddress = gmsl.EmailAddress
		where SkippedRecord = 0 and gmsl.IsDeleted = 0

		SELECT @skippedcount = @@ROWCOUNT
		if not exists (select * from @result where Action = 'M')
			insert into @result values ('M',@skippedcount)
		else
			update @result set Counts = Counts + @skippedcount where Action = 'M'

		--master suppression group
		update #emails  
		set SkippedRecord = 1  , Reason='MasterSuppressed'  , ActionCode='M'
		from #emails e 
			join [EmailGroups] eg WITH (NOLOCK) on e.EmailID = eg.EmailID
		where SkippedRecord = 0 and eg.GroupID = @SuppGroupID
	
		SELECT @skippedcount = @@ROWCOUNT	
		if not exists (select * from @result where Action = 'M')
			insert into @result values ('M',@skippedcount)
		else
			update @result set Counts = Counts + @skippedcount where Action = 'M'

		--domain suppression
		update #emails  
		set SkippedRecord = 1  , Reason='MasterSuppressed'  , ActionCode='M'
		from #emails e 
			join DomainSuppression dms WITH (NOLOCK) on e.Emailaddress like +'%@'+ dms.Domain
		where SkippedRecord = 0 and (dms.BaseChannelID = @BaseChannelID or dms.CustomerID = @CustomerID) and dms.IsDeleted = 0

		SELECT @skippedcount = @@ROWCOUNT
		if not exists (select * from @result where Action = 'M')
			insert into @result values ('M',@skippedcount)
		else
			update @result set Counts = Counts + @skippedcount where Action = 'M'

	END
	--end


	

	if @EmailAddressOnly = 0
	Begin
		--print ('Step 7 : Update #emails = '  + convert(varchar,getdate(),109))  

		-- if emailID is not null - update [Emails] and insert/update in [EmailGroups] table  
		SET ROWCOUNT 0 
		DECLARE @I INT
		DECLARE @MinTempId INT
		DECLARE @MaxTempId INT
		DECLARE @Rowcount INT

		SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #emails)
		SET @Rowcount = 1000
		SET @I =0

		IF EXISTS (	SELECT 1
					FROM 	#emails el
						INNER JOIN [Emails] e WITH (NOLOCK) ON el.emailID = e.emailID 
					WHERE
						ISNULL(DuplicateRecord,0) != 1  
						AND SkippedRecord = 0
				)
		SET @I = (	SELECT COUNT(*) FROM 	#emails el)
		SET ROWCOUNT @Rowcount
		SET @MaxTempId = @MinTempId + @Rowcount -1

		WHILE @I > 0
		BEGIN 
			UPDATE
				[Emails]   
			SET  
				Title = isnull(el.title, e.Title),  
				FirstName = isnull(el.FirstName, e.FirstName),  
				LastName = isnull(el.LastName, e.LastName),  
				FullName = isnull(el.FullName, e.FullName),  
				Company = isnull(el.Company, e.Company),  
				Occupation = isnull(el.Occupation, e.Occupation),  
				Address = isnull(el.Address, e.Address),  
				Address2 = isnull(el.Address2, e.Address2),  
				City = isnull(el.City, e.City),  
				State = isnull(el.State, e.State),  
				Zip = isnull(el.Zip, e.Zip),  
				Country = isnull(el.Country, e.Country),  
				Voice = isnull(el.Voice, e.Voice),  
				Mobile = isnull(el.Mobile, e.Mobile),  
				Fax = isnull(el.Fax, e.Fax),  
				Website = isnull(el.Website, e.Website),  
				Age = isnull(el.Age, e.Age),  
				Income = isnull(el.Income, e.Income),  
				Gender = isnull(el.Gender, e.Gender),  
				User1 = isnull(el.User1, e.User1),  
				User2 = isnull(el.User2, e.User2),  
				User3 = isnull(el.User3, e.User3),  
				User4 = isnull(el.User4, e.User4),  
				User5 = isnull(el.User5, e.User5),  
				User6 = isnull(el.User6, e.User6),  
				Birthdate = isnull(el.Birthdate, e.Birthdate),  
				UserEvent1 = isnull(el.UserEvent1, e.UserEvent1),  
				UserEvent1Date = isnull(el.UserEvent1Date, e.UserEvent1Date),  
				UserEvent2 = isnull(el.UserEvent2, e.UserEvent2),  
				UserEvent2Date = isnull(el.UserEvent2Date, e.UserEvent2Date),  
				notes = isnull(el.notes, e.notes),  
				password = isnull(el.password, e.password),
				DateUpdated = @dt
			--OUTPUT
			--	Deleted.EmailID
			--	Inserted.EmailID
			FROM   
				#emails el WITH (NOLOCK) 
				join [Emails] e WITH (NOLOCK) ON el.emailID = e.emailID 
			WHERE  
				ISNULL(DuplicateRecord,0) != 1  
				AND SkippedRecord = 0 
				AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId

			SET @i = @i - @Rowcount
			SET @MinTempId = @MinTempId + @Rowcount 
			SET @MaxTempId = @MinTempId + @Rowcount -1
		END
		
		SET ROWCOUNT 0
	End
	 
	--print ('Step 8 : Insert into [Emails]  = '  + convert(varchar,getdate(),109))  
 	SET ROWCOUNT 0 
	--DECLARE @I INT
	--DECLARE @MinTempId INT
	--DECLARE @MaxTempId INT
	--DECLARE @Rowcount INT

	SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #emails)
	SET @Rowcount = 1000
	SET @I =0
	
	IF EXISTS (	SELECT 1 
				FROM 
					#emails 
				WHERE
					EmailID is null 
					AND SkippedRecord = 0 
					AND Isnull(duplicateRecord,0) <> 1  )
	SET @I = (	SELECT COUNT(*)	FROM #emails )
	SET ROWCOUNT @Rowcount
	SET @MaxTempId = @MinTempId + @Rowcount -1

	WHILE @I > 0
	BEGIN 
	-- if emailID is null #emails table - insert into [Emails] and [EmailGroups]  

		INSERT INTO [Emails] 
			(Emailaddress, CustomerID, Title, FirstName, LastName, FullName, Company,  
			Occupation, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax,  
			Website, Age, Income, Gender, User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, notes, password)  
		SELECT 
			el.EmailAddress, @CustomerID, el.Title, el.FirstName, el.LastName, el.FullName, el.Company,  
			el.Occupation, el.Address, el.Address2, el.City, el.State, el.Zip, el.Country, el.Voice, el.Mobile, el.Fax,  
			el.Website, el.Age, el.Income, el.Gender, el.User1, el.User2, el.User3, el.User4, el.User5, el.User6,   
			el.Birthdate, el.UserEvent1, el.UserEvent1Date, el.UserEvent2, el.UserEvent2Date,el.notes,el.password
		FROM 
			#emails el 
		WHERE
			EmailID is null 
			AND SkippedRecord = 0 
			AND Isnull(el.duplicateRecord,0) <> 1  
			AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId

		SET @i = @i - @Rowcount
		SET @MinTempId = @MinTempId + @Rowcount 
		SET @MaxTempId = @MinTempId + @Rowcount -1
	END
	SET ROWCOUNT 0

	--print ('Step 9 : Update EmailID again for New [Emails]  = '  + convert(varchar,getdate(),109))  
	  
	-- update EmailID and [EmailGroups] ID  
	update #emails  
	set EmailID = e.EmailID  
	from   
		#emails el with (NOLOCK) 
		join [Emails] e with (NOLOCK) on el.emailaddress = e.emailaddress   
	where  
		customerID = @customerID 
		and SkippedRecord = 0 
		and el.emailID is null 
		--and Isnull(el.duplicateRecord,0) <> 1  
	 
	--print ('Step 10 : Update EmailGroupID = '  + convert(varchar,getdate(),109))  

	-- UPDATE @emails WITH EMAILGROUPID

	update 
		#emails  
	set 
		EmailGroupID = eg.emailgroupID , 
		Reason='UpdatedRecord'  , 
		ActionCode='U'
	from   
		#emails el with (NOLOCK) 
		join [EmailGroups] eg with (NOLOCK) on el.emailID = eg.emailID   
	where 
		Isnull(el.duplicateRecord,0) <> 1 
		and SkippedRecord = 0 
		and eg.groupID = @groupID

	insert into @result values ('U',@@ROWCOUNT) 

	--print ('Step 11 : Update [EmailGroups] = '  + convert(varchar,getdate(),109))  
	
	--declare @subcode varchar(20)
	--select @subcode = subscribetypecode from #emails
	--print (@subcode)

 	SET ROWCOUNT 0 
	--DECLARE @I INT
	--DECLARE @MinTempId INT
	--DECLARE @MaxTempId INT
	--DECLARE @Rowcount INT

	SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #emails)
	SET @Rowcount = 1000
	SET @I =0	

	IF EXISTS (SELECT 1 
				FROM 
					#emails e
					join [EmailGroups] eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID 
				WHERE 
					Isnull(duplicateRecord,0) <> 1 
					AND SkippedRecord = 0 
					AND upper(eg.subscribetypecode) <> 'M'
			)

	SET @I = (	SELECT COUNT(*) FROM #emails e)

	SET ROWCOUNT @Rowcount
	SET @MaxTempId = @MinTempId + @Rowcount -1

	WHILE @I > 0
	BEGIN 
		UPDATE  
			[EmailGroups] 
		SET	
			FormatTypeCode = case when isnull(e.formattypecode,'') = '' then eg.formattypecode else lower(e.formattypecode) end,   
			SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) > 0 and eg.SubscribeTypeCode <> 'U' then upper(@subscribetypecode) else eg.subscribetypecode end) when isnull(e.subscribetypecode,'') = 'P' and eg.SubscribeTypeCode = 'S' then 'S' else upper(e.subscribetypecode)  end,	
			--SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then eg.subscribetypecode else upper(@subscribetypecode) end) else upper(e.subscribetypecode) end, 
			SMSEnabled=  case when isnull(e.smsenabled,'') = '' then eg.smsenabled else e.smsenabled end,
			LastChanged = @dt, LastChangedSource = @source
		FROM 
			#emails e with (NOLOCK) 
			join [EmailGroups] eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID 
		WHERE 
			Isnull(duplicateRecord,0) <> 1 
			AND SkippedRecord = 0 
			--AND upper(eg.subscribetypecode) <> 'M'
			AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId

		SET @i = @i - @Rowcount
		SET @MinTempId = @MinTempId + @Rowcount 
		SET @MaxTempId = @MinTempId + @Rowcount -1
	END
	SET ROWCOUNT 0
 
	--print ('Step 12 : Insert into [EmailGroups] = '  + convert(varchar,getdate(),109))  
	--DECLARE @I INT
	--DECLARE @MinTempId INT
	--DECLARE @MaxTempId INT
	--DECLARE @Rowcount INT

	DECLARE @EmailGroupInsertCount INT
	SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #emails)
	SET @Rowcount = 1000
	SET @I =0 
	
	IF EXISTS (	SELECT 1 
				FROM 
					#emails
				WHERE 
					emailgroupID IS NULL 
					AND Isnull(duplicateRecord,0) <> 1 
					AND SkippedRecord = 0 
			  )
	SET @I = (	SELECT COUNT(*) FROM #emails)
	SET ROWCOUNT @Rowcount
	SET @MaxTempId = @MinTempId + @Rowcount -1
	SET @EmailGroupInsertCount = 0

	WHILE @I > 0
	BEGIN 

		--INSERT INTO [EmailGroups]
		insert into [EmailGroups] 
			(EmailID, GroupID, FormatTypeCode, SubscribeTypeCode,SMSEnabled,CreatedOn,CreatedSource)  
		select
			EmailID, @groupID,   
			case when isnull(formattypecode,'') = '' then 'html' else lower(formattypecode) end,   
			case when len(rtrim(ltrim(subscribetypecode))) > 0 then subscribetypecode else (case when len(rtrim(ltrim(@subscribetypecode))) > 0 then upper(@subscribetypecode) else 'S' end) end,
			case when isnull(smsenabled,'') = '' then 'True' else smsenabled end,   
			@dt, @source
		from 
			#emails   
		where 
			emailgroupID IS NULL 
			AND Isnull(duplicateRecord,0) <> 1 
			AND SkippedRecord = 0 
			AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId

		SET @EmailGroupInsertCount = @EmailGroupInsertCount + @@ROWCOUNT
		SET @i = @i - @Rowcount
		SET @MinTempId = @MinTempId + @Rowcount 
		SET @MaxTempId = @MinTempId + @Rowcount -1
	END
	SET ROWCOUNT 0

	INSERT INTO @result VALUES ('I',@EmailGroupInsertCount)	

	if @filename <>''
	begin
		insert into ImportEmailsLog(
			Emailaddress, 
			GroupID,
			ActionCode, 
			[FileName], 
			Reason,
			Source)
		select  
			Emailaddress,
			@GroupID, 
			ActionCode, 
			@filename, 
			Reason,
			@source 
		from 
			#emails
			
		update ImportEmailsLog 
		set 
			ActionCode='I', 
			Reason='' 
		where 
			[FileName]=@filename 
			and ActionCode is null
	end
	
	  
	/************************************************  
	[EMAILDATAVALUES] - UDF FIELD updates  
	************************************************/  
	
	if exists(select top 1 GroupDatafieldsID from GroupDatafields WITH (NOLOCK) where GroupID = @GroupID and IsDeleted = 0)
	Begin
	
		create TABLE #UDF 
		(  
			tmpEmailID INT IDENTITY(1,1),
			EmailID int,    
			Emailaddress varchar(255),   
			GroupDataFieldsID int ,   
			Datavalue varchar(500),   
			DatafieldSetID int,  
			EntryID uniqueIdentifier,  
			RowID int,
			edvID BIGINT,--int  
			SurveyGridID int
		)  
		
		CREATE INDEX UDF_1 on #UDF (EmailID, GroupDataFieldsID)
		CREATE INDEX UDF_2 on #UDF (EmailID, GroupDataFieldsID, entryID)
		CREATE INDEX UDF_3 on #UDF (entryID)
		CREATE INDEX UDF_4 on #UDF (Emailaddress)
	
		--print ('Step 2 : Create Temp UDF = '  + convert(varchar,getdate(),109))  
		
		select @tUDF_Exists = case when DatafieldSetID > 0 then 1 else 0 end from GroupDatafields WITH (NOLOCK) where GroupID = @GroupID and IsDeleted = 0 and isnull(DatafieldSetID,0) > 0
		  
		EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlUDF  
		  
		insert into #UDF (Emailaddress, GroupDataFieldsID, Datavalue, EntryID, DataFieldsetID, RowID, edvID, SurveyGridID)  		
		select t.Emailaddress, t.GroupDataFieldsid, t.datavalue, t.EntryID, t.DataFieldsetID, t.RowID, t.edvID,t.SurveyGridID from
		(
			 SELECT o.Emailaddress, o.GroupDataFieldsid, o.datavalue, NULL as EntryID, g.DataFieldsetID, RowID, 0 edvID, o.SurveyGridID, ROW_NUMBER() over (partition by Emailaddress, o.GroupdatafieldsID order by RowID desc) as eOrder  FROM   
			 OPENXML(@docHandle, N'//row/udf')   
			 WITH   
			 (  
			   Emailaddress varchar(255) '../ea',  
			   GroupDataFieldsid int '@id',  
			   Datavalue varchar(500) 'v',  
			   Entrysid varchar(50) 'g',  
			   SurveyGridID int 'sgid',
			   RowID int '@mp:parentid'  
			 ) o join Groupdatafields g on g.GroupDataFieldsid = o.GroupDataFieldsid and g.IsDeleted = 0 and isnull(g.DatafieldSetID,0) = 0 and g.GroupID	= @GroupID
		) t  
		where t.eOrder = 1
		union
		SELECT o.Emailaddress, o.GroupDataFieldsid, o.datavalue, NULL as EntryID, g.DataFieldsetID, RowID, 0 edvID, o.SurveyGridID  FROM   
			 OPENXML(@docHandle, N'//row/udf')   
			 WITH   
			 (  
			   Emailaddress varchar(255) '../ea',  
			   GroupDataFieldsid int '@id',  
			   Datavalue varchar(500) 'v',  
			   Entrysid varchar(50) 'g',  
			   SurveyGridID int 'sgid',
			   RowID int '@mp:parentid'  
			 ) o join Groupdatafields g on g.GroupDataFieldsid = o.GroupDataFieldsid and g.IsDeleted = 0 and isnull(g.DatafieldSetID,0) > 0 and g.GroupID = @GroupID

		  
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
			insert into #UDF (Emailaddress, GroupDataFieldsID, Datavalue, EntryID, DataFieldsetID, RowID, edvID, SurveyGridID)  
			select distinct udf.Emailaddress, gdf2.GroupDatafieldsID, dbo.fn_GetGDFDValue(gdfd.DataValue, gdfd.SystemValue),null,gdf2.DatafieldSetID,udf.RowID, 0,null
			from #UDF udf
			join GroupDatafields gdf2 with(nolock) on gdf2.GroupID = @GroupID 
			join GroupDataFieldsDefault gdfd with(nolock) on gdf2.GroupDatafieldsID = gdfd.GDFID
			where gdfd.GDFID is not null 
				  and  gdf2.GroupDatafieldsID not in (Select GroupDataFieldsID from #UDF where LEN(ISNULL(DataValue,'')) > 0) 
				  and  gdf2.DatafieldSetID is not null 
				  and  gdf2.IsDeleted = 0
				  and gdf2.DatafieldSetID in (select distinct DataFieldSetID from #UDF)
				  and ISNULL(udf.Datavalue,'') <> ''
				

			update #UDF  
			set EmailID = e.EmailID  
			from   
				#UDF u join 
				[Emails] e  with (NOLOCK) on u.emailaddress = e.emailaddress and customerID = @customerID
				 --join  [EmailGroups] eg with (NOLOCK)  on e.emailID = eg.emailID and eg.groupID = @groupID
				
			------------------------------------------------------
			------------------ Transactional UDFs ------------------
			------------------------------------------------------
			
			if @tUDF_Exists = 1 -- IF TRANSACTION udf EXISTS
			Begin

				select @primaryKeyID = GroupDatafieldsID from GroupDatafields WITH (NOLOCK) where GroupID = @GroupID and DatafieldSetID > 0 and IsDeleted = 0 and IsPrimaryKey = 1
				
				if @primaryKeyID > 0 -- IF PRIMARY KEY EXISTS
				Begin
					insert into @entryID 
					select RowID, DatafieldSetID, edv.EntryID  
					from #UDF u join [EMAILDATAVALUES] edv WITH (NOLOCK)
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
				BEGIN
					UPDATE 
						#UDF
					SET 
						edvID = e.EmailDataValuesID
					FROM   
						#UDF u  with (NOLOCK)
						join [EMAILDATAVALUES] e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID and e.entryID = u.entryID
					WHERE 
						u.EmailID is not null 
						and u.EntryID is not null


--**--
					--DECLARE @I INT
					--DECLARE @MinTempId INT
					--DECLARE @MaxTempId INT
					--DECLARE @Rowcount INT

					SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #UDF)
					SET @Rowcount = 1000
					SET @I = (SELECT COUNT(*) FROM #UDF)
					SET ROWCOUNT @Rowcount
					SET @MaxTempId = @MinTempId + @Rowcount -1

					WHILE @I > 0
					BEGIN 
					
						UPDATE 
							EmailDataValues
						set	
							DataValue = u.Datavalue, 
							ModifiedDate = @dt, 
							UpdatedUserID = @UserID
						from   
							EmailDataValues e WITH (NOLOCK) 
							join #UDF u  on e.EmailDataValuesID = u.edvID
						where 
							u.edvID > 0
							AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId

						SET @i = @i - @Rowcount
						SET @MinTempId = @MinTempId + @Rowcount 
						SET @MaxTempId = @MinTempId + @Rowcount -1
					END

					SET ROWCOUNT 0
				End	

				
--**--
				--DECLARE @I INT
				--DECLARE @MinTempId INT
				--DECLARE @MaxTempId INT
				--DECLARE @Rowcount INT

				SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #UDF)
				SET @Rowcount = 1000
				SET @I = (SELECT COUNT(*) FROM #UDF)
				SET ROWCOUNT @Rowcount
				SET @MaxTempId = @MinTempId + @Rowcount -1

				WHILE @I > 0
				BEGIN 

					-- New UDF Records  - Transactions  
					Insert into [EMAILDATAVALUES] (EmailID, GroupDatafieldsID, DataValue, CreatedDate, EntryID, CreatedUserID)  
					select EmailID, GroupDatafieldsID, DataValue, @dt, EntryID, @UserID 
					from  #UDF 
					where 
						EmailID is not null 
						and  EntryID is not null 
						and Isnull(DataValue,'') <> ''  
						and edvID = 0
							AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId

						SET @i = @i - @Rowcount
						SET @MinTempId = @MinTempId + @Rowcount 
						SET @MaxTempId = @MinTempId + @Rowcount -1
				END
				SET ROWCOUNT 0
			End
			
			------------------------------------------------------
			------------------ stand alone UDFs ------------------
			------------------------------------------------------
		  
			update #UDF
				set edvID = e.EmailDataValuesID
			from   
				#UDF u  with (NOLOCK)  
				join [EMAILDATAVALUES] e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID
			where 
				u.EmailID is not null 
				and u.EntryID is null 
				and u.SurveyGridID is null
						
		  
			-- Existing UDF Records   
--**--
				--DECLARE @I INT
				--DECLARE @MinTempId INT
				--DECLARE @MaxTempId INT
				--DECLARE @Rowcount INT

				SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #UDF)
				SET @Rowcount = 1000
				SET @I = (SELECT COUNT(*) FROM #UDF)
				SET ROWCOUNT @Rowcount
				SET @MaxTempId = @MinTempId + @Rowcount -1

				WHILE @I > 0
				BEGIN 

					update 
						[EMAILDATAVALUES]  
					set 
						DataValue = u.datavalue,  
						ModifiedDate = @dt,
						UpdatedUserID = @UserID 
					from   
						[EMAILDATAVALUES] e  
						with (NOLOCK) 
						join #UDF u with (NOLOCK) on e.EmailDataValuesID = u.edvID 
					where   
						u.EntryID is null
						AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId

					SET @i = @i - @Rowcount
					SET @MinTempId = @MinTempId + @Rowcount 
					SET @MaxTempId = @MinTempId + @Rowcount -1
				END
				SET ROWCOUNT 0

		
		  
			-- New UDF Records    
--**--
				--DECLARE @I INT
				--DECLARE @MinTempId INT
				--DECLARE @MaxTempId INT
				--DECLARE @Rowcount INT

				SET @MinTempId = (SELECT MIN(tmpEmailID) FROM #UDF)
				SET @Rowcount = 1000
				SET @I = (SELECT COUNT(*) FROM #UDF)
				SET ROWCOUNT @Rowcount
				SET @MaxTempId = @MinTempId + @Rowcount -1

				WHILE @I > 0
				BEGIN 

					Insert into [EMAILDATAVALUES] (
						EmailID, 
						GroupDatafieldsID, 
						DataValue, 
						CreatedDate, 
						EntryID, 
						CreatedUserID, 
						SurveyGridID)  
					select 
						u.EmailID, 
						u.GroupDatafieldsID, 
						MAX(u.DataValue), 
						@dt, 
						NULL, 
						@UserID, 
						u.SurveyGridID
					from	
						#UDF u  with (NOLOCK) 
					where   ((
								u.edvID = 0 and  --u.edvID is null and 
								u.EmailID is not null and 
								u.EntryID is null and 
								Isnull(u.DataValue,'') <> ''  
							)
							or
							(
								--for surveys, grid responses
								u.edvID = 0 and  --u.edvID is null and 
								u.EmailID is not null and 
								u.EntryID is null and
								u.SurveyGridID is not null
							))
							AND tmpEmailID BETWEEN @MinTempId AND @MaxTempId
					group by 
						u.EmailID, 
						u.GroupDatafieldsID,
						u.SurveyGridID
					SET @i = @i - @Rowcount
					SET @MinTempId = @MinTempId + @Rowcount 
					SET @MaxTempId = @MinTempId + @Rowcount -1
				END
			SET ROWCOUNT 0

			
		END  
		DROP TABLE #UDF
	END
	
	DROP TABLE #emails
	
	UPDATE ImportEmailTracking 
	SET EndTime = GETDATE() 
	WHERE TrackingID = @TrackingID
	
	SELECT * FROM @Result  

END