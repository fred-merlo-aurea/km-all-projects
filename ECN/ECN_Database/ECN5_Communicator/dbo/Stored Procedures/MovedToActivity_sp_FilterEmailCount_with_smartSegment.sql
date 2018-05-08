CREATE  PROCEDURE [dbo].[MovedToActivity_sp_FilterEmailCount_with_smartSegment](
  @GroupID int,  
  @CustomerID int,  
  @Filter varchar(8000),  
  @ActionType varchar(10),  
  @BlastID int,   
  @rowCountOut int OUTPUT ) 
AS
BEGIN  
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_FilterEmailCount_with_smartSegment', GETDATE()) 
	--set @GroupID = 93815
	--set @CustomerID = 1
	--set @Filter = ''
	--set @ActionType = ''
	--set @BlastID = 433151 

	set NOCOUNT ON  

	declare   
	@SqlString  varchar(8000),   
	@EmailString  varchar(8000)

	set @SqlString = ''  
	set @EmailString  = '' 
     
	create table #tempA (EmailID int)  
  
	create table #E(  
	EmailDataValuesID int,  
	EmailID int,  
	GroupDatafieldsID int,  
	DataValue varchar(500),  
	ModifiedDate datetime,  
	SurveyGridID int,  
	EntryID uniqueidentifier  
	)  

	create table #c (cnt int)  
	  
	if lower(@ActionType) = 'unopen'  
	Begin  
		insert into #tempA  
		select distinct EmailID from EmailActivityLog el where BlastID = @BlastID and ActionTypeCode = 'open'   

		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@BlastID) + ' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '  
	end   
	else if lower(@ActionType) = 'unclick' -- Add all the other conditions below     
	Begin  
		insert into #tempA  
		 select distinct EmailID from EmailActivityLog el where BlastID = @BlastID and ActionTypeCode = 'click'   

		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@BlastID) + ' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '  
	End  
   
	if not exists (select top 1 GroupDatafieldsID from GroupDataFields where groupID = @GroupID) 
	Begin  
		exec ( ' insert into #c select count(Emails.EmailID) ' +   
		' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +   
		' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +  
		' and EmailGroups.GroupID = ' + @GroupID +  
		' ' + @Filter + ' ' +  @EmailString )  

		select @rowCountOut = isnull(cnt,0) from #c  

	End  
	Else/* if UDF's exists*/  
	Begin  
		 DECLARE @StandAloneUDFs VARCHAR(2000)
			SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
			DECLARE @TransactionalUDFs VARCHAR(2000)
			SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
			
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
							from	EmailDataValues edv  join  
									Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
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
						from	EmailDataValues edv  join  
								Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
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
	  
	  
	   exec('insert into #c select count(Emails.EmailID) ' +  
				' from Emails 
						join ' +       
		' EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +   
		' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +  
		' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString)  
	  
	  select @rowCountOut = isnull(cnt,0) from #c  
	    
	END  
  
	drop table #tempA  
	drop table #E  
	drop table #c  

END
