CREATE PROCEDURE [dbo].[e_EmailGroup_PreviewFilteredEmails]
(
	@GroupID int,
	@CustomerID int,
	@Filter varchar(8000)
)
AS
BEGIN

	if @Filter <> ''
	begin
		set @Filter= REPLACE(@Filter, 'CONVERT(VARCHAR,', 'CONVERT(VARCHAR(255),');
	end
	declare @gdf table(GID int, ShortName varchar(50)) 
		
	if LEN(@Filter) > 0        
	Begin
		insert into @gdf         
		select distinct gdf.GroupDatafieldsID, gdf.ShortName 
		from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
		where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0 and gdf.IsDeleted = 0         
	end
	Else
	Begin
		insert into @gdf         
		select distinct gdf.GroupDatafieldsID, gdf.ShortName 
		from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
		where  g.groupID = @GroupID and g.customerID = @CustomerID and gdf.IsDeleted = 0   
	End	

	
	IF not exists (select gdf.GroupDatafieldsID from GroupDataFields gdf WITH (NOLOCK) JOIN Groups WITH (NOLOCK) on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID and gdf.IsDeleted = 0) or not exists(select * from @gdf)
	Begin

  		exec (	' select distinct [Emails].EmailID, EmailAddress ' +
			  	' from  [Emails] WITH (NOLOCK) join [EmailGroups] WITH (NOLOCK) on [EmailGroups].EmailID = [Emails].EmailID ' +
		     	' where [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%'' and [EmailGroups].SubscribeTypeCode = ''S''' +
		     	' and [EmailGroups].GroupID = ' + @GroupID + 
				' ' + @Filter + ' order by [Emails].EmailID ')
	End
	Else
	Begin
		DECLARE @StandAloneUDFs VARCHAR(2000)
		DECLARE @TransactionalUDFs VARCHAR(2000)
		DECLARE @StandAloneUDFIDs VARCHAR(1000)
		DECLARE @TransactionalUDFIDs VARCHAR(1000)
		DECLARE @StandAloneColumns VARCHAR(200)
		DECLARE @TransactionalColumns VARCHAR(200)
		DECLARE @StandAloneQuery VARCHAR(4000)
		DECLARE @TransactionalQuery VARCHAR(4000)
		DECLARE @StandAloneTempQuery VARCHAR(4000)
		DECLARE @TransactionalTempQuery VARCHAR(4000)
		DECLARE @StandAloneDrop VARCHAR(500)
		DECLARE @TransactionalDrop VARCHAR(500)
				
		SET @StandAloneUDFs = ''
		SET @TransactionalUDFs = ''
		SET @StandAloneUDFIDs = ''
		SET @TransactionalUDFIDs  = ''
		SET @StandAloneColumns  = ''
		SET @TransactionalColumns = ''
		SET @StandAloneQuery  = ''
		SET @TransactionalQuery  = ''
		SET @standAloneTempQuery  = ''
		SET @TransactionalTempQuery  = ''		
		SET @StandAloneDrop  = ''
		SET @TransactionalDrop  = ''
		
		
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
				
		if LEN(@StandAloneUDFs) > 0
		Begin
			 set @StandAloneColumns = ' SAUDFs.* '
			 set @standAloneTempQuery = '
						SELECT * into #tempStandAlone
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
							from	EmailDataValues edv WITH (NOLOCK) join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @StandAloneUDFIDs + ') and gdf.IsDeleted = 0 
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt;
						 
						 CREATE INDEX IDX_tempStandAlone_EmailID ON #tempStandAlone(tmp_EmailID);'
			 
			set @StandAloneQuery = ' left outer join #tempStandAlone SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'			 
			 
			set @StandAloneDrop  = 'drop table #tempStandAlone'
		End

		if LEN(@TransactionalUDFs) > 0
		Begin
			if LEN(@StandAloneColumns) > 0
			Begin
				set @TransactionalColumns = ', TUDFs.* '
			end
			Else
			Begin
				set @TransactionalColumns = ' TUDFs.* '
			End
			set @TransactionalTempQuery =  '  
						SELECT * into #tempTransactional
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
							from	EmailDataValues edv WITH (NOLOCK)  join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @TransactionalUDFIDs + ') and gdf.IsDeleted = 0 
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt;
						 
						 CREATE INDEX IDX_tempTransactional_EmailID ON #tempTransactional(tmp_EmailID1);'			
			
			set @TransactionalQuery = '  left outer join #tempTransactional TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
			
			set @TransactionalDrop  = 'drop table #tempTransactional'
		End
		
		EXEC ( 
			@StandAloneTempQuery + ';' + 
			@TransactionalTempQuery + ';' +
		' select distinct [Emails].EmailID, EmailAddress from  Emails with (NOLOCK) ' +@StandAloneQuery + @TransactionalQuery + 
			' join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' and dbo.fn_ValidateEmailAddress(Emails.EmailAddress) = 1' +   
			' ' + @Filter + ' order by Emails.EmailID;' +
			@StandAloneDrop + ';' + 
			@TransactionalDrop + ';  ' 
			) 

	END

END