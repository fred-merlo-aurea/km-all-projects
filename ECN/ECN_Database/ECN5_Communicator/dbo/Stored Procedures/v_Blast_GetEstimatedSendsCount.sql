--USE [ECN5_COMMUNICATOR]
--GO
--/****** Object:  StoredProcedure [dbo].[v_Blast_GetEstimatedSendsCount]    Script Date: 04/01/2015 10:22:44 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
/***********************************************
-- Created:
--
-- Updated:
--	2014-03-21 updated to use temp tables for UDF pivot filters MK.
--
--
***********************************************/

CREATE proc [dbo].[v_Blast_GetEstimatedSendsCount]
(
	@customerID int,
	@xmlFormat text,
	@ignoreSuppression bit = 0
)
as
Begin

	declare @docHandle int,
			@minRowID int,
			@maxRowID int,
			@sqlstring varchar(max),
			@BuildUDFTempTablesString  varchar(max),
			@BuildTransTempTablesString  varchar(max),
			@Suppressionsqlstring varchar(max),
			@SuppressionGroupIDs   varchar(max),
			@masterSuppressionGroupID int,
			@basechannelID int,
			@FilterWhereclause varchar(max)	,
			--@emailcolumns varchar(max),
			@groupID int,
			@cmsExists bit,
			@gmsExists bit,
			@domainSuppressionExists bit

	DECLARE @StandAloneUDFs VARCHAR(MAX),
			@TransactionalUDFs VARCHAR(MAX),
			@StandAloneUDFIDs VARCHAR(MAX),
			@TransactionalUDFIDs VARCHAR(MAX),
			@StandAloneColumns VARCHAR(MAX),
			@TransactionalColumns VARCHAR(MAX),
			@StandAloneQuery VARCHAR(MAX),
			@TransactionalQuery VARCHAR(MAX)
			
	declare @SGminRowID int,
			@SGmaxRowID int,
			@SGsqlstring varchar(MAX),
			@SGgroupID int,
			@SGFilterWhereclause varchar(MAX),
			@SGBuildUDFTempTablesString varchar(MAX),
			@SGBuildTransTempTablesString varchar(MAX)

	set nocount on

	set @sqlstring = ''
	SET @BuildUDFTempTablesString =''
	SET @BuildTransTempTablesString =''

	set @SGsqlstring = ''
	set @SGBuildUDFTempTablesString = ''
	set @SGBuildTransTempTablesString = ''						

	set @Suppressionsqlstring = ''

	declare @gdf table(GID int, ShortName varchar(50))  
	declare @Group TABLE (RowID int not null  identity(1,1) primary key, GroupID int)  
	declare @GroupFilters TABLE (GroupID int, FilterID int)  
	declare @GroupSmartSegments TABLE (GroupID int,SmartSegmentID int, RefBlastIDs varchar(5000))  
	
	declare @SuppressionGroup TABLE (RowID int not null  identity(1,1) primary key, GroupID int)  
	declare @SuppressionGroupFilters  TABLE (GroupID int, FilterID int)
	declare @SuppressionGroupSmartSegments  TABLE (GroupID int, SmartSegmentID int, RefBlastIDs varchar(1000))

	select @masterSuppressionGroupID = groupID from Groups g where CustomerID = @CustomerID and MasterSupression = 1
	select @basechannelID = basechannelID from ecn5_accounts..Customer where customerID = @CustomerID

	if exists (select top 1 CMSID from ChannelMasterSuppressionList where BaseChannelID = @basechannelID)
		set @cmsExists = 1
	else 
		set @cmsExists  = 0

	if exists (select top 1 GSID from GlobalMasterSuppressionList)
		set @gmsExists = 1
	else 
		set @gmsExists  = 0
		
	if exists (select top 1 DomainSuppressionID from DomainSuppression  where BaseChannelID = @basechannelID or CustomerID = @CustomerID)
		set @domainSuppressionExists = 1
	else 
		set @domainSuppressionExists  = 0		
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlFormat  

	insert into @Group 
	(
		GroupID
	)  
	SELECT 
		distinct GroupID
	FROM OPENXML(@docHandle, N'/NoBlast/Group')   
	WITH   
	(  
		GroupID int '@id'
	) 
	
	insert into @GroupFilters 
	(
		GroupID, FilterID
	)  
	SELECT 
		distinct GroupID, FilterID
	FROM OPENXML(@docHandle, N'/NoBlast/Group/FilterID')   
	WITH   
	(  
		GroupID int '../@id', FilterID int '@id'
	) 
	
	insert into @GroupSmartSegments 
	(
		GroupID, SmartSegmentID, RefBlastIDs
	)  
	SELECT 
		distinct GroupID,  ssID, RefBlastIDs
	FROM OPENXML(@docHandle, N'/NoBlast/Group/SmartSegmentID')   
	WITH   
	(  
		GroupID int '../@id', ssID int '@id', RefBlastIDs varchar(5000) 'RefBlastIDs' 
	) 
	
	insert into @SuppressionGroup
	(
		GroupID
	)  
	SELECT  distinct GroupID
	FROM OPENXML(@docHandle, N'/NoBlast/SuppressionGroup/Group')   
	WITH   
	(  
		GroupID int '@id'
	)

	insert into @SuppressionGroupFilters
	(
		GroupID, FilterID
	)  
	SELECT  distinct GroupID, FilterID
	FROM OPENXML(@docHandle, N'/NoBlast/SuppressionGroup/Group/FilterID')   
	WITH   
	(  
		GroupID int '../@id',FilterID int '@id'
	)
	
	insert into @SuppressionGroupSmartSegments
	(
		GroupID, SmartSegmentID, RefBlastIDs
	)  
	SELECT distinct GroupID, ssID, RefBlastIDs
	FROM OPENXML(@docHandle, N'/NoBlast/SuppressionGroup/Group/SmartSegmentID')   
	WITH   
	(  
		GroupID int '../@id', ssID int '@id', RefBlastIDs varchar(1000) 'RefBlastIDs'
	)
	
	EXEC sp_xml_removedocument @docHandle   

	select @minRowID = min(RowID), @maxRowID = max(RowID) from @Group
	
	while @minRowID <= @maxRowID 
	begin
	
		DECLARE	@StandAloneQueryTmpTable VARCHAR(MAX)
		DECLARE	@TransactionalQueryTmpTable VARCHAR(MAX)

		set @FilterWhereclause = ''
	    --set @emailcolumns= ''
		SET @StandAloneQueryTmpTable = ''
		SET @TransactionalQueryTmpTable = ''

	    delete from @gdf
	    
		if @sqlstring = ''
			select	
					@groupID = groupID,
					@sqlstring = '
					select e1.emailID, e1.emailaddress from Emailgroups  with (NOLOCK) 
					join Emails e1  with (NOLOCK) on emailgroups.emailID = e1.emailID 
					where groupID = ' + CONVERT(varchar(20), groupID) + ' 
					and subscribetypecode=''S'' '
			from @group
			where rowID = @minRowID
		else
			select 
					@groupID = groupID,
					@sqlstring += ' 
					union 
					select  e1.emailID, e1.emailaddress  from Emailgroups  with (NOLOCK)  
					join Emails e1  with (NOLOCK) on emailgroups.emailID = e1.emailID   
					where groupID = ' + CONVERT(varchar(20), groupID)+ ' 
					and subscribetypecode=''S'' '
			from @group
			where rowID = @minRowID

		-- if smart segment filters exists for group		
		if exists (select top 1 1 from @GroupSmartSegments where GroupID = @groupID and SmartSegmentID > 0 and ISNULL(RefBlastIDs,'') <> '')
		Begin
			SELECT	@sqlstring = @sqlstring  + 
					CASE lower(ss.SmartSegmentName)
						WHEN 'unopen' THEN
							' and e1.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' + RefBlastIDs +  ')) and e1.EmailID not in (select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK) where BlastID in (' + RefBlastIDs +'))' 
						WHEN 'unclick' THEN
							' and e1.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' +  RefBlastIDs  + ')) and e1.EmailID not in (select EmailID from ecn_activity..BlastActivityClicks WITH (NOLOCK) where BlastID in (' + RefBlastIDs +'))'
						WHEN 'open' THEN
							' and e1.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK)  where BlastID in (' + RefBlastIDs +')) '
						WHEN 'click' THEN
							' and e1.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks WITH (NOLOCK)  where BlastID in (' + RefBlastIDs +')) '   
						WHEN 'suppressed' THEN
							' and e1.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.SuppressedCodes bc WITH (NOLOCK) on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + RefBlastIDs +') and SupressedCode in (''Threshold'')) ' 
						WHEN 'sent' THEN
							' and e1.EmailID in (select EmailID from ecn_activity.dbo.BlastActivitySends WITH (NOLOCK)  where BlastID in (' + RefBlastIDs +')) '  
						WHEN 'not sent' THEN
							' and e1.EmailID not in (select EmailID from ecn_activity.dbo.BlastActivitySends WITH (NOLOCK)  where BlastID in (' + RefBlastIDs +')) '  
					END
			from  
					@GroupSmartSegments gs JOIN SmartSegment ss on gs.SmartSegmentID = ss.SmartSegmentID
			where 
					GroupID = @groupID and gs.SmartSegmentID > 0 and ISNULL(gs.RefBlastIDs,'') <> ''
		End
		
		-- if regular filters exists for group	
		if exists (select top 1 1 from @GroupFilters where GroupID = @groupID and isnull(FilterID,0) > 0)
		Begin
			-- Add logic for Filter and appent to @sqlstring
			
			SELECT @FilterWhereclause = @FilterWhereclause + ' and ( ' + Convert(varchar(MAX),Whereclause) + ' ) '
			from  filter f WITH (NOLOCK) join @GroupFilters gf on  f.FilterID = gf.FilterID
			where f.GroupID = @groupID and isnull(f.IsDeleted,0) = 0
			
			--select @FilterWhereclause = Whereclause from filter WITH (NOLOCK) where FilterID = @FilterID and isnull(IsDeleted,0) = 0
			set @FilterWhereclause = RTRIM(LTRIM(@FilterWhereclause))
            set @FilterWhereclause = REPLACE(@FilterWhereclause, 'CONVERT(VARCHAR,', 'CONVERT(VARCHAR(255),');
			set @FilterWhereclause = REPLACE(@FilterWhereclause,'[emailaddress]','emailaddress')
				
			--select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
			--(			
			--select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' 
			--where c.name not in ('emailID','customerID','emailaddress')
			--and PATINDEX('%' + c.name + '%', @FilterWhereclause) > 0
			--) x

			insert into @gdf 
			select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g WITH (NOLOCK) join GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = g.GroupID      
						where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX('[' + ShortName + ']', @FilterWhereclause) > 0 and gdf.IsDeleted = 0     

			if not exists(select top 1 GID from @gdf)	
			Begin
				select @sqlstring += ' and e1.emailID in (
					 select e3.EmailID  from  Emails e3 with (NOLOCK) join EmailGroups eg3 with (NOLOCK) on eg3.EmailID = e3.EmailID          
					 where eg3.SubscribeTypeCode = ''S'' and eg3.GroupID = ' + CONVERT(varchar(20), @groupID) + ' ' + @FilterWhereclause +')'
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
				WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'

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
					SET @StandAloneQueryTmpTable =
							' 
							SELECT 
								*  
							INTO 
								#UdfValues_' + CONVERT(VARCHAR(50),@minRowID) + ' 
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
												
							CREATE INDEX tmp_IDX_Udf_EmailId_'+ CONVERT(VARCHAR(50),@minRowID) +' ON #UdfValues_'+ CONVERT(VARCHAR(50),@minRowID) +' (tmp_EmailID)
							'
					
					set @StandAloneQuery = ' left outer join  #UdfValues_'+ CONVERT(VARCHAR(50),@minRowID)+' udf ON Emails.emailID = UDF.tmp_EmailID '											

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

					SET @TransactionalQueryTmpTable = 
						' 
						SELECT 
							* 
						INTO 
							#TransactionalValues_' + CONVERT(VARCHAR(50),@minRowID) + ' 
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

						CREATE INDEX tmp_IDX_TransactionalValues_EmailId_'+ CONVERT(VARCHAR(50),@minRowID) +' ON #TransactionalValues_'+ CONVERT(VARCHAR(50),@minRowID) +' (tmp_EmailID)
						'
			
					set @TransactionalQuery = ' left outer join  #TransactionalValues_'+ CONVERT(VARCHAR(50),@minRowID) + ' tudf ON Emails.emailID = tudf.tmp_EmailID '											
