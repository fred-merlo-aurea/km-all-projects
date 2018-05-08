------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
--
--
--
------------------------------------------------------

CREATE PROC [dbo].[sp_copyProfiles_UDF_ToGroup_UPI](@FullSync bit, @SourceGroupID int , @DestinationGroupID int, @hasNonQualResponse bit)       
as    
Begin  
   declare   
   @SourceCustomerID int,       
   @DestinationCustomerID int,       
   @dt datetime  
          
 set @dt = GETDATE()   
 set nocount on     
   
 select @SourceCustomerID = CustomerID from Groups where GroupID = @SourceGroupID
 select @DestinationCustomerID = CustomerID from Groups where GroupID = @DestinationGroupID
  
CREATE TABLE #srcEmails (     
	DestEmailID int, 
	EmailID int, 
	EmailAddress varchar(255), 
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
	Notes varchar(1000),
	Password varchar(25),
	DateToUse datetime)  
     
CREATE TABLE #srcEmailDatavalues (
	EmailDataValuesID BIGINT,--int, 
	DestEmailID int, 
	GroupDatafieldsID int, 
	shortname varchar(100), 
	DataValue varchar(255))    
    
 IF @SourceGroupID > 0 and @DestinationGroupID > 0
 BEGIN
    
  declare @filter varchar(8000) 
  
  set @filter = '(((BUSINESS= ''T'' AND ' + case when @hasNonQualResponse = 1 then '1=1' else '1=0' end  + ')            
           OR Country not in (''UNITED STATES OF AMERICA'',''CANADA'')) AND EmailAddress not like ''20%@%.kmpsgroup.com'' AND len(Country) > 0 AND (' + 
		   case when @FullSync = 1 then '1=1' else '1=0' end  + ' OR (convert(varchar(10),isnull(EmailGroups.LastChanged, EmailGroups.createdon),101) = convert(varchar(10),GETDATE()-1,101) OR convert(varchar(10),isnull(Emails.DateUpdated, Emails.DateAdded),101)
 = convert(varchar(10),GETDATE()-1,101))))'
  
  
  insert into #srcEmails (EmailID, EmailAddress, Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,    
    City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,    
    UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,Password, DateToUse)     
  exec sp_getGroupDatausingFilter @SourceGroupID, @filter
  
  update #srcEmails set DestEmailID = 0     
       
  --check if they have same customerID     
  if @SourceCustomerID <> @DestinationCustomerID
  Begin    
   /* Update Emails */     
   update emails    
   set Title=src.Title,FirstName=src.FirstName,LastName=src.LastName,FullName=src.FullName,Company=src.Company,Occupation=src.Occupation,
   Address=src.Address,Address2=src.Address2,City=src.City,State=src.State,    
     Zip=src.Zip,Country=src.Country,Voice=src.Voice,Mobile=src.Mobile,    
     Fax=src.Fax,Website=src.Website,Age=src.Age,Income=src.Income,Gender=src.Gender,User1=src.User1,User2=src.User2,User3=src.User3,User4=src.User4,User5=src.User5,User6=src.User6,    
     Birthdate=src.Birthdate,UserEvent1=src.UserEvent1,UserEvent1Date=src.UserEvent1Date,UserEvent2=src.UserEvent2,UserEvent2Date=src.UserEvent2Date,Notes=src.Notes,    
     Password=src.Password, DateUpdated = src.DateToUse    
   from Emails e join #srcEmails src on src.emailaddress = e.emailaddress  
   where e.CustomerID = @DestinationCustomerID
       
   --print 'Insert Emails'    
     
   update #srcEmails    
   set  destEmailID = e.emailID    
   from #srcEmails src join Emails e on src.emailaddress = e.EmailAddress
   where e.CustomerID = @DestinationCustomerID
   
   --select * from #srcEmails  
       
   insert into Emails (EmailAddress,CustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,City,State,Zip,
   Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,
   User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,DateAdded,DateUpdated)    
   select EmailAddress,@DestinationCustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,
			Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,DateToUse, DateToUse
   from #srcEmails    
   where destEmailID = 0
                         
  End                 
 End  

 /* EmailGroups*/    
 select * from #srcEmails
 
 delete #srcEmails 
		from #srcEmails src join EmailGroups eg on src.DestEmailID = eg.EmailID and eg.GroupID = @DestinationGroupID
		

 select * from #srcEmails		
  
   
 if exists(select GroupID from Groups where GroupID = @DestinationGroupID)   
 begin  
	 insert into EmailGroups (EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)    
	 select distinct src.destEmailID as EmailID, @DestinationGroupID as GroupID, 'html' as FormatTypeCode, 'S' as SubscribeTypeCode, 
	 src.DateToUse as CreatedOn, src.DateToUse as LastChanged from #srcEmails src left outer join 
	 EmailGroups eg on src.destEmailID = eg.EmailID and eg.GroupID = @SourceGroupID where eg.EmailID is null  
 end  
             
 /* EmailDataValues */    
 insert into #srcEmailDatavalues (EmailDataValuesID, DestEmailID, GroupDatafieldsID, shortname, DataValue) 
	select 0 as EmailDataValuesID, src.destEmailID, gdf1.GroupDatafieldsID, gdf1.ShortName, DataValue 
	from #srcEmails src join EmailDataValues edv on src.emailID = edv.emailID join groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
	join groupdatafields gdf1 on gdf.ShortName = gdf1.ShortName    
	where gdf1.groupID = @DestinationGroupID and  gdf.groupID = @SourceGroupID and entryID is null  
       
 update #srcEmailDatavalues    
 set  EmailDataValuesID = edv.emaildatavaluesID    
 from #srcEmailDatavalues src join    
   emaildatavalues edv on edv.EmailID = src.destEmailID and edv.GroupDatafieldsID = src.GroupDatafieldsID    
     
 update EmailDataValues    
 set DataValue = edv1.datavalue, ModifiedDate = @dt    
 from EmailDataValues edv join  #srcEmailDatavalues edv1 on edv.EmailDataValuesID = edv1.EmailDataValuesID    
    
 --select @@ROWCOUNT as 'update edv'    
    
 insert into EmailDataValues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID)    
 select destEmailID, groupdatafieldsID, datavalue, @dt, -1, null     
 from  #srcEmailDatavalues    
 where EmailDataValuesID = 0
     
 --select @@ROWCOUNT as 'insert edv'    
    
 --select * from #srcEmailDatavalues     
 --select * from #srcEmails   
   
 -- Remove from the source group after moved to the destination group  
      
 delete from EmailGroups where GroupID = @SourceGroupID and EmailID in (select EmailID from #srcEmails)      
     
 drop table #srcEmails    
 drop table #srcEmailDatavalues   
   
End