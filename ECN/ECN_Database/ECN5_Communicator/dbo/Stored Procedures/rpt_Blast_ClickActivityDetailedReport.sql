CREATE  PROCEDURE [dbo].[rpt_Blast_ClickActivityDetailedReport](
	@BlastID int,    
	@LinkURL varchar (8000),
	@CustomerID int,
	@ProfileFilter varchar(25) = 'ProfilePlusStandalone'  )
AS
BEGIN 	
	--set @BlastID = 433151
	--set @LinkURL = 'http://www.medtechinsider.de/?p=470'  
 

	set NOCOUNT ON    
     
	declare	@GroupID int  
    
	select @GroupID =GroupID from [Blast] with (nolock) where BlastID = @BlastID and CustomerID = @CustomerID 
     
       
	create table #tempA (EmailID int, ActionDate datetime)    
 

	if right(@LinkURL,3) = 'eid'
	Begin
		insert into #tempA    
		select	EmailID, ClickTime as ActionDate  from ecn_Activity..[BlastActivityClicks] bac with (nolock) 
		where	blastID = @BlastID and bac.URL like @LinkURL + '%' --= @LinkURL    -- like '''' +  @LinkURL + '%'''
		-- group by EmailID, ActionDate   -- commented by Sunil - get all click records - "group by " will eliminate dup records.
	end
	else
	Begin
		insert into #tempA    
		select	EmailID, ClickTime ActionDate  from ecn_Activity..[BlastActivityClicks] bac with (nolock)
		where	blastID = @BlastID and bac.URL = @LinkURL

	end

    if not exists(select top 1 GroupDatafieldsID from [groupdatafields] with (nolock) where GroupDatafields.groupID = @GroupID and IsDeleted = 0)    
	Begin    
		select	Emails.EmailID, EmailAddress, @LinkURL as Link, #tempA.ActionDate as ActionDate, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, 
				City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, 
				User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, 
				Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode 
		from	
				[Emails] with (nolock) join [EmailGroups] with (nolock) on EmailGroups.EmailID = Emails.EmailID join 
				#tempA on #tempA.EmailID = Emails.EmailID 
		where 
				EmailGroups.GroupID = @GroupID 
		order by Emails.EmailID 
	End    
	Else --if UDF's exists    
	Begin    
		DECLARE @StandAloneUDFs VARCHAR(2000)
		if(@ProfileFilter = 'ProfilePlusStandalone' or @ProfileFilter = 'ProfilePlusAll')
		BEGIN
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM [groupdatafields] with (nolock) WHERE GroupID = @GroupID AND IsDeleted = 0 AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		END

		DECLARE @TransactionalUDFs VARCHAR(2000)
		if(@ProfileFilter = 'ProfilePlusAll')
		BEGIN
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM [groupdatafields] with (nolock) WHERE GroupID = @GroupID AND IsDeleted = 0 AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		END
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
						from	[EmailDataValues] edv with (nolock) join  
								[Groupdatafields] gdf with (nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID  join 
								#tempA on #tempA.EmailID = edv.EmailID 
						where 
								gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null and gdf.IsDeleted = 0 
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
					from	[EmailDataValues] edv with (nolock) join  
							[Groupdatafields] gdf with (nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID join 
							#tempA on #tempA.EmailID = edv.EmailID 
					where 
							gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0 and gdf.IsDeleted = 0
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
				' from [Emails] with (nolock) join ' +          
				' [EmailGroups] with (nolock) on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +        
				' join #tempA on #tempA.EmailID = Emails.EmailID '+      
				' where EmailGroups.GroupID = ' + @GroupID +  
				' order by #tempA.ActionDate desc, emailaddress')   
    
	END    
	drop table #tempA    


END