/*					set @TransactionalQuery = '  left outer join (SELECT * 
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
								 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt) TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
*/
					
				End
				
				select @sqlstring += ' and e1.emailID in ( select distinct [Emails].EmailID from  Emails with (NOLOCK) ' +@StandAloneQuery + @TransactionalQuery + 
					' join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
					' where EmailGroups.SubscribeTypeCode = ''S''' +        
					' and EmailGroups.GroupID = ' + CONVERT(varchar(20), @GroupID) + ' ' + @FilterWhereclause + ')'  

			End
		End
		
		SET @BuildUDFTempTablesString = @BuildUDFTempTablesString + @StandAloneQueryTmpTable		
		SET @BuildTransTempTablesString = @BuildTransTempTablesString + @TransactionalQueryTmpTable
		
		set @minRowID = @minRowID + 1
	end			
					
	select @SGminRowID = min(RowID), @SGmaxRowID = max(RowID) from @SuppressionGroup
	if @ignoreSuppression <> 1
	begin
		while @SGminRowID <= @SGmaxRowID 
		begin
		
			DECLARE	@SGStandAloneQueryTmpTable VARCHAR(MAX)
			DECLARE	@SGTransactionalQueryTmpTable VARCHAR(MAX)

			set @SGFilterWhereclause = ''
			SET @SGStandAloneQueryTmpTable = ''
			SET @SGTransactionalQueryTmpTable = ''

			delete from @gdf
		    
			if @SGsqlstring = ''
				select	
						@SGgroupID = groupID,
						@SGsqlstring = '
						select SEG.emailID from Emailgroups SEG with (NOLOCK) 
						where groupID = ' + CONVERT(varchar(20), groupID) + ' and SEG.SubscribeTypeCode = ''S'''
				from @SuppressionGroup
				where rowID = @SGminRowID
			else
				select 
						@SGgroupID = groupID,
						@SGsqlstring += ' 
						union 
						select  SEG.emailID from Emailgroups SEG with (NOLOCK)  
						where groupID = ' + CONVERT(varchar(20), groupID) + ' and SEG.SubscribeTypeCode = ''S'''
				from @SuppressionGroup
				where rowID = @SGminRowID
			
			-- if regular filters exists for group	
			if exists (select top 1 1 from @SuppressionGroupFilters where GroupID = @SGgroupID and isnull(FilterID,0) > 0)
			Begin
				-- Add logic for Filter and appent to @SGsqlstring
				
				
				SELECT @SGFilterWhereclause = @SGFilterWhereclause + ' and ( ' + Convert(varchar(MAX),Whereclause) + ' ) '
				from  filter f WITH (NOLOCK) join @SuppressionGroupFilters gf on  f.FilterID = gf.FilterID
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
						
					SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
					WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH(''), root('MyString'),type).value('/MyString[1]','varchar(max)' ), 1, 2, '') + ']'

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
								' 
								SELECT 
									*  
								INTO 
									#SGUdfValues_' + CONVERT(VARCHAR(50),@SGminRowID) + ' 
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
													
								CREATE INDEX tmp_IDX_Udf_EmailId_'+ CONVERT(VARCHAR(50),@SGminRowID) +' ON #SGUdfValues_'+ CONVERT(VARCHAR(50),@SGminRowID) +' (tmp_EmailID)
								'
						
						set @StandAloneQuery = ' left outer join  #SGUdfValues_'+ CONVERT(VARCHAR(50),@SGminRowID)+' udf ON Emails.emailID = UDF.tmp_EmailID '											

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
								#SGTransactionalValues_' + CONVERT(VARCHAR(50),@SGminRowID) + ' 
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

							CREATE INDEX tmp_IDX_SGTransactionalValues_EmailId_'+ CONVERT(VARCHAR(50),@SGminRowID) +' ON #SGTransactionalValues_'+ CONVERT(VARCHAR(50),@SGminRowID) +' (tmp_EmailID)
							'
				
						set @TransactionalQuery = ' left outer join  #SGTransactionalValues_'+ CONVERT(VARCHAR(50),@SGminRowID) + ' tudf ON Emails.emailID = tudf.tmp_EmailID '											
				
					End
					
					select @SGsqlstring += ' and SEG.emailID in ( select distinct [Emails].EmailID from  Emails with (NOLOCK) ' +@StandAloneQuery + @TransactionalQuery + 
						' join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
						' where EmailGroups.GroupID = ' + CONVERT(varchar(20), @SGgroupID) + ' ' + @SGFilterWhereclause  + ')'

				End
			End
			
			-- if smart segment filters exists for group	
			-- sunil -- todo -- 3/30/2015 - do I really need groupID for suppression with smart segments?? @SuppressionGroupSmartSegments.GroupID 
			if exists (select top 1 1 from @SuppressionGroupSmartSegments where @SGgroupID > 0 and SmartSegmentID > 0 and ISNULL(RefBlastIDs,'') <> '')
			Begin
				SELECT	@SGsqlstring = @SGsqlstring  + 
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
								' and SEG.EmailID in (select Seg.emailID from emailgroups Seg join blast b on b.groupID = Seg.groupID where BlastID in (' + RefBlastIDs +') and Seg.emailID not in (select bas.EmailID from ecn_activity.dbo.BlastActivitySends bas WITH (NOLOCK)  where bas.BlastID in (' + RefBlastIDs +'))) '  
						END
				from  
						@SuppressionGroupSmartSegments gss JOIN SmartSegment ss on gss.SmartSegmentID = ss.SmartSegmentID
				where 
						gss.GroupID > 0 and gss.SmartSegmentID > 0 and ISNULL(gss.RefBlastIDs,'') <> ''
			End
			
			--Create table #SuppressionSmartSegment (EmailID int) ;
			
			SET @SGBuildUDFTempTablesString = @SGBuildUDFTempTablesString + @SGStandAloneQueryTmpTable		
			SET @SGBuildTransTempTablesString = @SGBuildTransTempTablesString + @SGTransactionalQueryTmpTable
			
			set @SGminRowID = @SGminRowID + 1
		end	
	end
	/* Handling Suppressions */
	if len(ltrim(rtrim(@SGsqlstring))) > 0 and @ignoreSuppression <> 1
	Begin
		select @SGsqlstring = '
				Create table #SuppressionSmartSegment (EmailID int) ;
				CREATE INDEX IDX_SuppressionSmartSegment_EmailID ON #SuppressionSmartSegment(EmailID);
				Insert into #SuppressionSmartSegment ' + @SGsqlstring + ';
		'
		set @Suppressionsqlstring = ' left outer join (select sss.emailID from #SuppressionSmartSegment sss) sss2 on x.emailID = sss2.emailID'
	End
	
	if (@ignoreSuppression <> 1) 
	Begin
		set @Suppressionsqlstring += ' left outer join (select eg1.emailID from emailgroups eg1 with (NOLOCK) where groupID in (' + Convert(varchar(20), @masterSuppressionGroupID) + ')) ms on x.emailID = ms.emailID '
		
		if @cmsExists = 1
			set @Suppressionsqlstring += ' left outer join channelmastersuppressionlist cms with (NOLOCK) on  cms.basechannelID = ' + Convert(varchar(20), @basechannelID) + ' and x.emailaddress = cms.emailaddress AND isnull(cms.IsDeleted,0) = 0 '

		if @gmsExists = 1
			set @Suppressionsqlstring += ' left outer join globalmastersuppressionlist gms with (NOLOCK) on x.emailaddress = gms.emailaddress AND isnull(gms.IsDeleted,0) = 0 '

		if @domainSuppressionExists = 1
			set @Suppressionsqlstring += ' left outer join DomainSuppression ds WITH (NOLOCK) ON (ds.CustomerID= ' + Convert(varchar(20), @customerID) + ' OR ds.BaseChannelID = ' + Convert(varchar(20), @basechannelID) + ') and RIGHT(x.EmailAddress, LEN(x.EmailAddress) - CHARINDEX(''@'', x.EmailAddress)) = ds.Domain AND isnull(ds.IsDeleted,0) = 0 '
	End
	
	set @Suppressionsqlstring += ' Where 1 = 1' --dbo.fn_ValidateEmailAddress(x.EmailAddress) = 1 
	
	if len(ltrim(rtrim(@SGsqlstring))) > 0
		set @Suppressionsqlstring += ' and sss2.emailID is null  '
	
	if (@ignoreSuppression <> 1) 
	Begin
		set @Suppressionsqlstring += ' and  ms.emailID is null  ' 
	
		if @cmsExists = 1
			set @Suppressionsqlstring += ' and cms.CMSID is null '

		if @gmsExists = 1
			set @Suppressionsqlstring += ' and gms.GSID is null '
			
		if @domainSuppressionExists = 1
			set @Suppressionsqlstring += ' and ds.DomainSuppressionID is null '
	End
	
	if len(ltrim(rtrim(@SGsqlstring))) > 0
		Begin
			set @Suppressionsqlstring += '; 
						 drop table #SuppressionSmartSegment; '
		End
	
	--select * from @group
	--select * from @GroupFilters
	--select * from @GroupSmartSegments
	
	--select * from @SuppressionGroup
	--select * from @SuppressionGroupFilters
	--select * from @SuppressionGroupSmartSegments
	
	--print '@SGBuildUDFTempTablesString : ' + @SGBuildUDFTempTablesString
	--print ''
	--print '@SGBuildTransTempTablesString : ' + @SGBuildTransTempTablesString
	--print ' '
	--print '@SGsqlstring : ' +  @SGBuildUDFTempTablesString + @SGBuildTransTempTablesString + @SGsqlstring	

	--PRINT '@BuildUDFTempTablesString : ' + @BuildUDFTempTablesString
	--print ' '
	--PRINT '@BuildTransTempTablesString : ' + @BuildTransTempTablesString
	--print ' '
	--PRINT '@sqlString : ' + @sqlString
	--print ' '
	
	PRINT (@SGBuildUDFTempTablesString + @SGBuildTransTempTablesString + @SGsqlstring + @BuildUDFTempTablesString + @BuildTransTempTablesString + ' select COUNT(x.emailID) from (' + @sqlString + ') x ' + @Suppressionsqlstring)
	
	EXEC (@SGBuildUDFTempTablesString + @SGBuildTransTempTablesString + @SGsqlstring + @BuildUDFTempTablesString + @BuildTransTempTablesString + ' select COUNT(x.emailID) from (' + @sqlString + ') x ' + @Suppressionsqlstring)

End

--
