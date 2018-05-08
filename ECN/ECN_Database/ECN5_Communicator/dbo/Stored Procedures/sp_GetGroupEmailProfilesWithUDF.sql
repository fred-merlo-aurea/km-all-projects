---------------------------------------------------------------
-- 4/10/2014 Updated with NOLOCK hints to reduced blocking MK TFS Task 7005
--
---------------------------------------------------------------

CREATE proc [dbo].[sp_GetGroupEmailProfilesWithUDF] 
(    
	@GroupID int,    
	@CustomerID int,    
	@Filter varchar(8000),    
	@SubscribeType varchar(100) ,
	@ProfileFilter varchar(100) = 'ProfilePlusAllUDFs'      
)    
as    
BEGIN  
--declare 
--	@GroupID int = 191545,    
--	@CustomerID int = 3643,    
--	@Filter varchar(8000) = ' AND ((
--	(
--		CONVERT(VARCHAR, ISNULL([CAT], '''')) = CONVERT(VARCHAR, ''10'')
--	)  
--	OR 
--	(
--		CONVERT(VARCHAR, ISNULL([CAT], '''')) = CONVERT(VARCHAR, ''70'')
--	) 
--)) ',    
--	@SubscribeType varchar(100) = ' ''S'', ''U'', ''D'', ''P'', ''B'', ''M'' ' ,
--	@ProfileFilter varchar(100) = 'ProfilePlusAllUDFs'  
  set NOCOUNT ON  
   
  if not exists(select GroupID from groupdatafields  WITH (NOLOCK)  where GroupDatafields.groupID = @GroupID)
	Begin  

		exec ( 'select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' Convert(varchar(1000),Notes) as Notes, password, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' +  
		' from  Emails  WITH (NOLOCK) join EmailGroups WITH (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +   
		' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+')'+  
		' and EmailGroups.GroupID = ' + @GroupID + ' ' + @Filter)  
	End  
	Else --if UDF's exists  
	Begin

		DECLARE @StandAloneUDFs VARCHAR(MAX)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields  WITH (NOLOCK) WHERE GroupID = @GroupID AND DatafieldSetID is null AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(MAX)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields  WITH (NOLOCK) WHERE GroupID = @GroupID AND DatafieldSetID > 0  AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'
		DECLARE @NoDupes VARCHAR(50)
		SET @NoDupes = ''

		declare @sColumns varchar(MAX),
				@tColumns varchar(MAX),
				@standAloneQuery varchar(MAX),
				@TransactionalQuery varchar(MAX)

		if LEN(@standaloneUDFs) > 0 
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
		
		if LEN(@TransactionalUDFs) > 0
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

		if @ProfileFilter <> 'ProfilePlusAllUDFs'
		BEGIN
			SET @tColumns = ''
			SET @NoDupes = 'distinct '
		END

		if @ProfileFilter = 'ProfileOnly'
		BEGIN
			SET @sColumns = ''
		END



		exec (' select ' + @NoDupes + 'Emails.EmailID, Emails.EmailAddress, Emails.Title, Emails.FirstName, Emails.LastName, Emails.FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' Emails.User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar(1000),Notes) as Notes, password, EmailGroups.CreatedOn, EmailGroups.LastChanged, GroupID, FormatTypeCode, EmailGroups.SubscribeTypeCode ' +
				@sColumns + @tColumns + 
			' from 
				Emails  WITH (NOLOCK) join 
				EmailGroups  WITH (NOLOCK) on Emails.EmailID = EmailGroups.emailID' + @standAloneQuery + @TransactionalQuery + 
			' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+')'+  
			' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' order by Emails.EmailAddress')  
		
	END
  
	 --exec (' select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			--' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			--' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			--' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, InnerTable2.* ' +  
			--' from Emails left outer join  (' +   
			--' select InnerTable1.EmailID as temp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from ' +
			--'( select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E) as InnerTable1 Group by InnerTable1.EmailID, InnerTable1.EntryID ) as InnerTable2 on Emails.EmailID = InnerTable2.temp_EmailID ' +  
			--' join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +  
			--' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+')'+  
			--' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter)  			
			
END