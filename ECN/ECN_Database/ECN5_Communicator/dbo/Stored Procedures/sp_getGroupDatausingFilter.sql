CREATE PROC [dbo].[sp_getGroupDatausingFilter]  
(    
 @GroupID int,    
 @filter varchar(4000)    
)   
AS   
BEGIN  
	set nocount on

	declare @CustomerID int,  
			@sColumns varchar(200),    
			@tColumns varchar(200),    
			@standAloneQuery varchar(4000),    
			@TransactionalQuery varchar(4000) ,
			@StandAloneUDFs VARCHAR(2000),
			@TransactionalUDFs 	VARCHAR(2000)		
    
	set @sColumns = ''
	set @tColumns = ''
	set @standAloneQuery = ''
	set @TransactionalQuery = ''
  
  
  select @CustomerID = customerID from Groups where GroupID = @GroupID  
    
  SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields with (NOLOCK)
  WHERE GroupID = @GroupID AND DatafieldSetID is null and CHARINDEX(ShortName, @Filter) > 0 and IsDeleted = 0    
  ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'    

  SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields with (NOLOCK)
  WHERE GroupID = @GroupID AND DatafieldSetID > 0  and CHARINDEX(ShortName, @Filter) > 0 and IsDeleted = 0  
  ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'    
    
  if LEN(@standaloneUDFs) > 0    
  Begin    
    set @sColumns = ', SAUDFs.* '    
    set @standAloneQuery= ' left outer join       
     (    
      SELECT *    
       FROM    
       (    
       SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue    
       from EmailDataValues edv with (NOLOCK) join      
         Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID    
       where     
         gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null    
       ) u    
       PIVOT    
       (    
       MAX (DataValue)    
       FOR ShortName in (' + @StandAloneUDFs + ')) as pvt        
     )     
    SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'    
  End    
    
  if LEN(@TransactionalUDFs) > 0    
  Begin    
    set @tColumns = ', TUDFs.* '    
    set @TransactionalQuery= '  left outer join    
     (    
      SELECT *    
       FROM    
       (    
       SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue    
       from EmailDataValues edv with (NOLOCK) join      
         Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID    
       where     
         gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0    
       ) u    
       PIVOT    
       (    
       MAX (DataValue)    
       FOR ShortName in (' + @TransactionalUDFs + ')) as pvt        
     )     
    TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '    
  End    
    
  if len(@filter) > 0    
   set @filter = ' and ' + @filter       
       
  --print ('select distinct Emails.EmailID, Emails.EmailAddress, Emails.Title,Emails.FirstName,Emails.LastName,Emails.FullName,Emails.Company,Emails.Occupation,  
  -- Emails.Address,Emails.Address2,Emails.City,Emails.State,Emails.Zip,Emails.Country,Emails.Voice,Emails.Mobile,Emails.Fax,Emails.Website,  
  -- Emails.Age,Emails.Income,Emails.Gender,Emails.User1,Emails.User2,Emails.User3,Emails.User4,Emails.User5,Emails.User6,Emails.Birthdate,      
  -- Emails.UserEvent1,Emails.UserEvent1Date,Emails.UserEvent2,Emails.UserEvent2Date,Emails.Notes,Emails.Password ' + --@sColumns + @tColumns +     
  -- ' from     
  --  Emails join     
  --  EmailGroups on Emails.EmailID = EmailGroups.emailID' + @standAloneQuery + @TransactionalQuery +     
  -- ' where Emails.CustomerID = ' + convert(varchar,@CustomerID) +      
  -- '  and EmailGroups.GroupID = ' + convert(varchar,@GroupID) +     
  -- '  and emailgroups.subscribetypecode=''s'' ' + @filter)  
     
   exec ('select distinct Emails.EmailID, Emails.EmailAddress, Emails.Title,Emails.FirstName,Emails.LastName,Emails.FullName,Emails.Company,Emails.Occupation,  
   Emails.Address,Emails.Address2,Emails.City,Emails.State,Emails.Zip,Emails.Country,Emails.Voice,Emails.Mobile,Emails.Fax,Emails.Website,  
   Emails.Age,Emails.Income,Emails.Gender,Emails.User1,Emails.User2,Emails.User3,Emails.User4,Emails.User5,Emails.User6,Emails.Birthdate,      
   Emails.UserEvent1,Emails.UserEvent1Date,Emails.UserEvent2,Emails.UserEvent2Date,Emails.Notes,Emails.Password, ISNULL(EmailGroups.LastChanged,EmailGroups.CreatedOn) as DateToUse ' + --@sColumns + @tColumns +     
   ' from     
    Emails with (NOLOCK) join     
    EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.emailID' + @standAloneQuery + @TransactionalQuery +     
   ' where Emails.CustomerID = ' + @CustomerID +      
   '  and EmailGroups.GroupID = ' + @GroupID +     
   '  and emailgroups.subscribetypecode=''s'' ' + @filter)      
     
              
END

