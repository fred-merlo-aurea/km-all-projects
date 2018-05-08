------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
-- 2014-10-24 MK Added  WITH (NOLOCK) hints
-- 2014-11-25 MK Added code to log XML for single row imports
--
------------------------------------------------------

CREATE proc [dbo].[sp_importEmails]
(  
 @CustomerID int,  
 @GroupID int,  
 @xmlProfile text,  
 @xmlUDF text,
 @formattypecode varchar(50),
 @subscribetypecode varchar(50),
 @EmailAddressOnly bit
)  
AS  
BEGIN  
	SET NOCOUNT ON  	

	DECLARE @docHandle int,
			@dt datetime,
			@primaryKeyID int,
			@tUDF_Exists bit,
			@skippedcount int,
			@TrackingID int
	
	SET @primaryKeyID = 0
	SET @tUDF_Exists = 0
	SET @dt = getdate()
	SET @skippedcount = 0
	
	SET @TrackingID = 0

	INSERT INTO ImportEmailTracking (
		CustomerID, 
		GroupID, 
		StartTime) 
	VALUES (
		@CustomerID, 
		@GroupID, 
		GETDATE())

	SELECT @TrackingID = @@IDENTITY

	DECLARE @result TABLE  (Action varchar(100), Counts int)  

	DECLARE @entryID TABLE (RowID int, DatafieldSetID int, entryID uniqueidentifier)  
  
  	--print ('Step 1 : Create Temp Emails and Index = '  + convert(varchar,getdate(),109))

	CREATE TABLE #emails   
	(  
		tmpEmailID int IDENTITY(1,1) ,
		EmailID int,  
		EmailGroupID int, 
		Emailaddress varchar(255), 
		Title varchar(50), 
		FirstName varchar(50), 
		LastName varchar(50),  
		FullName varchar(50), 
		Company varchar(100), 
		Occupation varchar(50), 
		Address varchar(255), 
		Address2 varchar(255), 
		City varchar(50),  
		State varchar(50), 
		Zip varchar(50), 
		Country varchar(50), 
		Voice varchar(50), 
		Mobile varchar(50), 
		Fax varchar(50), 
		Website varchar(50),  
		Age varchar(50), 
		Income varchar(50), 
		Gender varchar(50),   
		User1 varchar(255), 
		User2 varchar(255), 
		User3 varchar(255), 
		User4 varchar(255), 
		User5 varchar(255), 
		User6 varchar(255), 
		Birthdate datetime,  
		UserEvent1 varchar(50),
		UserEvent1Date datetime, 
		UserEvent2 varchar(50), 
		UserEvent2Date datetime,  
		notes varchar(1000), 
		[password] varchar(25),
		formattypecode varchar(50),  
		subscribetypecode  varchar(1),  
		duplicateRecord bit,
		SkippedRecord bit  
	)  

	CREATE INDEX EA_1 on #emails (Emailaddress)
	CREATE INDEX EA_2 on #emails (EmailID, SkippedRecord, duplicateRecord)


