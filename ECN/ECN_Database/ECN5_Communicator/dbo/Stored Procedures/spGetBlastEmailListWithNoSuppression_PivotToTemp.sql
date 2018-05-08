--Sp_helptext sp_FilterEmails_ALL_with_smartSegment 2850, 1053, 0, 'AND ( emails.EmailID in ( SELECT EmailID FROM EmailGroups WHERE GroupID = 2850 AND (CreatedOn between ''03/11/2008 00:00:00'' and ''03/11/2008 23:59:59'' or LastChanged between ''03/11/2008 00:00:00'' and ''03/11/2008 23:59:59'') ))', 0,'', '',0

CREATE  PROCEDURE [dbo].[spGetBlastEmailListWithNoSuppression_PivotToTemp]( 
	@BlastID int)
	--declare @BlastID int
	--set @BlastID = 584538
AS
BEGIN  
	
 	SET NOCOUNT ON

	DECLARE @GroupID int
	DECLARE @CustomerID int
	DECLARE @FilterID int
	DECLARE @Filter varchar(8000)
	DECLARE @ActionType varchar(10)
	DECLARE @refBlastID VARCHAR(2000)
	DECLARE @WhereClause varchar(8000)
	DECLARE @EmailString  varchar(8000) 
	
	set @FilterID = 0
	set @GroupID = 0
	set @CustomerID = 0
	set @Filter = ''
	set @ActionType = ''
	set @refBlastID = ''
	set @WhereClause = ''
	set @EmailString  = ''	
	
	select	@GroupID	=	groupID,
			@CustomerID =	customerID,
			@FilterID	=	(case	when isnull(filterID,0) = 2147483645 or isnull(filterID,0) = 2147483647 or isnull(filterID,0) = 2147483644 then 0 else isnull(filterID,0) end ) ,
			@ActionType =	(case	when isnull(filterID,0) = 2147483645 then 'unopen' 
									when isnull(filterID,0) = 2147483644 then 'suppressed'
									when isnull(filterID,0) = 2147483647 then 'unclick' 
									else '' end
							),
			@refBlastID = (case	when isnull(filterID,0) = 2147483645 or isnull(filterID,0) = 2147483647 or isnull(filterID,0) = 2147483644 then isnull(refBlastID,'') else '' end )
	from	
			[BLAST] 
	where
			BlastID = @BlastID

	if (@FilterID > 0)
	Begin
		select @WhereClause = convert (varchar(8000),WhereClause) from [FILTER] where FilterID = @FilterID
		if (LEN(@WhereClause) > 0)
		Begin
			select @Filter = ' and ( ' + @WhereClause + ')' from [FILTER] where FilterID = @FilterID 
		End		
	End
			
	create table #g(
		GID int,
		ShortName	varchar(50),
		DatafieldSetID	int
		)

	insert into #g 
	select GroupDatafieldsID, ShortName, DatafieldSetID from groupdatafields
	where GroupDatafields.groupID = @GroupID

	if lower(@ActionType) = 'unopen'
	Begin
		set @EmailString = @EmailString + ' and Emails.EmailID in (select bas.EmailID from ecn_Activity.dbo.BlastActivitySends bas WITH (NOLOCK) left outer join ecn_activity.dbo.BlastActivityOpens bao on bas.blastID = bao.blastID and bas.EmailID = bao.EmailID
  		 Where bas.BlastID in (' + @refBlastID +') and bao.OpenID is null) ' 	
	end
	else if lower(@ActionType) = 'unclick'
 	Begin

 		set @EmailString = @EmailString + ' and Emails.EmailID in (select bas.EmailID from ecn_Activity.dbo.BlastActivitySends bas WITH (NOLOCK) left outer join ecn_activity.dbo.BlastActivityClicks bac on bas.blastID = bac.blastID and bas.EmailID = bac.EmailID
  		 Where bas.BlastID in (' + @refBlastID +') and bac.ClickID is null) ' 	
  		
 	End       
  	else if lower(@ActionType) = 'softbounce'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivityBounces bab join ECN_ACTIVITY.dbo.BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID where BlastID = ' + convert(varchar(10),@BlastID) +   
      
            								' and BounceCode IN (''soft'', ''softbounce'')) '        
  	End  
    else if lower(@ActionType) = 'open'        
 	Begin        
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens  where BlastID in (' + @refBlastID +')) '        
  	end    
  	else if lower(@ActionType) = 'click'        
 	Begin        
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks  where BlastID in (' + @refBlastID +')) '         
  	end 
  	else if lower(@ActionType) = 'suppressed'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab join ECN_ACTIVITY.dbo.SuppressedCodes bc on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + @refBlastID +') and SupressedCode in (''Threshold'')) '       
  	End
  	
	if not exists (select GroupDatafieldsID from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID)  or not exists(select * from #g)
 	Begin
 		exec (	'select distinct EmailAddress ' +
				' from  Emails  WITH (NOLOCK) join EmailGroups  WITH (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' + 
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%'' ' +
				' and EmailGroups.GroupID = ' + @GroupID +
				' ' + @Filter + ' ' +  @EmailString)
	End
	Else/* if UDF's exists*/
 	Begin
 	--Print '1'
 		DECLARE @StandAloneUDFs VARCHAR(2000)
		DECLARE @TransactionalUDFs VARCHAR(2000)
		DECLARE @StandAloneQuery VARCHAR(4000)
		DECLARE @TransactionalQuery VARCHAR(4000)
		DECLARE @StandAloneTempQuery VARCHAR(4000)
		DECLARE @TransactionalTempQuery VARCHAR(4000)
		DECLARE @StandAloneDrop VARCHAR(500)
		DECLARE @TransactionalDrop VARCHAR(500)
			
		SET @StandAloneUDFs = ''
		SET @TransactionalUDFs = ''
		SET @StandAloneQuery  = ''
		SET @TransactionalQuery  = ''
		SET @standAloneTempQuery  = ''
		SET @TransactionalTempQuery  = ''		
		SET @StandAloneDrop  = ''
		SET @TransactionalDrop  = ''
 	
  		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND isnull(DatafieldSetID,0) > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		if LEN(@StandAloneUDFs) > 0
		Begin
			 set @standAloneTempQuery = '
						SELECT * into #tempStandAlone
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
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt;
						 
						 CREATE INDEX IDX_tempStandAlone_EmailID ON #tempStandAlone(tmp_EmailID);'
			 
			set @StandAloneQuery = ' left outer join #tempStandAlone SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'			 
			 
			set @StandAloneDrop  = 'drop table #tempStandAlone'
		End

		if LEN(@TransactionalUDFs) > 0
		Begin
			set @TransactionalTempQuery =  '
						SELECT * into #tempTransactional
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID1,  gdf.ShortName, edv.DataValue
							from	EmailDataValues edv  join  
									Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							where 
									gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and isnull(datafieldsetID,0) > 0
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt ;
						 
						 CREATE INDEX IDX_tempTransactional_EmailID ON #tempTransactional(tmp_EmailID1);'			
			
			set @TransactionalQuery = '  left outer join #tempTransactional TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
			
			set @TransactionalDrop  = 'drop table #tempTransactional'
		End
		
		exec (
			@StandAloneTempQuery + ';' + 
			@TransactionalTempQuery + ';' +
			' select distinct EmailAddress from Emails WITH (NOLOCK) ' + @StandAloneQuery + @TransactionalQuery + 
   				' join EmailGroups  WITH (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%'' ' +
				' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString + ';' +
				@StandAloneDrop + ';' + 
				@TransactionalDrop + ';  ' 
				)
			
 END

	drop table #G
END
