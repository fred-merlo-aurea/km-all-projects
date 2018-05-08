--Sp_helptext sp_FilterEmails_ALL_with_smartSegment 2850, 1053, 0, 'AND ( emails.EmailID in ( SELECT EmailID FROM EmailGroups WHERE GroupID = 2850 AND (CreatedOn between ''03/11/2008 00:00:00'' and ''03/11/2008 23:59:59'' or LastChanged between ''03/11/2008 00:00:00'' and ''03/11/2008 23:59:59'') ))', 0,'', '',0

CREATE  PROCEDURE [dbo].[spGetBlastEmailListWithNoSuppression]( 
	@CampaignItemID int,
	@BlastID int)
	--declare @BlastID int
	--set @BlastID = 608257
AS
BEGIN  
	
 	SET NOCOUNT ON

	DECLARE @GroupID int
	DECLARE @CustomerID int
	DECLARE @FilterID varchar(1000)
	DECLARE @Filter varchar(MAX)
	DECLARE @SmartSegmentID int
	DECLARE @ActionType varchar(MAX)
	DECLARE @refBlastID VARCHAR(2000)
	DECLARE @WhereClause varchar(MAX)
	DECLARE @EmailString  varchar(MAX) 
	declare @actionTypes table(SSID int, action varchar(50), refBlastIDs varchar(1000))
	declare @gdf table(GID int, ShortName varchar(50)) 
	DECLARE @SuppressionGroups table(RowID int not null  identity(1,1) primary key,GroupID int)
	DECLARE @SuppressionGroupsSmartSegments table(GroupID int, SSID int, RefBlastIDs varchar(100))
	DECLARE @SuppressionGroupsFilters table(GroupID int, FilterID int)
	DECLARE @StandAloneUDFs VARCHAR(MAX),
			@TransactionalUDFs VARCHAR(MAX),
			@StandAloneUDFIDs VARCHAR(MAX),
			@TransactionalUDFIDs VARCHAR(MAX),
			@StandAloneColumns VARCHAR(MAX),
			@TransactionalColumns VARCHAR(MAX),
			@StandAloneQuery VARCHAR(MAX),
			@TransactionalQuery VARCHAR(MAX)
	DECLARE @minSuppRowID int,
			@maxSuppRowID int
	declare @SGsqlstring varchar(MAX),
			@SGgroupID int,
			@SGFilterWhereclause varchar(MAX),
			@SGBuildUDFTempTablesString varchar(MAX),
			@SGBuildTransTempTablesString varchar(MAX),
			@Suppressionsqlstring varchar(max),
			@SGStandaloneDrop varchar(max),
			@SGTransactionalDrop varchar(max),
			@blastpriority int = 0

    create table #FinalList (EmailAddress varchar(255), BlastsAlreadySent varchar(500))
	delete from #FinalList
	create table #OtherSuppression (OrderID int, EmailID int, Reason varchar(50))  
	create table #g(	GID int,	ShortName	varchar(50),	DatafieldSetID	int	)
	create table #tempA (EmailID int)
	declare @blasttime datetime
	SELECT             
        @blastPriority = (case when mt.Priority = 1 then isnull(mt.SortOrder,0) else 0 end),
		@blasttime = b.SendTime
    FROM  
        Blast b WITH (NOLOCK)
        JOIN Layout l WITH (NOLOCK) ON b.LayoutID = l.LayoutID
        JOIN MessageType mt WITH (NOLOCK) ON l.MessageTypeID = mt.MessageTypeID
    WHERE 
        b.BlastID = @BlastID and
        b.StatusCode <> 'Deleted' and
        l.IsDeleted = 0 and
        mt.IsDeleted = 0
        
    CREATE UNIQUE CLUSTERED INDEX OtherSuppression_ind on #OtherSuppression(EmailID) with ignore_dup_key

	declare @cursorBlastID int
	declare @cursorBlastTime datetime
	declare ciCursor cursor
	FOR
		SELECT distinct b.BlastID, b.SendTime
		FROM Blast b with(nolock)
			join CampaignItemBlast cib with(nolock) on b.blastid = cib.BlastID
			join Layout l with(nolock) on b.layoutid = l.layoutid
			join MessageType mt with(nolock) on l.MessageTypeID = mt.MessageTypeID
		WHERE cib.CampaignItemID = @CampaignItemID and b.StatusCode <> 'Deleted' and 
				(
					(b.StatusCode='active'  and b.blastID <> @blastID) or
					(b.StatusCode='pending' AND mt.SortOrder < @blastPriority and b.SendTime < (SELECT CONVERT(VARCHAR(10),GETDATE(),111) + ' 23:59:59')) or
					(b.StatusCode='pending' AND b.sendtime < @blasttime) or
					(b.StatusCode='pending' AND b.sendtime = @blasttime and b.BlastID < @BlastID) 
				)
	OPEN ciCursor
	FETCH NEXT FROM ciCursor
	INTO @cursorBlastID,@cursorBlastTime
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		print 'blastid: ' + convert(varchar(10),@cursorblastid)
		set @FilterID = 0
		set @SmartSegmentID = 0
		set @GroupID = 0
		set @CustomerID = 0
		set @Filter = ''
		set @ActionType = ''
		set @refBlastID = ''
		set @WhereClause = ''
		set @EmailString  = ''	
	
		select	@GroupID	=	b.groupID,
				@CustomerID =	b.customerID
				--@FilterID	=	isnull(b.filterID,0) ,
				--@ActionType =	LOWER(ss.SmartSegmentName),
				--@refBlastID = (case	when isnull(b.SmartSegmentID,0) = 0 then isnull(b.refBlastID,'') else b.RefBlastID end )
		from	
				Blast b with (nolock)
			
		where
				b.BlastID = @cursorBlastID and
				b.StatusCode <> 'Deleted'

		select @FilterID = rtrim(STUFF(convert(varchar(10),cibf.FilterID) + ',', 1,0,''))
		from Blast b with(nolocK)
		join CampaignItemBlast cib with(nolock) on b.blastid = cib.blastid
		join CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID
		where b.BLastID = @cursorBlastID and b.StatusCode <> 'Deleted' and cibf.FilterID is not null and cibf.IsDeleted = 0

		SELECT @FilterID = SUBSTRING(@FilterID, 1, len(@FilterID) - 1)



		select @Filter = @Filter + t.WhereClause
				from 
				(
					  SELECT ' and (' + Convert(varchar(MAX),Whereclause) + ') ' as WhereClause
					  FROM filter WITH (NOLOCK) 
					  where FilterID in (SELECT CONVERT(int,t.Items) 
													from dbo.fn_Split(@FilterID,',') t)
				) t

      
		  --wgh: trimming where clause
		  set @Filter = RTRIM(ltrim(@Filter))

		  if @Filter <> ''
		  begin
				--set @filter = ' and (' + @filter + ') '
				set @Filter= REPLACE(@Filter, 'CONVERT(VARCHAR,', 'CONVERT(VARCHAR(255),');
		  end

		  set @Filter = replace(@Filter,'[emailaddress]','emailaddress')
		  set @Filter = replace(@Filter,'emailaddress','emails.emailaddress')
	  
		--Suppression lists

		
		SET @StandAloneUDFs = ''
		SET @TransactionalUDFs = ''
		SET @StandAloneUDFIDs = ''
		SET @TransactionalUDFIDs = ''
		SET @StandAloneColumns = ''
		SET @TransactionalColumns = ''
		SET @StandAloneQuery = ''
		SET @TransactionalQuery = ''



		SET @minSuppRowID = 0
		SET @maxSuppRowID = 0
		
		delete from @SuppressionGroups
		delete from @SuppressionGroupsFilters
		delete from @SuppressionGroupsSmartSegments

		insert into @SuppressionGroups(GroupID)
		Select cis.GroupID
		FRom Blast b with(nolock)
		join CampaignItemBlast cib with(nolock) on b.blastid = cib.BLastiD
		join CampaignItemSuppression cis with(nolock) on cib.CampaignItemID = cis.CampaignItemID
		where b.blastid = @cursorBlastID and b.StatusCode <> 'Deleted' and cis.IsDeleted = 0

		insert into @SuppressionGroupsSmartSegments(SSID, RefBlastIDs)
		Select cibf.SmartSegmentID, cibf.RefBlastIDs
		From Blast b with(nolock)
		join CampaignItemBlast cib with(nolock) on b.blastid = cib.BLastiD
		join CampaignItemSuppression cis with(nolock) on cib.CampaignItemID = cis.CampaignItemID
		join CampaignItemBlastFilter cibf with(nolock) on cis.CampaignItemSuppressionID = cibf.CampaignItemSuppressionID
		where b.blastid = @cursorBlastID and b.StatusCode <> 'Deleted' and cibf.SmartSegmentID is not null and cibf.IsDeleted = 0 and cis.IsDeleted = 0

		insert into @SuppressionGroupsFilters(GroupID, FilterID)
		Select cibf.SuppressionGroupID, cibf.FilterID
		From Blast b with(nolock)
		join CampaignItemBlast cib with(nolock) on b.blastid = cib.BLastiD
		join CampaignItemSuppression cis with(nolock) on cib.CampaignItemID = cis.CampaignItemID
		join CampaignItemBlastFilter cibf with(nolock) on cis.CampaignItemSuppressionID = cibf.CampaignItemSuppressionID
		where b.blastid = @cursorBlastID and b.StatusCode <> 'Deleted' and cibf.FilterID is not null and cis.IsDeleted = 0 and cibf.IsDeleted = 0

		
		 select @minSuppRowID = min(RowID), @maxSuppRowID = max(RowID) from @SuppressionGroups


				set @SGsqlstring = ''
				set @SGBuildUDFTempTablesString = ''
				set @SGBuildTransTempTablesString = ''                                  

				set @Suppressionsqlstring = ''

				while @minSuppRowID <= @maxSuppRowID
				Begin
					print 'into the while loop'
					  DECLARE     @SGStandAloneQueryTmpTable VARCHAR(MAX)
					  DECLARE     @SGTransactionalQueryTmpTable VARCHAR(MAX)
                        
					  delete from @gdf
                              
					  set @SGFilterWhereclause = ''
					  SET @SGStandAloneQueryTmpTable = ''
					  SET @SGTransactionalQueryTmpTable = ''
					  SET @SGStandaloneDrop = ''
					  SET @SGTransactionalDrop = ''
                        
					  if @SGsqlstring = ''
							select      
										@SGgroupID = groupID,
										@SGsqlstring = '
										INSERT INTO #OtherSuppression
										SELECT 3, SEG.EmailID, ''List''
								  FROM 
											  EmailGroups SEG with(nolock)
										where groupID = ' + CONVERT(varchar(20), groupID)
							from @SuppressionGroups
							where rowID = @minSuppRowID
					  else
							select 
										@SGgroupID = groupID,
										@SGsqlstring += ' 
										union 
										 SELECT 3, SEG.EmailID, ''List''
										FROM 
											  EmailGroups SEG with(nolock) 
										where groupID = ' + CONVERT(varchar(20), groupID)
							from @SuppressionGroups
							where rowID = @minSuppRowID
                              
					  if exists (select top 1 1 from @SuppressionGroupsFilters where GroupID = @SGgroupID and isnull(FilterID,0) > 0)
					  Begin
							-- Add logic for Filter and appent to @SGsqlstring
                        
							print 'creating filters'
							SELECT @SGFilterWhereclause = @SGFilterWhereclause + ' and ( ' + Convert(varchar(MAX),Whereclause) + ' ) '
							from  filter f WITH (NOLOCK) join @SuppressionGroupsFilters gf on  f.FilterID = gf.FilterID
							where f.GroupID = @SGgroupID and isnull(f.IsDeleted,0) = 0
                                          
							--select @SGFilterWhereclause = Whereclause from filter WITH (NOLOCK) where FilterID = @FilterID and isnull(IsDeleted,0) = 0
							set @SGFilterWhereclause = RTRIM(LTRIM(@SGFilterWhereclause))
							set @SGFilterWhereclause = REPLACE(@SGFilterWhereclause, 'CONVERT(VARCHAR,', 'CONVERT(VARCHAR(255),');
							set @SGFilterWhereclause = REPLACE(@SGFilterWhereclause,'[emailaddress]','emailaddress')
                        
							insert into @gdf 
							select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g WITH (NOLOCK) join GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = g.GroupID      
											  where  g.groupID = @SGgroupID and CHARINDEX('[' + ShortName + ']', @SGFilterWhereclause) > 0 and gdf.IsDeleted = 0     

							if not exists(select top 1 GID from @gdf) 
							Begin
								  select @SGsqlstring += ' and SEG.emailID in (
										select e3.EmailID  from  Emails e3 with (NOLOCK) join EmailGroups eg3 with (NOLOCK) on eg3.EmailID = e3.EmailID          
										 where eg3.GroupID = ' + CONVERT(varchar(20), @SGgroupID) + ' ' + @SGFilterWhereclause +')'
							End
							Else
							Begin
                              
								  SET @StandAloneUDFs = ''
								  SET @TransactionalUDFs = ''
								  SET @StandAloneUDFIDs = ''
								  SET @TransactionalUDFIDs  = ''
								  SET @StandAloneColumns  = ''
								  SET @TransactionalColumns = ''
								  SET @StandAloneQuery  = ''
								  SET @TransactionalQuery  = ''
								print 'creating filters'
								SELECT @SGFilterWhereclause = @SGFilterWhereclause + ' and ( ' + Convert(varchar(MAX),Whereclause) + ' ) '
								from  filter f WITH (NOLOCK) join @SuppressionGroupsFilters gf on  f.FilterID = gf.FilterID
								where f.GroupID = @SGgroupID and isnull(f.IsDeleted,0) = 0
	                              
								--select @SGFilterWhereclause = Whereclause from filter WITH (NOLOCK) where FilterID = @FilterID and isnull(IsDeleted,0) = 0
								set @SGFilterWhereclause = RTRIM(LTRIM(@SGFilterWhereclause))
								set @SGFilterWhereclause = REPLACE(@SGFilterWhereclause, 'CONVERT(VARCHAR,', 'CONVERT(VARCHAR(255),');
								set @SGFilterWhereclause = REPLACE(@SGFilterWhereclause,'[emailaddress]','emailaddress')
	                        
								insert into @gdf 
								select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g WITH (NOLOCK) join GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = g.GroupID      
												  where  g.groupID = @SGgroupID and CHARINDEX('[' + ShortName + ']', @SGFilterWhereclause) > 0 and gdf.IsDeleted = 0     

								if not exists(select top 1 GID from @gdf) 
								Begin
									  select @SGsqlstring += ' and SEG.emailID in (
											select e3.EmailID  from  Emails e3 with (NOLOCK) join EmailGroups eg3 with (NOLOCK) on eg3.EmailID = e3.EmailID          
											 where eg3.SubscribeTypeCode = ''S'' and eg3.GroupID = ' + CONVERT(varchar(20), @SGgroupID) + ' ' + @SGFilterWhereclause +')'
								End
								Else
								Begin

									  SET @StandAloneUDFs = ''
									  SET @TransactionalUDFs = ''
									  SET @StandAloneUDFIDs = ''
									  SET @TransactionalUDFIDs  = ''
									  SET @StandAloneColumns  = ''
									  SET @TransactionalColumns = ''
									  SET @StandAloneQuery  = ''
									  SET @TransactionalQuery  = ''
	                                    
									  SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
									  WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') , root('MyString'),type).value('/MyString[1]','varchar(max)'), 1, 2, '') + ']'

									  SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
									  WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

									  SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
									  WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'       

									  SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
									  WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
	                              
									  -- sunil - TODO - 03/30/2015 - exclude Standalone UDFs if not used in filter
									  if LEN(@StandAloneUDFs) > 0 
									  Begin
											set @StandAloneColumns = ' SAUDFs.* '

											--Create temp table for inner query filter on UDFs 
											SET @SGStandAloneQueryTmpTable =
														'SELECT 
															  *  
														INTO 
															  #SGUdfValues_' + CONVERT(VARCHAR(50),@minSuppRowID) + ' 
															   FROM
															  (
														SELECT 
															  edv.emailID as tmp_EmailID,  
															  gdf.ShortName, edv.DataValue           
														FROM 
															  EmailDataValues edv WITH (NOLOCK) 
															  join groupdatafields gdf WITH (NOLOCK) ON edv.groupdatafieldsID = gdf.groupdatafieldsID 
														WHERE
															  gdf.groupdatafieldsID IN (' + @StandAloneUDFIDs + ') 
															  AND gdf.IsDeleted = 0 ) u 
															  PIVOT (
															  MAX (DataValue)
																	FOR ShortName in ('+ @StandAloneUDFs+')) 
																	AS pvt
														'                             
														--CREATE INDEX tmp_IDX_Udf_EmailId_'+ CONVERT(VARCHAR(50),@minSuppRowID) +' ON #SGUdfValues_'+ CONVERT(VARCHAR(50),@minSuppRowID) +' (tmp_EmailID)
	                                    
	                                     
											set @StandAloneQuery = ' left outer join  #SGUdfValues_'+ CONVERT(VARCHAR(50),@minSuppRowID)+' udf ON Emails.emailID = UDF.tmp_EmailID '                                                                  
											set @SGStandaloneDrop += 'drop table #SGUdfValues_' + CONVERT(VARCHAR(50),@minSuppRowID) + ';'
									  End

									  -- sunil - TODO - 03/30/2015 - exclude Transactional UDFs if not used in filter
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

											SET @SGTransactionalQueryTmpTable = 
												  ' 
												  SELECT 
														* 
												  INTO 
														#SGTransactionalValues_' + CONVERT(VARCHAR(50),@minSuppRowID) + ' 
															   FROM
															  (
												  SELECT 
														edv.emailID as tmp_EmailID, 
														edv.entryID, 
														gdf.ShortName, 
														edv.DataValue
												  from  
														EmailDataValues edv WITH (NOLOCK)  join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
																	where 
														gdf.groupdatafieldsID in (' + @TransactionalUDFIDs + ') and gdf.IsDeleted = 0 
															   ) u

															  PIVOT
															  (
															  MAX (DataValue)
												  FOR ShortName in (' + @TransactionalUDFs + ')) as pvt

												  CREATE INDEX tmp_IDX_SGTransactionalValues_EmailId_'+ CONVERT(VARCHAR(50),@minSuppRowID) +' ON #SGTransactionalValues_'+ CONVERT(VARCHAR(50),@minSuppRowID) +' (tmp_EmailID)
												  '
	                        
											set @TransactionalQuery = ' left outer join  #SGTransactionalValues_'+ CONVERT(VARCHAR(50),@minSuppRowID) + ' tudf ON Emails.emailID = tudf.tmp_EmailID '                                                                  
											set @SGTransactionalDrop += 'drop #SGTransactionalValues_' + CONVERT(VARCHAR(50),@minSuppRowID) + ';'
									  End
	                                                      
									  select @SGsqlstring += ' and SEG.emailID in ( select distinct [Emails].EmailID from  Emails with (NOLOCK) ' +@StandAloneQuery + @TransactionalQuery + 
											' join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
											' where EmailGroups.GroupID = ' + CONVERT(varchar(20), @SGgroupID) + ' ' + @SGFilterWhereclause  + ')'
	                                    
								End
							END
					  End
                                    
					  -- if smart segment filters exists for group    
					  -- sunil -- todo -- 3/30/2015 - do I really need groupID for suppression with smart segments?? @SuppressionGroupSmartSegments.GroupID 
					  if exists (select top 1 1 from @SuppressionGroupsSmartSegments where @SGgroupID > 0 and SSID > 0 and ISNULL(RefBlastIDs,'') <> '')
					  Begin
							SELECT      @SGsqlstring = @SGsqlstring  + 
										CASE lower(ss.SmartSegmentName)
											  WHEN 'unopen' THEN
													' and SEG.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' + RefBlastIDs +  ') and e.EmailID not in (select bao.EmailID from ecn_activity.dbo.BlastActivityOpens bao WITH (NOLOCK) where BlastID in (' + RefBlastIDs +'))) ' 
											  WHEN 'unclick' THEN
													' and SEG.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' +  RefBlastIDs  + ') and e.EmailID not in (select bac.EmailID from ecn_activity..BlastActivityClicks bac WITH (NOLOCK) where BlastID in (' + RefBlastIDs +'))) '
											  WHEN 'open' THEN
													' and SEG.EmailID in (select bao.EmailID from ecn_activity.dbo.BlastActivityOpens bao WITH (NOLOCK)  where BlastID in (' + RefBlastIDs +')) '
											  WHEN 'click' THEN
													' and SEG.EmailID in (select bac.EmailID from ecn_activity.dbo.BlastActivityClicks bac WITH (NOLOCK)  where BlastID in (' + RefBlastIDs +')) '   
											  WHEN 'suppressed' THEN
													' and SEG.EmailID in (select bab.EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.SuppressedCodes bc WITH (NOLOCK) on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + RefBlastIDs +') and SupressedCode in (''Threshold'')) ' 
											  WHEN 'sent' THEN
													' and SEG.EmailID in (select bas.EmailID from ecn_activity.dbo.BlastActivitySends bas WITH (NOLOCK)  where BlastID in (' + RefBlastIDs +')) '  
											  WHEN 'not sent' THEN
													' and SEG.EmailID in (select Seg.emailID from emailgroups Seg join blasts b on b.groupID = eg.groupID where BlastID in (' + RefBlastIDs +') and eg.emailID not in (select bas.EmailID from ecn_activity.dbo.BlastActivitySends bas WITH (NOLOCK)  where bas.BlastID in (' + RefBlastIDs +')))) '  
										END
							from  
										@SuppressionGroupsSmartSegments gss JOIN SmartSegment ss on gss.SSID = ss.SmartSegmentID
							where 
										gss.GroupID > 0 and gss.SSID > 0 and ISNULL(gss.RefBlastIDs,'') <> ''
					  End
                              
                  
					  SET @SGBuildUDFTempTablesString = @SGBuildUDFTempTablesString + @SGStandAloneQueryTmpTable          
					  SET @SGBuildTransTempTablesString = @SGBuildTransTempTablesString + @SGTransactionalQueryTmpTable
                                    
					  set @minSuppRowID = @minSuppRowID + 1
					  print 'made it to the end'
					  print convert(varchar(10),@minsupprowid)
				end

				if len(ltrim(rtrim(@SGsqlstring))) > 0
				Begin
					  Print @SGBuildUDFTempTablesString
					  Print @SGBuildTransTempTablesString
					  Print  @SGsqlstring
					  Print @SGStandaloneDrop
					  Print @SGTransactionalDrop
                        

					  EXEC (@SGBuildUDFTempTablesString + @SGBuildTransTempTablesString + @SGsqlstring + @SGStandaloneDrop + @SGTransactionalDrop)
				End

		DELETE from #g

		insert into #g 
		select GroupDatafieldsID, ShortName, DatafieldSetID from groupdatafields WITH (NOLOCK)
		where GroupDatafields.groupID = @GroupID and IsDeleted = 0
	
		
		delete from #tempA
		delete from @actionTypes

		 INSERT INTO @actionTypes(SSID, action,refBlastIDs)
		 select cibf.SmartSegmentID,'', refBlastIDs
		 FROM Blast b with(nolock)
		 join CampaignItemBlast cib with(nolock) on b.blastid = cib.blastid
		 join CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID
		 where b.blastid = @cursorBlastID and b.StatusCode <> 'Deleted' and cibf.SmartSegmentID is not null
	 
		 update @actionTypes
		  set action = (Select SmartSegmentName from SmartSegment where SmartSegmentID = SSID)
		  --JWelter changing the way it looks for SS
      
		  if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'unopen'    )    
		  Begin        
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'unopen'
				exec('insert into #tempA select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK) where BlastID in (' + @refBlastID +') ') 
         
		  set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' + @refBlastID +        
															') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
		  end         
		  if  exists(select Top 1 * from @actionTypes a where lower(a.action) =  'unclick')                 
		  Begin        
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'unclick'
		  exec('insert into #tempA select EmailID from ecn_activity..BlastActivityClicks WITH (NOLOCK) where BlastID in (' + @refBlastID +') ') 

         
				set @EmailString = @EmailString +   ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' +  @refBlastID  +         
														  ') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
		  End        
		  if  exists(select Top 1 * from @actionTypes a where lower(a.action) = 'softbounce'  )             
		  Begin        
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'softbounce'
				set @EmailString = @EmailString +   ' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivityBounces bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.BounceCodes bc WITH (NOLOCK) on bab.BounceCodeID = bc.BounceCodeID where BlastID = ' + convert(varchar(10),@BlastID) +   
      
																' and BounceCode IN (''soft'', ''softbounce'')) '        
		  End  
		if  exists(select Top 1 * from @actionTypes a where lower(a.action) = 'open')
		 Begin        
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'open'
		  set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK)  where BlastID in (' + @refBlastID +')) '        
		  end    
		  if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'click')        
		  Begin     
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'click'   
				set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks WITH (NOLOCK)  where BlastID in (' + @refBlastID +')) '         
		  end 
		  if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'suppressed')               
		  Begin        
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'suppressed'  
				set @EmailString = @EmailString +   ' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.SuppressedCodes bc WITH (NOLOCK) on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + @refBlastID +') and SupressedCode in (''Threshold'')) '       
		  End
		  if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'sent')
		  BEGIN
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'sent'  
				set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @refBlastID + ')) '
		  END
		  if  exists(select Top 1 * from @actionTypes a where lower(a.action) = 'not sent')
		  BEGIN
				Set @refBlastID = ''
				Select @refBlastID = refBlastIDs from @actionTypes where action = 'not sent'  
				set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @refBlastID + ')) '
		  END
  	
		if not exists (select GroupDatafieldsID from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID and IsDeleted = 0)  or not exists(select * from #g)
 		Begin
 			exec (	'INSERT INTO #FinalList(EmailAddress, BlastsAlreadySent) ' +
					' select distinct EmailAddress, convert(varchar(15),' + @cursorBlastID + ') as BlastsAlreadySent ' +
					' from  Emails  WITH (NOLOCK) ' + 
					' join EmailGroups  WITH (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +
					' left outer join #OtherSuppression os on Emails.EmailID = os.EmailID ' +
					' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%'' ' +
					' and os.EmailID is null ' +
					' and EmailGroups.GroupID = ' + @GroupID +
					' ' + @Filter + ' ' +  @EmailString)
		End
		Else/* if UDF's exists*/
 		Begin
 		--Print '1'

			DECLARE @StandAloneTempQuery VARCHAR(MAX)
			DECLARE @TransactionalTempQuery VARCHAR(MAX)
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
 	
  			SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WITH (NOLOCK) WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'
			SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WITH (NOLOCK) WHERE GroupID = @GroupID AND isnull(DatafieldSetID,0) > 0 ORDER BY '],[' + ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'       

			if LEN(@StandAloneUDFs) > 0
			Begin
				 set @standAloneTempQuery = '
							SELECT * into #tempStandAlone
							 FROM
							 (
								SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
								from	EmailDataValues edv WITH (NOLOCK)  join  
										Groupdatafields gdf WITH (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
								where 
										gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null and gdf.IsDeleted = 0 
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
								from	EmailDataValues edv WITH (NOLOCK)  join  
										Groupdatafields gdf WITH (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
								where 
										gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and isnull(datafieldsetID,0) > 0 and gdf.IsDeleted = 0 
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
				' INSERT INTO #FinalList(EmailAddress, BlastsAlreadySent) ' +
				' select distinct EmailAddress, convert(varchar(15), ' + @cursorBlastID + ') as BlastsAlreadySent from Emails WITH (NOLOCK) ' + @StandAloneQuery + @TransactionalQuery + 
   					' join EmailGroups  WITH (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +
					' left outer join #OtherSuppression os on Emails.EmailID = os.EmailID ' +
					' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%'' ' +
					' and os.EmailID is null ' +
					' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString + ';' +
					@StandAloneDrop + ';' + 
					@TransactionalDrop + ';  ' 
					)


		END
					FETCH NEXT FROM ciCursor 
			INTO @CursorBlastID,@cursorBlastTime
	 END
	 Close ciCursor
	 DEallocate ciCursor

	 SELECT Distinct EmailAddress, STUFF((SELECT ',' + se2.BlastsAlreadySent
												FROM #FinalList se2
												WHERE fl.EmailAddress = se2.emailaddress
												FOR XML PATH('')),1,1,'')
	 FROM #FinalList fl
	 group by EmailAddress,fl.BlastsAlreadySent

	 drop table #FinalList
	 drop table #OtherSuppression
	 drop table #tempA 
	 drop table #G
END