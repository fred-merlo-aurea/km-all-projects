CREATE PROCEDURE [dbo].[e_EmailGroup_ImportEmails_PreImportResults]
(  
 @CustomerID int,  
 @GroupID int,  
 @xmlProfile text
)  
AS
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
	
	declare @result TABLE  (Action varchar(100), Counts int)  

	create TABLE #emails 
	(  
		tmpEmailID int IDENTITY(1,1) , 
		EmailID int, 
		EmailGroupID int,
		Emailaddress varchar(255),  
		subscribetypecode  varchar(1), 
		duplicateRecord bit,
		SkippedRecord bit  ,
		ActionCode varchar(100),
		Reason varchar(100)
	)  

	CREATE INDEX EA_1 on #emails (Emailaddress)
	CREATE INDEX EA_2 on #emails (EmailID, SkippedRecord, duplicateRecord)

 
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlProfile  
   
	-- IMPORT FROM XML TO TEMP TABLE
	insert into #emails 
	(
		Emailaddress, subscribetypecode, SkippedRecord
	)  
	SELECT 
		LTRIM(RTRIM(Emailaddress)), 
		case when isnull(subscribetypecode,'') not in ('S','U','P','D','B','M') then '' else subscribetypecode end,
		case when isnull(Emailaddress,'') = '' then 1 else 0 end
	FROM OPENXML(@docHandle, N'/XML/Emails')   
	WITH   
	(  
		Emailaddress varchar(255) 'emailaddress', subscribetypecode varchar(50) 'subscribetypecode'
	) 
	
	EXEC sp_xml_removedocument @docHandle    
	
	insert into @result values ('T',@@ROWCOUNT)  
    declare @insertcount varchar(50)
    set @insertcount = (select COUNT(tmpEmailID) from #emails)

	Update #emails
	set SkippedRecord = 1, Reason='Skipped'
	where dbo.fn_ValidateEmailAddress(Emailaddress) = 0 or ISNULL(subscribetypecode,'') = 'M'

	insert into @result
	select 'S', @@ROWCOUNT

	--update duplicate records in temp tables  
	update #emails  
	set duplicateRecord = 1, Reason='DuplicateRecord', ActionCode='D'  
	from	#emails e join   
			(select max(el.tmpEmailID) as tmpEmailID, el.emailaddress from #emails el group by el.emailaddress) el1 
			on  e.emailaddress = el1.emailaddress  
	where e.tmpemailID <> el1.tmpEmailID and SkippedRecord = 0 
	  
	insert into @result values ('D',@@ROWCOUNT)
	
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
		
	declare @SuppGroupID int, @BaseChannelID int
	select @SuppGroupID = groupid from [Groups] WITH (NOLOCK) where CustomerID = @CustomerID and MasterSupression = 1
	select @BaseChannelID = BaseChannelID from [ECN5_ACCOUNTS].[DBO].Customer WITH (NOLOCK) where customerID = @CustomerID and IsDeleted = 0

	--channel master suppression
	update #emails  
	set SkippedRecord = 1  , Reason='MasterSuppressed'  , ActionCode='M'
	from #emails e 
		join ChannelMasterSuppressionList cmsl WITH (NOLOCK) on e.Emailaddress = cmsl.EmailAddress
	where SkippedRecord = 0 and cmsl.BaseChannelID = @BaseChannelID and cmsl.IsDeleted = 0

	SELECT @skippedcount = @@ROWCOUNT
	
	if not exists (select top 1 1 from @result where Action = 'M')
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
	
	if not exists (select top 1 1 from @result where Action = 'M')
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
	
	INSERT INTO @result VALUES ('I', (select COUNT(*) from #emails e where emailgroupID IS NULL 
			AND Isnull(duplicateRecord,0) <> 1 
			AND SkippedRecord = 0))

	DROP TABLE #emails
	
	SELECT * FROM @Result  

End
