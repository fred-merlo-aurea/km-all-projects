CREATE proc [dbo].[sp_ExportSurveyRespondents] 
(    
	@GroupID int,    
	@CustomerID int 
)    
as   
BEGIN  
  set NOCOUNT ON  
   CREATE TABLE #standAloneUDF(temp_EmailID int, ShortName varchar(200), DataValue varchar(8000)) 
	insert into #standAloneUDF 
	SELECT edv.emailID as temp_EmailID,  gdf.ShortName, edv.DataValue
	from	EmailDataValues edv  join  
			Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
	where 
			gdf.GroupID = @GroupID and datafieldsetID is null  AND gdf.IsDeleted = 0
	order by edv.EmailID
	
	CREATE TABLE #transUDF(temp_EmailID int, temp_ShortName varchar(200), temp_DataValue varchar(8000)) 
	insert into #transUDF 
	SELECT edv.emailID as temp_EmailID,  gdf.ShortName as temp_ShortName, edv.DataValue as temp_DataValue
	from	EmailDataValues edv  join  
			Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
	where 
			gdf.GroupID = @GroupID and datafieldsetID > 0   AND gdf.IsDeleted = 0
	order by edv.EmailID
	
  if not exists(select GroupID from groupdatafields where GroupDatafields.groupID = @GroupID)  
	Begin  

		exec ( 'select Emails.EmailID, EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' Convert(varchar,Notes) as Notes, password, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode ' +  
		' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +   
		' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode =''S'''+  
		' and EmailGroups.GroupID = ' + @GroupID)  
	End  
	Else --if UDF's exists  
	Begin
		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0  AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		declare @sColumns varchar(200),
				@tColumns varchar(200),
				@standAloneQuery varchar(4000),
				@TransactionalQuery varchar(4000)

		if LEN(@standaloneUDFs) > 0
		Begin
			 set @sColumns = ', SAUDFs.* '
			 set @standAloneQuery= ' left outer join			
					(
						SELECT *
						 FROM
						 (
							select temp_EmailID, ShortName, SUBSTRING(
							(select ('', '' + q2.DataValue) 
							from 
							#standAloneUDF q2
							where q2.temp_EmailID=q1.temp_EmailID and q2.ShortName=q1.ShortName
							order by temp_EmailID, ShortName, DataValue
							for XML PATH('''')),3,1000) as DataValue
							from 
							#standAloneUDF q1
							group by temp_EmailID, ShortName
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
							select temp_EmailID, ShortName, SUBSTRING(
							(select ('', '' + q2.DataValue) 
							from 
							#transUDF q2
							where q2.temp_EmailID=q1.temp_EmailID and q2.ShortName=q1.ShortName
							order by temp_EmailID, ShortName, DataValue
							for XML PATH('''')),3,1000) as DataValue
							from 
							#transUDF q1
							group by temp_EmailID, ShortName
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
					) 
				TUDFs on Emails.emailID = TUDFs.temp_EmailID1 '
		End 
	  exec (' select Emails.EmailID, Emails.EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' Emails.User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar,Notes) as Notes, password, EmailGroups.CreatedOn, EmailGroups.LastChanged, GroupID, FormatTypeCode, EmailGroups.SubscribeTypeCode ' +
			 @sColumns + @tColumns + 
			' from 
				Emails join 
				EmailGroups on Emails.EmailID = EmailGroups.emailID' + @standAloneQuery + @TransactionalQuery + 
			' where Emails.CustomerID = ' + @CustomerID +  
			' and EmailGroups.GroupID = ' + @GroupID + ' order by Emails.EmailAddress')  
	END 
	drop table #standAloneUDF
	drop table #transUDF
END

