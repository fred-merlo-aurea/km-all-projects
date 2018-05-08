Create proc [dbo].[sp_copyProfilesToGroup]  
(  
 @SourceGroupID int,   
 @DestinationGroupID int,   
 @FullSync bit,   
 @filter varchar(max) = ''  
)  
as  
Begin  
	set nocount on  
   
	declare	@srcCustomerID int,  
			@DestCustomerID int,  
			@dt datetime ,
			@EmailInsertCount  int,
			@EmailUpdateCount	int,
			@EDVInsertCount	int,
			@EDVUpdateCount	int,
			@EmailGroupInsertCount int
     
	set	@srcCustomerID = 0  
	set	@DestCustomerID = 0  
	set	@dt = GETDATE()  
	set	@EmailInsertCount = 0  
	set	@EmailUpdateCount = 0  
	set	@EDVInsertCount = 0  
	set	@EDVUpdateCount = 0  
	set	@EmailGroupInsertCount = 0
 
     
	select  @srcCustomerID = CustomerID from Groups where GroupID = @SourceGroupID  
	select  @DestCustomerID = CustomerID from Groups where GroupID = @DestinationGroupID  
  
	if @filter <> ''  
		set @filter = @filter + ' and '  
	    
	set @filter = @filter + ' EmailAddress not like ''%@%.kmpsgroup.com'''  
	  
	if @FullSync = 0  
	Begin  
		set @filter = @filter + ' and (convert(DATE,isnull(EmailGroups.LastChanged, EmailGroups.createdon)) =  ''' + Convert(varchar(10), GETDATE()-1, 101)+ ''' or convert(DATE,isnull(emails.DateUpdated, emails.DateAdded)) =  ''' + Convert(varchar(10), GETDATE()-1, 101)+ ''') '  
	End  
   
  
	CREATE TABLE #srcEmails   
	(   
	DestEmailID int, EmailID int, EmailAddress varchar(255), Title varchar(50),FirstName varchar(50),LastName varchar(50),FullName varchar(50),Company varchar(100), Occupation varchar(50),  
	Address varchar(255),Address2 varchar(255),City varchar(50),State varchar(50),Zip varchar(50),Country varchar(50),Voice varchar(50),Mobile varchar(50),Fax varchar(50),Website varchar(50),  
	Age varchar(50),Income varchar(50),Gender varchar(50),User1 varchar(255),User2 varchar(255),User3 varchar(255),User4 varchar(255),User5 varchar(255),User6 varchar(255),Birthdate datetime,  
	UserEvent1 varchar(50),UserEvent1Date datetime,UserEvent2 varchar(50),UserEvent2Date datetime,Notes varchar(1000),Password varchar(25), DateToUser DateTime
	)  
  
	CREATE CLUSTERED INDEX IDX_C_srcEmails_EmailID ON #srcEmails(DestEmailID)  
	CREATE INDEX IDX_C_srcEmails_EmailAddress ON #srcEmails(EmailAddress)  
      
      
	if @SourceGroupID > 0 and @DestinationGroupID > 0 and @srcCustomerID > 0 and @DestCustomerID > 0  
	Begin  
		insert into #srcEmails 
			(EmailID, EmailAddress, Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,  
			City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,  
			UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,Password,DateToUser)  
		exec sp_getGroupDatausingFilter @SourceGroupID, @filter   
    
		--check if they have same customerID   
		if @srcCustomerID <> @DestCustomerID  
		Begin  
			/* Update Emails */   
			update emails  
			set  
				Title=src.Title,FirstName=src.FirstName,LastName=src.LastName,FullName=src.FullName,Company=src.Company,Occupation=src.Occupation,Address=src.Address,Address2=src.Address2,City=src.City,State=src.State,  
				Zip=src.Zip,Country=src.Country,Voice=src.Voice,Mobile=src.Mobile,  
				Fax=src.Fax,Website=src.Website,Age=src.Age,Income=src.Income,Gender=src.Gender,User1=src.User1,User2=src.User2,User3=src.User3,User4=src.User4,User5=src.User5,User6=src.User6,  
				Birthdate=src.Birthdate,UserEvent1=src.UserEvent1,UserEvent1Date=src.UserEvent1Date,UserEvent2=src.UserEvent2,UserEvent2Date=src.UserEvent2Date,Notes=src.Notes,  
				Password=src.Password, DateUpdated = DateToUser  
			from 
				Emails e 
				join #srcEmails src on src.emailaddress = e.emailaddress  
			where   
				e.CustomerID =  @DestCustomerID     

			set @EmailUpdateCount = @@ROWCOUNT
			--print 'Insert Emails'  

			update #srcEmails  
			set  
				destEmailID = e.emailID  
			from 
				#srcEmails src 
				join Emails e on src.emailaddress = e.EmailAddress  
			where e.CustomerID = @DestCustomerID  

			insert into Emails 
				(EmailAddress,CustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,DateAdded,DateUpdated)  
			select 
				EmailAddress,@DestCustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,DateToUser, DateToUser
			from 
				#srcEmails  
			where 
				isnull(destEmailID,0) = 0  
   
			set @EmailInsertCount = @@ROWCOUNT
	     
			update #srcEmails  
			set  
				destEmailID = e.emailID  
			from 
				#srcEmails src 
				join Emails e on src.emailaddress = e.EmailAddress  
			where 
				e.CustomerID = @DestCustomerID      
		End  
		Else  
		Begin  
			Update #srcEmails 
			set 
				DestEmailID = EmailID    
		End  
    
		/* EmailGroups*/  
		insert into EmailGroups 
		(EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)  
		select distinct src.destEmailID, @DestinationGroupID, 'html', 's', DateToUser, DateToUser
		from 
			#srcEmails src 
			left outer join EmailGroups eg with (NOLOCK) on src.destEmailID = eg.EmailID and eg.GroupID = @DestinationGroupID 
		where 
			eg.EmailID is null  
  
		set @EmailGroupInsertCount = @@ROWCOUNT
  
		if exists (select top 1 GroupDatafieldsID from GroupDatafields where GroupID = @DestinationGroupID and IsDeleted = 0)  
		Begin    
			CREATE TABLE #srcEmailDatavalues 
			(  
				EmailDataValuesID BIGINT,--int,   
				DestEmailID int,   
				GroupDatafieldsID int,   
				shortname varchar(100),   
				DataValue varchar(255),
				ModifiedDate datetime
			)    
	  
			CREATE CLUSTERED INDEX IDX_C_srcEmailDatavalues_EmailDataValuesID ON #srcEmailDatavalues(EmailDataValuesID)  
			CREATE INDEX IDX_C_srcEmailDatavalues_EmailID_GroupDatafieldsID ON #srcEmailDatavalues(DestEmailID, GroupDatafieldsID)  
	        
			/* EmailDataValues */  
			insert into #srcEmailDatavalues (EmailDataValuesID, DestEmailID, GroupDatafieldsID, shortname, DataValue,ModifiedDate)  
			select 
				0 as EmailDataValuesID, src.destEmailID, gdf1.GroupDatafieldsID, gdf1.ShortName, DataValue, ISNULL(edv.ModifiedDate, edv.CreatedDate)   
			from   
				#srcEmails src  
				join EmailDataValues edv  WITH (NOLOCK)  on src.emailID = edv.emailID  
				join groupdatafields gdf WITH (NOLOCK)  on edv.GroupDatafieldsID = gdf.GroupDatafieldsID   
				join groupdatafields gdf1 WITH (NOLOCK)  on gdf.ShortName = gdf1.ShortName  
			where   
				gdf1.groupID = @DestinationGroupID and    
				gdf.groupID = @SourceGroupID and   
				entryID is null and  
				gdf.IsDeleted = 0 and  
				gdf1.IsDeleted = 0  
	  
			update #srcEmailDatavalues  
			set  
				EmailDataValuesID = edv.emaildatavaluesID  
			from 
				#srcEmailDatavalues src 
				join emaildatavalues edv on edv.EmailID = src.destEmailID and edv.GroupDatafieldsID = src.GroupDatafieldsID  
		     
			update EmailDataValues  
			set 
				DataValue = edv1.datavalue, ModifiedDate = edv1.ModifiedDate
			from 
				EmailDataValues edv 
				join #srcEmailDatavalues edv1 on edv.EmailDataValuesID = edv1.EmailDataValuesID  
		  
			set @EDVUpdateCount = @@ROWCOUNT
			--select @@ROWCOUNT as 'update edv'  
	  
			insert into EmailDataValues 
				(EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)   
				select 
					destEmailID, groupdatafieldsID, datavalue, ModifiedDate, -1, null, ModifiedDate   
				from  
					#srcEmailDatavalues  
				where 
					EmailDataValuesID = 0  
		   
			set @EDVInsertCount = @@ROWCOUNT
		    
			drop table #srcEmailDatavalues  
	  
		End   
    
	End
 
	--update emailgroups subscribetypecode if email in master suppression
	UPDATE
		EmailGroups
	SET
		SubscribeTypeCode = 'M',
		LastChanged = DateAdd(ss,-1,Convert(varchar(10), GETDATE(), 101))
	WHERE
		GroupID = @DestinationGroupID and
		SubscribeTypeCode not in ('U', 'M')  and
		EmailID in (
						SELECT 
							EmailID 
						FROM 
							EmailGroups eg WITH (NOLOCK)
							join Groups g WITH (NOLOCK) ON eg.GroupID = g.GroupID
						WHERE
							g.CustomerID = @DestCustomerID and
							g.MasterSupression = '1'
					)   
	--add to log
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_TYPE='BASE TABLE'  AND TABLE_NAME='Log_sp_copyProfilesToGroup') 
	Begin
		--create table Log_sp_copyProfilesToGroup (CopyLogID int IDENTITY(1,1), sourceGroupID int, DestinationGroupID int, FullSync bit, Filter Varchar(max),  EmailInsert int, EmailUpdate int, EmailgroupInsert int, EDVInsert int, EDVUpdate int, Starttime datetime, endtime datetime, PRIMARY KEY (CopyLogID))
		Insert into Log_sp_copyProfilesToGroup 
			(sourceGroupID, DestinationGroupID , FullSync, Filter,  EmailInsert, EmailUpdate, EmailgroupInsert, EDVInsert, EDVUpdate, Starttime , endtime) 
		values
			(@SourceGroupID, @DestinationGroupID, @FullSync, @filter, @EmailInsertCount, @EmailUpdateCount, @EmailGroupInsertCount, @EDVInsertCount, @EDVUpdateCount, @dt, GETDATE())
	End
  
	drop table #srcEmails  
End