-- DROP PROC e_EmailGroup_GetBestProfileForEmailAddress

CREATE proc [dbo].[e_EmailGroup_GetBestProfileForEmailAddress]
(    
	@GroupID int,    
	@CustomerID int,    
	@Filter varchar(8000),    
	@SubscribeType varchar(100) ,
	@ProfileFilter varchar(100) = 'ProfilePlusAllUDFs'      
)    
as    
---------------------------------------------------------------
-- 2015/11/02 Created from ECN/Branches/Release/2015_Q3/
--		      [ECN5_COMMUNICATOR].[dbo].[sp_GetGroupEmailProfilesWithUDF] 
---------------------------------------------------------------
BEGIN  
  set NOCOUNT ON  
  if not exists(select GroupID from groupdatafields  WITH (NOLOCK)  where GroupDatafields.groupID = @GroupID) or @ProfileFilter = 'ProfileOnly'
	Begin  
		print  ( @SubscribeType)  
		exec ( 'select top 1 ' 
		+' Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' 
		+' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, '
		+' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' 
		+' Convert(varchar(1000),Notes) as Notes, password, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, EmailGroups.SMSEnabled '
		+' from  Emails  WITH (NOLOCK) LEFT OUTER join EmailGroups WITH (NOLOCK) on EmailGroups.EmailID = Emails.EmailID '
		+' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+')'
		+ @Filter
		+ ' order by CASE ISNULL(EmailGroups.GroupID,0) WHEN ' + @GroupID + ' THEN 1 ELSE 0 END desc '
		+ ' , ISNULL(Emails.DateUpdated,ISNULL(Emails.DateAdded,''01-01-1900'')) desc ')
	End  
	Else --if UDF's exists  
	Begin
		DECLARE @StandAloneUDFs VARCHAR(3000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields  WITH (NOLOCK) WHERE GroupID = @GroupID AND DatafieldSetID is null AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields  WITH (NOLOCK) WHERE GroupID = @GroupID AND DatafieldSetID > 0  AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		declare @sColumns varchar(200),
				@tColumns varchar(200),
				@standAloneQuery varchar(4000),
				@TransactionalQuery varchar(4000)

		if LEN(@standaloneUDFs) > 0 and (@ProfileFilter = 'ProfilePlusStandalone' or @ProfileFilter = 'ProfilePlusAllUDFs')
		Begin
			 set @sColumns = ', SAUDFs.* '
			 set @standAloneQuery= ' left outer join			
					(
						SELECT *
						 FROM
						 (
							SELECT edv.emailID as temp_EmailID,  gdf.ShortName, edv.DataValue
							from	EmailDataValues edv  WITH (NOLOCK) join  
									Groupdatafields gdf  WITH (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							where 
									gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null  AND gdf.IsDeleted = 0
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
					) 
				SAUDFs on Emails.emailID = SAUDFs.temp_EmailID'
		End

		if LEN(@TransactionalUDFs) > 0 and @ProfileFilter = 'ProfilePlusAllUDFs'
		Begin
			 set @tColumns = ', TUDFs.* '
			 set @TransactionalQuery= '  left outer join
					(
						SELECT *
						 FROM
						 (
							SELECT edv.emailID as temp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
							from	EmailDataValues edv WITH (NOLOCK) join  
									Groupdatafields gdf  WITH (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							where 
									gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0  AND gdf.IsDeleted = 0
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
					) 
				TUDFs on Emails.emailID = TUDFs.temp_EmailID1 '
		End
		
	exec(
	' select top 1 ' 
	           + ' Emails.EmailID, Emails.EmailAddress, Emails.Title, Emails.FirstName, '
	           + ' Emails.LastName, Emails.FullName, Company, Occupation, Address, Address2, ' 
	           +  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' 
	           +  ' Emails.User1, User2, User3, User4, User5, User6, Birthdate, ' 
	           +  ' UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' 
	           +  ' Convert(varchar(1000),Notes) as Notes, password, '
	           +  ' GroupID, FormatTypeCode, EmailGroups.SubscribeTypeCode, EmailGroups.CreatedOn, EmailGroups.LastChanged, EmailGroups.SMSEnabled ' 
	           +  @sColumns
	           +  @tColumns
	           +  ' from  Emails  WITH (NOLOCK) LEFT OUTER join '
	           +  '  EmailGroups  WITH (NOLOCK) on Emails.EmailID = EmailGroups.emailID' 
	           + @standAloneQuery
	           + @TransactionalQuery
	           + ' where Emails.CustomerID = ' + @CustomerID
	           + ' and EmailGroups.SubscribeTypeCode IN (' + @SubscribeType + ' ) '
	           + @Filter 
	           +  ' order by CASE ISNULL(EmailGroups.GroupID,0) WHEN ' + @GroupID + ' THEN 1 ELSE 0 END desc '
			   +  ' , ISNULL(Emails.DateUpdated,ISNULL(Emails.DateAdded,''01-01-1900'')) desc '
			   + '  , Emails.EmailAddress'
			)			
	END 

END
