CREATE PROCEDURE [dbo].[spClickActivityDetailedReport](
	@BlastID int,    
	@LinkURL varchar (8000)  )
AS
BEGIN 	
	--set @BlastID = 433151
	--set @LinkURL = 'http://www.medtechinsider.de/?p=470'  
 

	set NOCOUNT ON    
     
	declare	@GroupID int  
    
	select @GroupID =GroupID from ecn5_communicator..[BLAST] with(nolock) where BlastID = @BlastID    
     
       
	create table #tempA (EmailID int, ActionDate datetime)    
 

	if right(@LinkURL,3) = 'eid'
	Begin
		insert into #tempA    
		select	EmailID, BlastActivityClicks.ClickTime as 'ActionDate' from BlastActivityClicks with(nolock)
		where	blastID = @BlastID and URL like @LinkURL + '%'
	end
	else
	Begin
		insert into #tempA    
		select	EmailID, BlastActivityClicks.ClickTime as 'ActionDate' from BlastActivityClicks with(nolock) 
		where	blastID = @BlastID and URL = @LinkURL

	end

    if not exists(select top 1 GroupDatafieldsID from ecn5_communicator..groupdatafields  where GroupDatafields.groupID = @GroupID)    
	Begin    
		select	Emails.EmailID, EmailAddress, @LinkURL as Link, #tempA.ActionDate as ActionDate, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, 
				City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, 
				User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, 
				Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode 
		from	
				ecn5_communicator..Emails with(nolock) join ecn5_communicator..EmailGroups with(nolock) on EmailGroups.EmailID = Emails.EmailID join 
				#tempA on #tempA.EmailID = Emails.EmailID 
		where 
				EmailGroups.GroupID = @GroupID 
		order by Emails.EmailID 
	End    
	Else --if UDF's exists    
	Begin    
		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
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
						SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
						from	ecn5_communicator..EmailDataValues edv with(nolock)  join  
								ecn5_communicator..Groupdatafields gdf with(nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID  join 
								#tempA on #tempA.EmailID = edv.EmailID 
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
					from	ecn5_communicator..EmailDataValues edv with(nolock) join  
							ecn5_communicator..Groupdatafields gdf with(nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID join 
							#tempA on #tempA.EmailID = edv.EmailID 
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
		

		exec (	'select Emails.EmailID, EmailAddress, '''+@LinkURL+''' as Link, #tempA.ActionDate as ActionDate,  Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +    
				' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +    
				' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +    
				' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +      
				' from ecn5_communicator..Emails with(nolock) join ' +          
				' ecn5_communicator..EmailGroups with(nolock) on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +        
				' join #tempA on #tempA.EmailID = Emails.EmailID '+      
				' where ecn5_communicator..EmailGroups.GroupID = ' + @GroupID +  
				' order by #tempA.ActionDate desc, emailaddress')   
    
	END    
	drop table #tempA    


END