--,  CONSTRAINT PK PRIMARY KEY (tmpEmailID, Emailaddress)  
 
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlProfile  
  
  	--print ('Step 2 : Import to Temp = '  + convert(varchar,getdate(),109))
 
	-- IMPORT FROM XML TO TEMP TABLE
	INSERT INTO #emails 
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
		Notes, 
		case when ltrim(rtrim(isnull([Password],''))) = '' then NULL else ltrim(rtrim([Password])) end,
		case when isnull(formattypecode,'') not in ('html','text') then (case when len(rtrim(ltrim(@formattypecode))) = 0 then '' else @formattypecode end) else formattypecode end,  
		case when isnull(subscribetypecode,'') not in ('S','U','P','D','B') then '' else subscribetypecode end,
		--case when isnull(subscribetypecode,'') not in ('S','U','P','D','B') then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then '' else @subscribetypecode end) else subscribetypecode end,
		case when isnull(Emailaddress,'') = '' then 1 else 0 end
	FROM
		OPENXML(@docHandle, N'/XML/Emails')   
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
	
	INSERT INTO @Result VALUES ('T',@@ROWCOUNT)  
    DECLARE @insertcount varchar(50)
    SET @insertcount = (select COUNT(*) from #emails)
    
    IF @insertcount = 1
    BEGIN
		UPDATE 
			ImportEmailTracking 
		SET 
			EmailCount = (select COUNT(*) from #emails) ,
			XMLProfile = @xmlProfile,
			XMLUDF = @xmlUDF
		WHERE 
			TrackingID = @TrackingID
	END
	
	ELSE
	
	BEGIN
		UPDATE 
			ImportEmailTracking 
		SET 
			EmailCount = (select COUNT(*) from #emails) 
		WHERE 
			TrackingID = @TrackingID
	END
	    
	--print ('Step 2 check : '  + @insertcount )
	
	EXEC sp_xml_removedocument @docHandle    

	--print ('Step 3 : VALIDATE EMAIL ADDRESS AND MARK AS SKIPERD FOR BAD EMAILS : '  + convert(varchar,getdate(),109))  

	UPDATE 
		#Emails
	SET 
		SkippedRecord = 1
	WHERE
		dbo.fn_ValidateEmailAddress(Emailaddress) = 0

	--Update #emails
	--SET SkippedRecord = 1
	--where 
	--	NOT  (  
	--			LEN(ltrim(rtrim(Emailaddress))) <> 0 AND 
	--			CHARINDEX ( ' ',Emailaddress) = 0  AND  -- No embedded spaces
	--			CHARINDEX ( '''',Emailaddress) = 0  AND  -- No single quotes  
	--			CHARINDEX ( ',',Emailaddress) = 0  AND  -- No commas
	--			CHARINDEX ( '>',Emailaddress) = 0  AND  -- No greater than
	--			CHARINDEX ( '<',Emailaddress) = 0  AND  -- No less than
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

	INSERT INTO @result
	SELECT 'S', @@ROWCOUNT

	--print ('Step 4 : Update Duplicates = '  + convert(varchar,getdate(),109))  
	
	--update duplicate records in temp tables  
	UPDATE 
		#emails  
	SET 
		duplicateRecord = 1  
	FROM	
		#emails e 
		JOIN   
			(SELECT 
				MAX(el.tmpEmailID) as tmpEmailID, 
				el.emailaddress 
			FROM 
				#emails el 
			GROUP BY 
				el.emailaddress) el1 on  e.emailaddress = el1.emailaddress  
	WHERE 
		e.tmpemailID <> el1.tmpEmailID 
		and SkippedRecord = 0 
	  
	INSERT INTO @Result VALUES ('D',@@ROWCOUNT)
	
	--print ('Step 5 : Update EmailID & EmailgroupID = '  + convert(varchar,getdate(),109))  

	--#2 Check if emailaddress already exists in the group , if yes, update EmailID and EmailGroup ID   
	--[added as Patch to fix the duplicate issue with KMPS emails - multiple emailaddress exists and when updating payment OLD query picks the top emailaddress from different group]
	
	UPDATE 
		#emails  
	SET	
		EmailID = e.EmailID, 
		EmailGroupID = eg.emailgroupID  
	FROM   
		#emails el with (NOLOCK) 
		JOIN Emails e with (NOLOCK) on el.emailaddress = e.emailaddress  
		JOIN EmailGroups eg with (NOLOCK) on eg.EmailID = e.EmailID
	WHERE  
		customerID = @customerID 
		AND eg.groupID = @groupID 
		AND SkippedRecord = 0 
		AND el.emailID IS NULL 
		--and Isnull(el.duplicateRecord,0) <> 1  
	
	--print ('Step 6 : Update EmailID '  + convert(varchar,getdate(),109))  

	UPDATE 
		#emails  
	SET 
		EmailID = e.EmailID  
	FROM   
		#emails el with (NOLOCK) 
		join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress   
	WHERE  
		customerID = @customerID 
		AND SkippedRecord = 0 
		AND ISNULL(el.duplicateRecord,0) <> 1 
		AND el.emailID IS NULL

	--print ('Step 6.5 : VALIDATE EMAIL ADDRESS AND MARK AS SKIPERD MASTER SUPPRESSION : '  + convert(varchar,getdate(),109)) 
	
	declare @SuppGroupID int, @BaseChannelID int
	select @SuppGroupID = groupid from Groups WITH (NOLOCK) where CustomerID = @CustomerID and MasterSupression = 1
	select @BaseChannelID = BaseChannelID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER] WITH (NOLOCK) where customerID = @CustomerID
	
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

	if @EmailAddressOnly = 0
	BEGIN
		--print ('Step 7 : Update #Emails = '  + convert(varchar,getdate(),109))  

		-- if emailID is not null - update emails and insert/update in emailgroups table  
		UPDATE 
			emails   
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
			DateUpdated = @dt,
			notes = isnull(el.notes, e.notes),  
			password = isnull(el.password, e.password)
		FROM   
			#emails el with (NOLOCK) 
			join emails e with (NOLOCK) on el.emailID = e.emailID --and customerID = @customerID   
		WHERE  
			el.emailID is not null and Isnull(duplicateRecord,0) <> 1  and SkippedRecord = 0
 	END
	 
	--print ('Step 8 : Insert into Emails  = '  + convert(varchar,getdate(),109))  

	-- if emailID is null #emails table - insert into emails and emailgroups  
	INSERT INTO Emails (
		Emailaddress, 
		CustomerID, 
		Title, 
		FirstName, 
		LastName, 
		FullName, 
		Company,  
		Occupation, 
		Address, 
		Address2, 
		City, 
		State, 
		Zip, 
		Country, 
		Voice, 
		Mobile, 
		Fax,  
		Website, 
		Age, 
		Income, 
		Gender, 
		User1, 
		User2, 
		User3, 
		User4, 
		User5, 
		User6, 
		Birthdate, 
		UserEvent1, 
		UserEvent1Date, 
		UserEvent2, 
		UserEvent2Date, 
		notes, 
		password, 
		DateAdded)  
	SELECT 
		el.EmailAddress, 
		@CustomerID, 
		el.Title, 
		el.FirstName, 
		el.LastName, 
		el.FullName, 
		el.Company,  
		el.Occupation, 
		el.Address, 
		el.Address2, 
		el.City, 
		el.State, 
		el.Zip, 
		el.Country, 
		el.Voice, 
		el.Mobile, 
		el.Fax,  
		el.Website, 
		el.Age, 
		el.Income, 
		el.Gender, 
		el.User1, 
		el.User2, 
		el.User3, 
		el.User4, 
		el.User5, 
		el.User6,   
		el.Birthdate, 
		el.UserEvent1, 
		el.UserEvent1Date, 
		el.UserEvent2, 
		el.UserEvent2Date,
		el.notes,
		el.password, 
		@dt  
	FROM 
		#emails el 
	WHERE 
		emailID IS NULL 
		AND SkippedRecord = 0 
		AND ISNULL(el.duplicateRecord,0) <> 1  

	--print ('Step 9 : Update EmailID again for New Emails  = '  + convert(varchar,getdate(),109))  
	  
	-- update EmailID and EmailGroup ID  
	UPDATE 
		#emails  
	SET 
		EmailID = e.EmailID  
	FROM   
		#emails el with (NOLOCK) 
		join Emails e with (NOLOCK) on el.emailaddress = e.emailaddress   
	WHERE  
		customerID = @customerID 
		AND SkippedRecord = 0 
		AND el.emailID IS NULL 
		--and Isnull(el.duplicateRecord,0) <> 1  
	 
	--print ('Step 10 : Update EmailGroupID = '  + convert(varchar,getdate(),109))  

	-- UPDATE @EMAIL WITH EMAILGROUPID
	UPDATE 
		#emails  
	SET 
		EmailGroupID = eg.emailgroupID  
	FROM   
		#emails el with (NOLOCK) 
		JOIN emailgroups eg with (NOLOCK) on el.emailID = eg.emailID   
	WHERE 
		ISNULL(el.duplicateRecord,0) <> 1 
		AND SkippedRecord = 0 
		AND eg.groupID = @groupID 

	insert into @result values ('U',@@ROWCOUNT) 

	--print ('Step 11 : Update EmailGroups = '  + convert(varchar,getdate(),109))  
	
	--UPDATE [EmailGroups]
	UPDATE  
		[EmailGroups] 
	SET	
		FormatTypeCode = case when isnull(e.formattypecode,'') = '' then eg.formattypecode else lower(e.formattypecode) end,   
		SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) > 0 and eg.SubscribeTypeCode <> 'U' then upper(@subscribetypecode) else eg.subscribetypecode end) else upper(e.subscribetypecode) end,			
		--SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then ( case when len(rtrim(ltrim(@subscribetypecode))) = 0 then eg.subscribetypecode else upper(@subscribetypecode) end) else upper(e.subscribetypecode) end,
		LastChanged = @dt
	FROM 
		#emails e with (NOLOCK) 
		JOIN [EmailGroups] eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID 
	WHERE 
		Isnull(duplicateRecord,0) <> 1 
		AND SkippedRecord = 0 
		AND upper(eg.subscribetypecode) <> 'M'

	----UPDATE EMAILGROUPS
	--UPDATE  EmailGroups 
	--SET		FormatTypeCode = case when isnull(e.formattypecode,'') = '' then eg.formattypecode else lower(e.formattypecode) end,   
	--		SubscribeTypeCode = case when isnull(e.subscribetypecode,'') = '' then eg.subscribetypecode else upper(e.subscribetypecode) end,   
	--		LastChanged = @dt 
	--from #emails e with (NOLOCK) join emailgroups eg with (NOLOCK) on e.emailgroupID = eg.emailgroupID 
	--where Isnull(duplicateRecord,0) <> 1 and SkippedRecord = 0 and upper(eg.subscribetypecode) <> 'U'
	  
	--print ('Step 12 : Insert into EmailGroups = '  + convert(varchar,getdate(),109))  

	--INSERT INTO EMAILGROUPS
	INSERT INTO EmailGroups (
		EmailID, 
		GroupID, 
		FormatTypeCode, 
		SubscribeTypeCode, 
		CreatedOn)  
	SELECT
		EmailID, 
		@groupID,   
		case when isnull(formattypecode,'') = '' then 'html' else lower(formattypecode) end,   
		--case when isnull(subscribetypecode,'') = '' then 'S' else upper(subscribetypecode) end, 
		case when len(rtrim(ltrim(subscribetypecode))) > 0 then subscribetypecode else (case when len(rtrim(ltrim(@subscribetypecode))) > 0 then upper(@subscribetypecode) else 'S' end) end,  
		@dt 
	FROM 
		#emails   
	WHERE 
		emailgroupID IS NULL 
		AND Isnull(duplicateRecord,0) <> 1 
		AND SkippedRecord = 0 

	INSERT INTO @result VALUES ('I',@@ROWCOUNT)

	  
	/************************************************  
	EMAILDATAVALUES - UDF FIELD updates  
	************************************************/  
	
	IF EXISTS(SELECT TOP 1 GroupDatafieldsID FROM GroupDatafields WITH (NOLOCK) WHERE GroupID = @GroupID)
	BEGIN
	
		CREATE TABLE #UDF 
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
		
		select @tUDF_Exists = case when DatafieldSetID > 0 then 1 else 0 end from GroupDatafields WITH (NOLOCK) WHERE GroupID = @GroupID and isnull(DatafieldSetID,0) > 0
		  
		EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlUDF  
		  
		INSERT INTO #UDF (
			Emailaddress, 
			GroupDataFieldsID, 
			Datavalue, 
			EntryID, 
			DataFieldsetID, 
			RowID, 
			edvID)  
		SELECT 
			o.Emailaddress, 
			o.GroupDataFieldsid, 
			o.datavalue, 
			NULL, 
			g.DataFieldsetID, 
			RowID, 
			0  
		FROM   
			OPENXML(@docHandle, N'//row/udf')   
			WITH   
			(  
			  Emailaddress varchar(255) '../ea',  
			  GroupDataFieldsid int '@id',  
			  Datavalue varchar(500) 'v',  
			  Entrysid varchar(50) 'g',  
			  RowID int '@mp:parentid'  
			) o 
			JOIN Groupdatafields g WITH (NOLOCK) on g.GroupDataFieldsid = o.GroupDataFieldsid  
		  
		EXEC sp_xml_removedocument @docHandle   
		  		  				
		IF EXISTS (SELECT TOP 1 * FROM #UDF)   
		BEGIN  
		
			UPDATE 
				#UDF  
			SET 
				EmailID = e.EmailID  
			FROM   
				#UDF u 
				join Emails e  with (NOLOCK) on u.emailaddress = e.emailaddress and customerID = @customerID 
				join Emailgroups eg with (NOLOCK)  on e.emailID = eg.emailID and eg.groupID = @groupID   
				
			------------------------------------------------------
			------------------ Transactional UDFs ------------------
			------------------------------------------------------
				
			if @tUDF_Exists = 1 -- IF TRANSACTION udf EXISTS
			Begin

				select @primaryKeyID = GroupDatafieldsID from GroupDatafields WITH (NOLOCK) where GroupID = @GroupID and DatafieldSetID > 0 and IsPrimaryKey = 1
				
				if @primaryKeyID > 0 -- IF PRIMARY KEY EXISTS
				BEGIN
					INSERT INTO @entryID 
					SELECT 
						RowID, 
						DatafieldSetID, 
						edv.EntryID  
					FROM 
						#UDF u 
						join EmailDataValues edv WITH (NOLOCK) on u.GroupDataFieldsID = edv.GroupDataFieldsID and u.EmailID = edv.EmailID and u.Datavalue = edv.DataValue
					WHERE
						u.DataFieldsetID > 0 
						AND edv.GroupDataFieldsID = @primaryKeyID   
				END

				-- Add Entry ID if need to maintain History.  
				INSERT INTO 
					@entryID  
				SELECT 
					RowID, 
					DatafieldSetID, 
					newID() as EntryID 
				FROM 
					#UDF ud 
				WHERE 
					ISNULL(datafieldsetID,0) <> 0 
					and Rowid NOT IN (Select RowID from @entryID)
				GROUP BY 
					RowID, 
					DatafieldSetID  

				UPDATE 
					#UDF  
				SET 
					EntryID = inn1.EntryID  
				FROM 
					#UDF u 
					join @entryID inn1 on inn1.rowID = u.rowID and inn1.DatafieldSetID = u.DatafieldSetID  
			
				if @primaryKeyID > 0 -- DO TRANSACTIONAL UPDATES ONLY IF PRIMARY KEY EXISTS
				BEGIN
					UPDATE 
						#UDF
					SET 
						edvID = e.EmailDataValuesID
					FROM   
						#UDF u  with (NOLOCK)  
						join emaildatavalues e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID and e.entryID = u.entryID
					WHERE 
						u.EmailID is not null 
						and u.EntryID is not null
						
					
					UPDATE 
						EmailDataValues
					SET 
						DataValue = u.Datavalue, 
						ModifiedDate = @dt
					FROM   
						emaildatavalues e WITH (NOLOCK) 
						JOIN #UDF u  on e.EmailDataValuesID = u.edvID
					WHERE 
						u.edvID > 0
				END	
					
				-- New UDF Records  - Transactions  
				INSERT INTO Emaildatavalues (
					EmailID, 
					GroupDatafieldsID, 
					DataValue, 
					ModifiedDate, 
					EntryID)  
				SELECT 
					EmailID, 
					GroupDatafieldsID, 
					DataValue, 
					@dt, 
					EntryID   
				FROM 
					#UDF 
				WHERE 
					EmailID IS NOT NULL 
					AND  EntryID IS NOT NULL 
					AND Isnull(DataValue,'') <> ''  
					AND edvID = 0
	
			END
			
			------------------------------------------------------
			------------------ stand alone UDFs ------------------
			------------------------------------------------------
		  
			UPDATE 
				#UDF
			SET
				edvID = e.EmailDataValuesID
			FROM   
				#UDF u  with (NOLOCK)  
				JOIN emaildatavalues e  with (NOLOCK) on e.emailID = u.emailID and e.groupdatafieldsID = u.groupdatafieldsID
			WHERE 
				u.EmailID IS NOT NULL 
				and u.EntryID IS NULL
						
		  
			-- Existing UDF Records   
			UPDATE 
				emaildatavalues  
			SET 
				DataValue = u.datavalue,  
				ModifiedDate = @dt 
			FROM   
				emaildatavalues e  WITH (NOLOCK) 
				join #UDF u with (NOLOCK) on e.EmailDataValuesID = u.edvID 
			WHERE   
				u.EntryID IS NULL  
		  
			-- New UDF Records    
			INSERT INTO Emaildatavalues (
				EmailID, 
				GroupDatafieldsID, 
				DataValue, 
				ModifiedDate, 
				EntryID)  
			SELECT 
				u.EmailID, 
				u.GroupDatafieldsID, 
				MAX(u.DataValue), 
				@dt, 
				NULL   
			FROM
				#UDF u  with (NOLOCK) 
			WHERE 
				u.edvID = 0 
				--AND u.edvID IS NULL 
				AND u.EmailID IS NOT NULL 
				AND u.EntryID IS NULL 
				AND Isnull(u.DataValue,'') <> ''  
			GROUP BY 
				u.EmailID, 
				u.GroupDatafieldsID
			
		END  
		DROP TABLE #UDF
	END
	
	DROP TABLE #Emails
	
	UPDATE
		ImportEmailTracking 
	SET 
		EndTime = GETDATE() 
	WHERE 
		TrackingID = @TrackingID
	
	SELECT * FROM @Result  
END