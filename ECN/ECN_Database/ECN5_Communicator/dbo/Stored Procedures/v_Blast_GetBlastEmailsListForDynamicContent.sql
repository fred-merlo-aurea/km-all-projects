CREATE PROCEDURE [dbo].[v_Blast_GetBlastEmailsListForDynamicContent]
	@CustomerID int,        	
	@BlastID int,
	@GroupID int,        
	@FilterID varchar(MAX),        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(MAX),        
	@SupressionList varchar(MAX),
	@OnlyCounts bit,
	@LogSuppressed bit = 1
AS
	
BEGIN        
	--declare     @CustomerID int = 1,          
 --     @BlastID int = 1648721,
 --     @GroupID int = 418661,        
 --     @FilterID varchar(MAX) = '63580',        
 --     @BlastID_and_BounceDomain varchar(250) = '',        
 --     @ActionType varchar(MAX) = '<?xml version="1.0" encoding="utf-8"?><SmartSegments></SmartSegments>',        
 --     @SupressionList varchar(MAX) = '<?xml version="1.0" encoding="utf-8"?><SuppFilters><SuppressionGroup id="419009"></SuppressionGroup></SuppFilters>',
 --     @OnlyCounts bit = 0,
 --     @LogSuppressed bit = 0      
      set NOCOUNT ON    

      declare         
            @SqlString  varchar(MAX) = '',         
            @EmailString  varchar(MAX) = '',        
            @Col1 varchar(MAX) = '',        
            @Col2 varchar(MAX) = '',      
            @topcount varchar(10) = '',
            @emailcolumns varchar(2000) = '',
            @emailsubject varchar(1000) = '',
            @EmailFrom varchar(100) = '',
            @Filter varchar(max) = '',
            @layoutID int = 0,
            @TestBlast  varchar(5)  = '' ,
            @selectslotstr varchar(MAX) = '',
            @BasechannelID int = 0,
            @DynamicFromName  varchar(100) = '',       
            @DynamicFromEmail varchar(100) = '',      
            @DynamicReplyToEmail  varchar(100) = '',
            @CurrDate varchar(10) = '',
            @mailRoute varchar(100) = '',
            @customerproductthreshold int,
            @customerproductpriority int,
            @blastpriority int = 0,
            @IsMessageEnabledforThreshold bit = 0,
            @Domain varchar(100) = '',
            @OverrideAmount int = null,
            @OverrideIsAmount int = null,
            @AmountAlreadySent int = null,
            @BlastType varchar(50) = '',
            @SampleID int = NULL,
            @IsMTAPriority bit = 0,
            @thresholdlimit int = 0,
            @blasttime datetime,
            @DynamicSlotExists bit = 0,
            @UDFExists bit = 0,
            @sqlTopCount NVARCHAR(4000), 
            @CountToUse int,
            @CountToSuppress int,
            @CountSent int,
            @CountAvailable int,
            @WinningBlastID int = 0,
            @DynamicTagExists bit = 0,    
            @DT_startTag varchar(25) = 'ECN.DynamicTag.',         
            @DT_endTag varchar(25) = '.ECN.DynamicTag',
            @DynamicTagColumns varchar(MAX) = '',
            @IgnoreSuppression bit = 1,
            @minSuppRowID int,
            @maxSuppRowID int,
            @LosingBlastID int,
            @DidNotReceiveAB bit,
            @DeliveredOrOpened varchar(50),
			@SeedListCount int,
			@BlastCount int,
			@TotalToSendForSample int,
			@CountAvailableWOAlreadySent int,
			@SampleAlreadySent int,
			@ABlastLayoutID int,
			@sentInEAL int
            
      DECLARE @StandAloneUDFs VARCHAR(MAX),
                  @TransactionalUDFs VARCHAR(MAX),
                  @StandAloneUDFIDs VARCHAR(MAX),
                  @TransactionalUDFIDs VARCHAR(MAX),
                  @StandAloneColumns VARCHAR(MAX),
                  @TransactionalColumns VARCHAR(MAX),
                  @StandAloneQuery VARCHAR(MAX),
                  @TransactionalQuery VARCHAR(MAX)
                        
      set @CurrDate = convert(varchar, getdate(), 112)

     declare @gdf table(GID int, ShortName varchar(50))    
      
      declare @actionTypes table(SSID int, action varchar(50), refBlastIDs varchar(1000))
      
          

     create table #AlreadySent (EmailID int, Which varchar(50))  
      CREATE UNIQUE CLUSTERED INDEX AlreadySent_ind on #AlreadySent(EmailID, Which) with ignore_dup_key

	  create table #EmailActivityLog_Sent (EmailID int)  
      CREATE UNIQUE CLUSTERED INDEX AlreadySent_ind on #EmailActivityLog_Sent(EmailID) with ignore_dup_key
     
      create table #ToSend (EmailID int, ECNRandomID uniqueidentifier)  
      CREATE UNIQUE CLUSTERED INDEX ToSend_ind on #ToSend (EmailID) with ignore_dup_key
     
      create table #OtherSuppression (OrderID int, EmailID int, Reason varchar(50),BlastsAlreadySent varchar(MAX))  
      CREATE UNIQUE CLUSTERED INDEX OtherSuppression_ind on #OtherSuppression(EmailID) with ignore_dup_key
     
      create table #ThresholdSuppression (EmailAddress varchar(100),BlastsAlreadySent varchar(MAX))  
      CREATE UNIQUE CLUSTERED INDEX ThresholdSuppression_ind on #ThresholdSuppression(EmailAddress) with ignore_dup_key

      create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
      CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
      

      declare @allContent table (contentID int)
      declare @dynamicTags table (DynamicTagID int, Tag varchar(50), contentID int)

	  --select @SeedListCount = ISNULL(COUNT(EmailID),0) from EmailGroups eg with(nolock)
			--								 join Groups g with(nolock) on eg.groupid = g.groupid
			--								 where g.customerid = @CustomerID and g.IsSeedList = 1 and eg.SubscribeTypeCode = 'S'

     create table #tempA (EmailID int)  
            
      IF @BlastID <> 0
      BEGIN
      
            select 
                  @Domain = RIGHT(rtrim(ltrim(b.EmailFrom)), LEN(rtrim(ltrim(b.EmailFrom))) - CHARINDEX('@', rtrim(ltrim(b.EmailFrom)))),@OverrideAmount = b.OverrideAmount, 
                  @OverrideIsAmount = b.OverrideIsAmount, @testblast = b.TestBlast, @blasttime = b.sendtime, @DynamicFromName = b.DynamicFromName, 
                  @DynamicFromEmail = b.DynamicFromEmail, @DynamicReplyToEmail = b.DynamicReplyToEmail, @BlastType = b.BlastType, @SampleID = SampleID,
                  @emailsubject = EmailSubject, @EmailFrom = EmailFrom,  @layoutID = layoutID, @IgnoreSuppression = IsNull(IgnoreSuppression, 0)--@SupressionList = BlastSuppression
            from 
                  blast b with (nolock)
            where 
                  b.blastID = @BlastID and b.StatusCode <> 'Deleted'
                  
            if exists (select top 1 filterID from contentFilter WITH (NOLOCK) where layoutID = @layoutID and IsDeleted = 0)
                  set @DynamicSlotExists = 1
                  
            insert into @allContent
            select contentID
            from
            (
            select LayoutID, ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9 from Layout 
            where LayoutID  = @layoutID  and IsDeleted = 0
            ) x
            unpivot (contentID for slot in (ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9)) as unpvt
            where contentID > 0
            union
            select ContentID from ContentFilter where LayoutID = @layoutID and IsDeleted = 0
            
            insert into @dynamicTags
            select distinct DynamicTagID, dt.Tag, dt.ContentID from Content c join @allContent ac on c.ContentID = ac.contentID cross join DynamicTag dt 
            Where c.CustomerID = @CustomerID and dt.CustomerID = @CustomerID and c.IsDeleted = 0 and isnull(dt.IsDeleted,0) = 0 and 
            (PATINDEX('%' + @DT_startTag +dt.Tag+ @DT_endTag + '%', c.ContentSource) > 0 or   PATINDEX('%' + @DT_startTag +dt.Tag+ @DT_endTag + '%', c.ContentText) > 0 )         
            
            if exists (select top 1 contentID from @dynamicTags)
            Begin
                  set @DynamicTagExists = 1                                   

                  SELECT 
                     @DynamicTagColumns = STUFF( (SELECT ',' + CASe when dynamicwhere is null then convert(varchar(10), x.ContentID) else 'Case ' + dynamicwhere + ' else ' +  convert(varchar(10), x.ContentID) + ' END ' END + 
                     ' as [' + @DT_startTag + Tag + @DT_endTag +']'  from 
                  (
                  SELECT 
                    dt.*,
                    STUFF((
                        SELECT ' When ' + WhereClause + ' then '  + convert(varchar(10), dtr1.ContentID)
                        FROM @dynamicTags dt1 join DynamicTagRule dtr1 on dt1.DynamicTagID = dtr1.DynamicTagID join [rule] r1 on dtr1.RuleID = r1.RuleID
                        WHERE  dt1.DynamicTagID = dt.DynamicTagID  and dtr1.IsDeleted = 0  and r1.IsDeleted = 0
                        order by dt1.DynamicTagID, dtr1.Priority
                        FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
                    ,1,0,'') AS dynamicwhere
                  FROM @dynamicTags dt
                  GROUP BY dt.DynamicTagID, dt.Tag, dt.contentID) x 
                   FOR XML PATH('')), 
                  1, 0, '')
            
                  select @DynamicTagColumns = replace(replace(@DynamicTagColumns, '&gt;', '>'), '&lt;', '<')
            End
            
      END
            
      if rtrim(ltrim(@TestBlast)) = 'Y'
      Begin
            set @IsMTAPriority = 1
      End
      
      if exists (select Top 1 gdf.GroupDatafieldsID from GroupDataFields gdf WITH (NOLOCK) JOIN Groups g WITH (NOLOCK) on gdf.GroupID = g.GroupID where g.groupID = @GroupID and gdf.IsDeleted = 0)
            set @UDFExists = 1 
       
      --@thresholdlimit is the channel's threshold limit, 0 is unlimited
      select @basechannelID = c.basechannelID, @thresholdlimit = EmailThreshold from ecn5_accounts..customer c WITH (NOLOCK)   join 
                  ecn5_accounts..BaseChannel bc WITH (NOLOCK) on bc.BaseChannelID = c.BaseChannelID
      where customerID = @CustomerID and c.IsDeleted = 0 and bc.IsDeleted = 0
      

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

     if (@BlastID > 0 and @testblast <> 'Y')
     Begin
           INSERT INTO #AlreadySent
            SELECT 
                  EmailID, 'Current'
            FROM 
                  EmailActivityLog WITH (NOLOCK) 
            WHERE 
                  ActionTypeCode = 'send' AND --(ActionTypeCode = 'send' OR ActionTypeCode = 'suppressed') AND --JWelter only getting already sent emails without suppressed emails 10/9/2017
                  BlastID = @BlastID
                  
			INSERT INTO #EmailActivityLog_Sent
			SELECT 
                  EmailID
            FROM 
                  #AlreadySent WITH (NOLOCK) 
            
           --@customerProductThreshold > 0 = Threshold feature enabled
           SELECT 
                  @customerProductThreshold = count(sf.SFCode) 
            FROM 
						KMPlatform..ClientServiceFeatureMap csfm WITH(NOLOCK)
						JOIN KMPlatform..ServiceFeature sf WITH(NOLOCK) on csfm.ServiceFeatureID = sf.ServiceFeatureID
						JOIN KMPlatform..Service s WITH(NOLOCK) on sf.ServiceID = s.ServiceID
						JOIN ECN5_ACCOUNTS..Customer c with(nolock) on csfm.ClientID = c.PlatformClientID
            WHERE 
                        c.CustomerID = @customerID AND 
                        s.ServiceCode = 'EMAILMARKETING' AND 
                        sf.SFCode = 'SetMessageThresholds' AND 
                        csfm.IsEnabled = 1
   
            
            --@customerProductPriority > 0 = Priority feature enabled         
            SELECT 
                  @customerProductPriority = count(sf.SFCode) 
            FROM 
                        KMPlatform..ClientServiceFeatureMap csfm WITH(NOLOCK)
						JOIN KMPlatform..ServiceFeature sf WITH(NOLOCK) on csfm.ServiceFeatureID = sf.ServiceFeatureID
						JOIN KMPlatform..Service s WITH(NOLOCK) on sf.ServiceID = s.ServiceID
						JOIN ECN5_ACCOUNTS..Customer c with(nolock) on csfm.ClientID = c.PlatformClientID
            WHERE 
                        c.CustomerID = @customerID AND 
                        s.ServiceCode = 'EMAILMARKETING' AND 
                        sf.SFCode = 'SetMessagePriority' AND 
                        csfm.IsEnabled = 1
                        
                        
            --@IsMessageEnabledforThreshold - 1 = Message threshold enabled
            --@blastPriority - 1 = Message priority enabled
            SELECT      
                        @IsMessageEnabledforThreshold = Threshold,
                        @blastPriority = (case when mt.Priority = 1 then isnull(mt.SortOrder,0) else 0 end)
            FROM  
                        Blast b WITH (NOLOCK)
                        JOIN Layout l WITH (NOLOCK) ON b.LayoutID = l.LayoutID
                        JOIN MessageType mt WITH (NOLOCK) ON l.MessageTypeID = mt.MessageTypeID
            WHERE 
                        b.BlastID = @BlastID and
                        b.StatusCode <> 'Deleted' and
                        l.IsDeleted = 0 and
                        mt.IsDeleted = 0
                        
            --new logic from flowchart                      
            IF @customerProductThreshold > 0 AND @thresholdlimit > 0
            BEGIN
                  --Threshold feature enabled
                  
                  CREATE TABLE #sentemail (emailaddress VARCHAR(255), totalemail int default 1,BlastsAlreadySent varchar(MAX))          
                  
                 -- wgh - thresh was not working, think it's because we limit insert of this to 1 per email - CREATE UNIQUE CLUSTERED INDEX sentemail_ind on  #sentemail(emailaddress) with ignore_dup_key
                  
                  DECLARE @cursorCampaignItemID int
                  DECLARE @cursorBlastID int
                  
                  IF @customerProductPriority > 0
                  BEGIN
                        --Priority feature enabled
                        IF @IsMessageEnabledforThreshold > 0
                        BEGIN
                              --Message threshold enabled                                 
                              insert into #sentEmail(emailaddress,totalemail, BlastsAlreadySent)
                              exec spGetThresholdSuppressedEmails @BlastID, 'N'
                              
                              IF @blastPriority > 0
                              BEGIN
                                    --Message priority enabled so do threshold & Priority
                                    DECLARE db_cursor CURSOR FOR              
                                    SELECT distinct  ci.CampaignItemID
                                    FROM CampaignItem ci with(nolock)
										  JOIN CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID
										  JOIN Blast b   WITH (NOLOCK) on cib.BlastID = b.BlastID
                                          JOIN ecn5_accounts..Customer c   WITH (NOLOCK) ON b.CustomerID = c.customerID
                                          JOIN Layout l   WITH (NOLOCK) on b.LayoutID = l.LayoutID
                                          JOIN MessageType mt   WITH (NOLOCK) on l.MessageTypeID = mt.MessageTypeID
                                    WHERE 
                                          c.BaseChannelID = @basechannelID AND
                                          b.TestBlast = 'N' AND
                                          b.StatusCode <> 'Deleted' AND
                                          c.IsDeleted = 0 AND
                                          l.IsDeleted = 0 AND
                                          mt.IsDeleted = 0 AND
                                          mt.Priority = 1 AND
                                          (
                                                (b.StatusCode='active' and b.blastID <> @blastID) or
                                                (b.StatusCode='pending' AND mt.SortOrder < @blastPriority and b.SendTime < (SELECT CONVERT(VARCHAR(10),GETDATE(),111) + ' 23:59:59')) or
                                                (b.StatusCode='pending' AND b.sendtime < @blasttime AND mt.SortOrder = @blastPriority) or
                                                (b.StatusCode='pending' AND b.sendtime = @blasttime and b.BlastID < @BlastID AND mt.SortOrder = @blastPriority) 
                                          )
                                                            
                                    OPEN db_cursor   
                                    FETCH NEXT FROM db_cursor INTO @cursorCampaignItemID

                                    WHILE @@FETCH_STATUS = 0   
                                    BEGIN   
                                             insert into #sentEmail (emailaddress,BlastsAlreadySent)
                                             EXEC spGetBlastEmailListWithNoSuppression @cursorCampaignItemID, @BlastID

                                                --select 'Active/Pending Email suppressed : ', @cursorBlastID, getdate(), @@ROWCOUNT 

                                             FETCH NEXT FROM db_cursor INTO @cursorCampaignItemID
                                    END 
                                    CLOSE db_cursor   
                                    DEALLOCATE db_cursor
                              END
                              ELSE
                              BEGIN
                                    --Message priority NOT enabled so do Threshold ONLY
                                    DECLARE db_cursor CURSOR FOR              
                                    SELECT distinct ci.CampaignItemID
                                    FROM CampaignItem ci with(nolock)
										  JOIN CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID
										  JOIN Blast b WITH (NOLOCK) on cib.blastid = b.BlastID
                                          JOIN ecn5_accounts..Customer c  WITH (NOLOCK) ON b.CustomerID = c.customerID
                                    WHERE 
                                          c.BaseChannelID = @basechannelID AND
                                          b.TestBlast = 'N' AND
                                          b.StatusCode <> 'Deleted' AND
                                          c.IsDeleted = 0 AND
                                          (
                                                (b.StatusCode='active'  and b.blastID <> @blastID) or
                                                (b.StatusCode='pending' AND b.sendtime < @blasttime) or
                                                (b.StatusCode='pending' AND b.sendtime = @blasttime and b.BlastID < @BlastID) 
                                          )
                                                            
                                    OPEN db_cursor   
                                    FETCH NEXT FROM db_cursor INTO @cursorCampaignItemID

                                    WHILE @@FETCH_STATUS = 0   
                                    BEGIN   
                                             insert into #sentEmail (emailaddress,BlastsAlreadySent)
                                             EXEC spGetBlastEmailListWithNoSuppression @cursorCampaignItemID, @BlastID

                                                --select 'Active/Pending Email suppressed : ', @cursorBlastID, getdate(), @@ROWCOUNT 

                                             FETCH NEXT FROM db_cursor INTO @cursorCampaignItemID
                                    END 
                                    CLOSE db_cursor   
                                    DEALLOCATE db_cursor 
                              END                     
                        END               
                  END
                  ELSE
                  BEGIN
                        --Priority feature NOT enabled so do Threshold ONLY
                        insert into #sentEmail(emailaddress,totalemail, BlastsAlreadySent)
                        exec spGetThresholdSuppressedEmails @BlastID, 'Y'
                  
                        DECLARE db_cursor CURSOR FOR              
                         SELECT distinct ci.CampaignItemID
                         FROM CampaignItem ci with(nolock)
							JOIN CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID
							JOIN Blast b WITH (NOLOCK) on cib.blastid = b.BlastID
							JOIN ecn5_accounts..Customer c  WITH (NOLOCK) ON b.CustomerID = c.customerID
                        WHERE 
                              c.BaseChannelID = @basechannelID AND
                              b.TestBlast = 'N' AND
                              b.StatusCode <> 'Deleted' AND
                              c.IsDeleted = 0 AND 
                              (
                                    (b.StatusCode='active'  and b.blastID <> @blastID) or
                                    (b.StatusCode='pending' AND b.sendtime < @blasttime) or
                                    (b.StatusCode='pending' AND b.sendtime = @blasttime and b.BlastID < @BlastID) 
                              )
                                                
                        OPEN db_cursor   
                        FETCH NEXT FROM db_cursor INTO @cursorBlastID

                        WHILE @@FETCH_STATUS = 0   
                        BEGIN   
                                 insert into #sentEmail (emailaddress,BlastsAlreadySent)
                                 EXEC spGetBlastEmailListWithNoSuppression @cursorCampaignItemID, @BlastID

                                    --select 'Active/Pending Email suppressed : ', @cursorBlastID, getdate(), @@ROWCOUNT 

                                 FETCH NEXT FROM db_cursor INTO @cursorBlastID
                        END 
                        CLOSE db_cursor   
                        DEALLOCATE db_cursor 
                  
                  END   
                  
                  INSERT INTO #ThresholdSuppression(EmailAddress, BlastsAlreadySent)
                  SELECT  se.emailaddress,STUFF((SELECT ',' + se2.BlastsAlreadySent
												FROM #sentemail se2
												WHERE se.EmailAddress = se2.emailaddress
												FOR XML PATH('')),1,1,'')
                  FROM #sentemail se 
                  left outer join ChannelNoThresholdList ntl  WITH (NOLOCK) on se.emailaddress = ntl.emailaddress and ntl.basechannelID = @basechannelID and ntl.IsDeleted = 0
                  WHERE ntl.emailaddress is null 
                  GROUP BY se.emailaddress
                  HAVING SUM(totalemail) >= @thresholdlimit 
                  
                  DROP TABLE #sentemail
                  
            END                     
                  
      End   

      -- insert threshold, 7 day, list, mslist, cmslist, domain (upto counttoUse)
      --1. Threshold Suppression    
      IF @IgnoreSuppression <> 1
      BEGIN
		print 'threshold suppression'
            INSERT INTO #OtherSuppression
            SELECT      
                        1, e.EmailID, 'Threshold', ts.BlastsAlreadySent
            FROM  
                        Emails e WITH (NOLOCK)
                              JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID
                              JOIN #ThresholdSuppression ts WITH (NOLOCK) ON e.EmailAddress = ts.EmailAddress 
            WHERE       
                        eg.GroupID = @GroupID
      END
      --2. 7 Day Suppression  
      IF (@TestBlast <> 'Y' AND lower(@ActionType) <> 'softbounce' and @IgnoreSuppression <> 1)             
      Begin   
            IF    @groupID <> 61640 and -- CBR Rental
                  @groupID <> 35894 and -- NEEBO Rental Transactions
                  @groupID <> 126097 and -- NEEBO Closed Wareshouse  Group
                  @groupID <> 126098 and -- NEEBO Closed  inMarket  Group
                  @CustomerID <> 2808 and --Apogee Retail
                  @CustomerID <> 3090 and -- Apogee Retail NY, LLC
                  @CustomerID <> 3137-- and --Neebo Rental Program
                  --@CustomerID <> 1941 -- KM Master Lists. -- removed KM Master Lists per Raisa on 12/30/2013
            BEGIN
                  INSERT INTO #OtherSuppression
                  SELECT      
                              2, eal.EmailID, '7Day',''
                  FROM  
                              EmailActivityLog eal WITH (NOLOCK)
                                    JOIN Blast B WITH (NOLOCK) ON eal.BlastID = B.BlastID
                  WHERE       
                              LayoutID = @LayoutID AND 
							  b.BlastID <> @BlastID and -- fixed on 03/27/2018 by sunil - we should not consider the current blast (if it is restarted)
                              SendTime > dateadd(d, -7 , @blasttime) AND
                              ActionTypeCode = 'send' and
                              B.StatusCode <> 'Deleted'
            END
      END
      DECLARE @SuppressionGroups table(RowID int not null  identity(1,1) primary key,GroupID int)
      --3. List Suppression   

      if CHARINDEX('SuppressionGroup', @SupressionList) > 0 and @IgnoreSuppression <> 1
      Begin

            --Sample XML for @SuppressionGroups
            --<SuppFilters>
            --    <SupppressionGroup id="1">
            --          <ssID id="1">
            --                <refBlastIDs>1,2,4</refBlastIDs>
            --          </ssID>
            --          <FilterIDs>11,22,33</FilterIDs>
            --    </SuppressionGroup>
            --    <SuppressionGroup id="2">
            --          <ssID id="2">
            --                <refBlastIDs>1</refBlastIDs>
            --          </ssID>
            --          <ssID id="4">
            --                <refBlastIDs>5</refBlastIDs>
            --          </ssID>
            --          <FilterIDs>44,55</FilterIDs>
            --    </SuppressionGroup>
            --</SuppFilters
            print 'starting suppressed groups'
            DECLARE @docHandleSupp int
            EXEC sp_xml_preparedocument @docHandleSupp OUTPUT, @SupressionList  
            INSERT INTO @SuppressionGroups(GroupID)
            SELECT SuppGroupID
            FROM OPENXML(@docHandleSupp, N'/SuppFilters/SuppressionGroup')
            WITH
            (
                  SuppGroupID int '@id'
            )
            
            DECLARE @SuppressionGroupsSmartSegments table(GroupID int, SSID int, RefBlastIDs varchar(100))
            INSERT INTO @SuppressionGroupsSmartSegments(GroupID, SSID, RefBlastIDs)
            SELECT SuppGroupID, SSID, Refblasts
            FROM OPENXML(@docHandleSupp, N'/SuppFilters/SuppressionGroup/ssID')
            WITH
            (
                  SuppGroupID int '../@id',SSID int '@id', Refblasts varchar(500) 'refBlastIDs'
            )
            DECLARE @SuppressionGroupsFilters table(GroupID int, FilterID int)
            INSERT INTO @SuppressionGroupsFilters(GroupID, FilterID)
            SELECT SuppGroupID, FilterIDs
            FROM OPENXML(@docHandleSupp, N'/SuppFilters/SuppressionGroup/FilterID')
            WITH
            (
                  SuppGroupID int '../@id',FilterIDs varchar(500) '@id'
            )
            exec sp_xml_removedocument @docHandleSupp

            
            select @minSuppRowID = min(RowID), @maxSuppRowID = max(RowID) from @SuppressionGroups
            declare @SGsqlstring varchar(MAX),
                        @SGgroupID int,
                        @SGFilterWhereclause varchar(MAX),
                        @SGBuildUDFTempTablesString varchar(MAX),
                        @SGBuildTransTempTablesString varchar(MAX),
                        @Suppressionsqlstring varchar(max),
                        @SGStandaloneDrop varchar(max),
                        @SGTransactionalDrop varchar(max)

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
                                    SELECT 3, SEG.EmailID, ''List'',''''
									FROM 
                                          EmailGroups SEG with(nolock)
                                    where SubscribeTypeCode = ''S'' and groupID = ' + CONVERT(varchar(20), groupID)
                        from @SuppressionGroups
                        where rowID = @minSuppRowID
                  else
                        select 
                                    @SGgroupID = groupID,
                                    @SGsqlstring += ' 
                                    union 
                                    SELECT 3, SEG.EmailID, ''List'',''''
                                    FROM 
                                          EmailGroups SEG with(nolock) 
                                    where SubscribeTypeCode = ''S'' and groupID = ' + CONVERT(varchar(20), groupID)
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
                                                ' and SEG.EmailID in (select Seg.emailID from emailgroups Seg join blast b on b.groupID = Seg.groupID where BlastID in (' + RefBlastIDs +') and Seg.emailID not in (select bas.EmailID from ecn_activity.dbo.BlastActivitySends bas WITH (NOLOCK)  where bas.BlastID in (' + RefBlastIDs +'))) '  
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
      END
      --4. Group Suppression  
      IF @IgnoreSuppression <> 1
      BEGIN
            INSERT INTO #OtherSuppression
            SELECT      
                        4, eg.EmailID, 'Group',''
            FROM  
                        EmailGroups eg WITH (NOLOCK)
                              JOIN GROUPS g WITH (NOLOCK) ON eg.GroupID = g.GroupID
            WHERE       
                        g.CustomerID = @CustomerID AND 
                        g.MasterSupression=1
      END
                  
      --5. Channel Suppression      
      IF @IgnoreSuppression <> 1
      BEGIN
            INSERT INTO #OtherSuppression
            SELECT      
                        5, e.EmailID, 'Channel',''
            FROM  
                        Emails e WITH (NOLOCK)
                              JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
                              JOIN ChannelMasterSuppressionList cms WITH (NOLOCK) ON e.EmailAddress = cms.EmailAddress
            WHERE       
                        eg.GroupID = @GroupID AND
                        cms.BaseChannelID = @BasechannelID and
                        cms.IsDeleted = 0
      END
      
      --6. Domain Suppression 
      IF @IgnoreSuppression <> 1
      BEGIN
            INSERT INTO #OtherSuppression
            SELECT      
                        6, e.EmailID, 'Domain',''
            FROM  
                        Emails e WITH (NOLOCK)
                              JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
                              JOIN ecn5_accounts..Customer c WITH (NOLOCK) on e.CustomerID = c.CustomerID
                              JOIN DomainSuppression ds WITH (NOLOCK) ON RIGHT(e.EmailAddress, LEN(e.EmailAddress) - CHARINDEX('@', e.EmailAddress)) = ds.Domain AND (e.CustomerID = ds.CustomerID OR c.BaseChannelID = ds.BaseChannelID)   
            WHERE       
                        eg.GroupID = @GroupID and c.IsDeleted = 0 and ds.IsDeleted = 0
      END

      --7. Global Suppression 
      IF @IgnoreSuppression <> 1
      BEGIN
            INSERT INTO #OtherSuppression
            SELECT      
                        5, e.EmailID, 'Global',''
            FROM  
                        Emails e WITH (NOLOCK)
                              JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
                              JOIN GlobalMasterSuppressionList gms WITH (NOLOCK) ON e.EmailAddress = gms.EmailAddress
            WHERE       
                        eg.GroupID = @GroupID AND
                        gms.IsDeleted = 0
      END

      --8. Invalid Email
      INSERT INTO #OtherSuppression
      SELECT      
                  1, e.EmailID, 'InvalidEmail',''
      FROM  
                  Emails e WITH (NOLOCK)
                        JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID
      WHERE       
                  eg.GroupID = @GroupID AND
                  dbo.fn_ValidateEmailAddress(Emailaddress) = 0

    --if sample, suppress the sends from the other sample
      if @BlastType = 'Sample'
      Begin
            declare @MAXEAID int
            Select @MAXEAID = MAXEAID from ECN_Activity..ActivityLogIDSync where TargetTable = 'Send'
            
			declare @OtherSample int
			Select @OtherSample = BlastID,@ABlastLayoutID = LayoutID from Blast WITH (NOLOCK) where blastID <> @BlastID and SampleID = @SampleID and BlastType = 'Sample' and StatusCode <> 'Deleted'
			insert into #AlreadySent
            select bas.emailID, 'Other' 
            from ECN_ACTIVITY..BlastActivitySends bas WITH (NOLOCK) 
			join Blast b with(nolock) on bas.blastid = b.BlastID
			join EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and b.GroupID = eg.GroupID			
            where bas.blastID = @OtherSample                  
                  and bas.EAID <= @MAXEAID
            UNION
            Select eal.EmailID, 'Other'
            FROM EmailActivityLog eal with(nolock)
			join Blast b with(nolock) on eal.blastid = b.BlastID
			join EmailGroups eg with(nolock) on eal.EmailID = eg.EmailID and b.GroupID = eg.GroupID			
            WHERE eal.BlastID = @OtherSample
            and eal.EAID > @MAXEAID and eal.ActionTypeCode = 'send'
			
			if @ABlastLayoutID = @layoutID
			BEGIN
				select @SampleAlreadySent = COUNT(*) from #AlreadySent s where s.Which = 'Other'
			END
			ELSE
			BEGIN
				SET @SampleAlreadySent = 0
			END
      
      End
      
            --if champion, suppress the sends from the winning blast id
      --insert logic here to handle sending champion to losing A/B emails
      if @BlastType = 'Champion'
      Begin

            select @WinningBlastID = WinningBlastID, @DidNotReceiveAB = DidNotReceiveAB, @DeliveredOrOpened = DeliveredOrOpened from [Sample] WITH (NOLOCK) where SampleID = @SampleID and IsDeleted = 0
            select @LosingBlastID = BlastID from [Blast] b with(nolock) where b.SampleID = @SampleID and b.BlastID <> @WinningBlastID and b.BlastType = 'sample'

            
            if(@DidNotReceiveAB = 1 and ISNULL(@DeliveredOrOpened,'') <> '')--Handles if both DidNotReceiveAB and DeliveredOrOpened have been selected
            BEGIN
                    if(@DeliveredOrOpened = 'delivered')--Only send to people who didn't receive A or B but if they received the losing blast only people that it was delivered to
                    BEGIN
                              --suppress send with no bounce for winner
                              INSERT INTO #AlreadySent
                              select bas.EmailID, 'Other'
                              FROM ECN_ACTIVITY..BlastActivitySends bas with(nolock)
                              left outer JOIN ECN_ACTIVITY..BLastActivityBounces bab with(nolock) on bas.BlastID = bab.BLastiD and bas.EmailID = bab.EmailID
                              WHERE (bas.BlastID in(@WinningBlastID) and bab.BounceID is null)

                              --suppress bounced of loser
                              INSERT INTO #AlreadySent
                              select bas.EmailID, 'Other'
                              FROM ECN_ACTIVITY..BlastActivitySends bas with(nolock)
                              left outer JOIN ecn_activity..BlastActivityBounces bab with(nolock) on bas.BlastID = bab.BlastID and bas.EmailID = bab.EmailID
                              WHERE bas.BlastID = @LosingBlastID and bab.BounceID is not null
                              
                    END
                    else if(@DeliveredOrOpened = 'opened')--Only send to people who didn't receive A or B but if they received the losing blast only people that opened it
                    BEGIN
                              --suppress send with no bounce for winner
                              INSERT INTO #AlreadySent
                              select bas.EmailID, 'Other'
                              FROM ECN_ACTIVITY..BlastActivitySends bas with(nolock)
                              left outer JOIN ECN_ACTIVITY..BLastActivityBounces bab with(nolock) on bas.BlastID = bab.BLastiD and bas.EmailID = bab.EmailID
                              WHERE bas.BlastID = @WinningBlastID and bab.BounceID is null
                  
                              --suppress no open of loser
                              INSERT INTO #AlreadySent
                              select bas.EmailID, 'Other'
                              FROM ECN_ACTIVITY..BlastActivitySends bas with(nolock)
                              left outer JOIN ECN_ACTIVITY..BlastActivityOpens bao with(nolock) on bas.BlastID = bao.BLastiD and bas.EmailID = bao.EmailID
                              WHERE (bas.BlastID = @LosingBlastID and bao.OpenID is null)             

                    END
            END
            else if(@DidNotReceiveAB = 1 and ISNULL(@DeliveredOrOpened, '') = '')--Only DidNotReceiveAB was selected
            BEGIN
                  --suppress send with no bounce for loser and winner
                  insert into #AlreadySent
                  select bas.emailID, 'Other' 
                  from ECN_ACTIVITY..BlastActivitySends bas WITH (NOLOCK) 
                  join EmailGroups eg on bas.EmailID = eg.EmailID and eg.SubscribeTypeCode = 'S'
                  left outer JOIN ECN_ACTIVITY..BLastActivityBounces bab with(nolock) on bas.BlastID = bab.BLastiD and bas.EmailID = bab.EmailID
                  where bas.blastID in( @WinningBlastID, @LosingBlastID) and bab.BounceID is null
            END
            else if(@DidNotReceiveAB = 0 and ISNULL(@DeliveredOrOpened,'') <> '')--Only DeliveredOrOpened was selected
            BEGIN
                  if(@DeliveredOrOpened = 'delivered')--they selected to only send to people who received the losing blast and it was delivered
                  BEGIN
                              --suppress anyone who didn't receive(no send or send but no bounce) the Loser
                              INSERT INTO #AlreadySent
                              select eg.EmailID, 'Other'
                              FROM EmailGroups eg with(nolock)
                              where eg.EmailID not in (select bas.EmailID 
                                                                  from ECN_ACTIVITY..BlastActivitySends bas with(nolock) 
                                                                  left outer join ecn_activity..BlastActivityBounces bab with(nolock) on bas.BlastID = bab.BlastID and bas.EmailID = bab.EmailID
                                                                  where bas.BlastID  = @LosingBlastID and bab.BounceID is null-- were doing a 'not in' so i am selecting the emails that we want to send to here
                                                                  )
                              and eg.GroupID = @GroupID and eg.SubscribeTypeCode = 'S'

                  END
                  else if(@DeliveredOrOpened = 'opened')--they selected to only send to people who received the losing blast and they opened it
                  BEGIN
                        --suppress anyone who didn't open the Loser
                        INSERT INTO #AlreadySent
                        select eg.EmailID, 'Other'
                        FROM EmailGroups eg with(nolock)
                        where eg.EmailID not in (select bas.EmailID 
                                                            from ECN_ACTIVITY..BlastActivitySends bas with(nolock) 
                                                            left outer join ecn_activity..BlastActivityOpens bao with(nolock) on bas.BlastID = bao.BlastID and bas.EmailID = bao.EmailID
                                                            where bas.BlastID  = @LosingBlastID and bao.OpenID is not null-- were doing a 'not in' so i am selecting the emails that we want to send to here
 )           
                        and eg.GroupID = @GroupID and eg.SubscribeTypeCode = 'S'
                  END  
            End
      End
      delete from @gdf
     if @BlastID = 0 
      begin       
            if LEN(@Filter) > 0 and  @UDFExists = 1        
            Begin
                  insert into @gdf         
                  select distinct gdf.GroupDatafieldsID, gdf.ShortName 
                  from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
                  where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0 and gdf.IsDeleted = 0         
            end   
     end        
      else        
      begin  
            select @MailRoute = m.MTAName from MTA m WITH (NOLOCK) join MTACustomer mc WITH (NOLOCK) on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain
            if(@MailRoute is null or LEN(@MailRoute) < 1)
            Begin
                  select @MailRoute = m.MTAName from MTA m WITH (NOLOCK) join MTACustomer mc WITH (NOLOCK) on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'
            End
                        
            if @DynamicSlotExists = 1 or @DynamicTagExists = 1 or @blasttype = 'PERSONALIZATION'
            Begin
            
                  select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
                  (select convert(varchar(100),c.name) as columnName from sys.syscolumns c join sys.sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
                  
                  if @UDFExists = 1   
                        insert into @gdf         
                        select GroupDatafieldsID, ShortName from groupdatafields WITH (NOLOCK) where GroupDatafields.groupID = @GroupID and IsDeleted = 0  
            end
            else
            Begin       
                  select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
                  (
                  select distinct emailColumns.columnName 
                    from  blast WITH (NOLOCK) join         
                        Layout WITH (NOLOCK) on Blast.layoutID = Layout.layoutID left outer join        
                        Content WITH (NOLOCK) on  Content.ContentID = Layout.ContentSlot1 or         
                           Content.ContentID = Layout.ContentSlot2 or         
                           Content.ContentID = Layout.ContentSlot3 or         
                           Content.ContentID = Layout.ContentSlot4 or         
                           Content.ContentID = Layout.ContentSlot5 or         
                           Content.ContentID = Layout.ContentSlot6 or         
                           Content.ContentID = Layout.ContentSlot7 or         
                           Content.ContentID = Layout.ContentSlot8 or         
                           Content.ContentID = Layout.ContentSlot9 left outer join        
                        Template WITH (NOLOCK) on Layout.TemplateID  = Template.TemplateID and Template.IsDeleted = 0 left outer join        
                        contentFilter WITH (NOLOCK) on (contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID) and ContentFilter.IsDeleted = 0 cross join
                        ( select convert(varchar(100),c.name) as columnName from sys.syscolumns c join sys.sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
                    where         
                        Blast.BlastID = @BlastID and 
                        blast.StatusCode <> 'Deleted' and
                        Layout.IsDeleted = 0 and
                        Content.IsDeleted = 0 and
                        (
                              PATINDEX('%[$][$]' +emailColumns.columnName+ '[$][$]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[$][$]' +emailColumns.columnName+ '[$][$]%', Content.ContentText) > 0 or   
                              PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%', Content.ContentText) > 0 or   
                              PATINDEX('%[#][#]' +emailColumns.columnName+ '[#][#]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[#][#]' +emailColumns.columnName+ '[#][#]%', Content.ContentTExt) > 0 or                                 
                              PATINDEX('%' +emailColumns.columnName+ '%', contentFilter.whereclause) > 0 or   
                              PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%',  Template.TemplateSource) > 0 or   
                              PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%',  Template.TemplateText) > 0 or
                              PATINDEX('%' +emailColumns.columnName+ '%', @Filter) > 0 or
                              PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%',  @emailsubject) > 0 
                        )
                  ) emailColumns

                  if(LEN(@Filter) > 0)
                  begin
                        insert into @gdf         
                        select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g WITH (NOLOCK) join GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = g.GroupID      
                        where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0 and gdf.IsDeleted = 0     
                        union        
                        select distinct gdf.GroupDatafieldsID, gdf.ShortName      
                        from  blast WITH (NOLOCK) join         
                        Layout WITH (NOLOCK) on Blast.layoutID = Layout.layoutID left outer join        
                        Content WITH (NOLOCK) on  Content.ContentID = Layout.ContentSlot1 or         
                           Content.ContentID = Layout.ContentSlot2 or         
                           Content.ContentID = Layout.ContentSlot3 or         
                           Content.ContentID = Layout.ContentSlot4 or         
                           Content.ContentID = Layout.ContentSlot5 or         
                           Content.ContentID = Layout.ContentSlot6 or         
                           Content.ContentID = Layout.ContentSlot7 or         
                           Content.ContentID = Layout.ContentSlot8 or         
                           Content.ContentID = Layout.ContentSlot9 left outer join        
                        Template WITH (NOLOCK) on Layout.TemplateID  = Template.TemplateID and Template.IsDeleted = 0 left outer join        
                        contentFilter WITH (NOLOCK) on (contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID) and contentFilter.IsDeleted = 0  join        
                        Groups WITH (NOLOCK) on Groups.GroupID = blast.groupID and Groups.CustomerID = Blast.CustomerID join         
                        GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = Groups.GroupID         
                    where         
                        Blast.BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and                    
                        blast.StatusCode <> 'Deleted' and
                        Layout.IsDeleted = 0 and
                        Content.IsDeleted = 0 and
                        gdf.IsDeleted = 0 and
                        (
                              PATINDEX('%sort="' +ShortName+ '"%', Content.ContentSource) > 0 or           
                              PATINDEX('%filter_field="' +ShortName+ '"%', Content.ContentSource) > 0 or   
   PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentTExt) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentTExt) > 0 or   
                              PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentTExt) > 0 or                              
                              PATINDEX('%' +ShortName+ '%', contentfilter.whereclause) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateSource) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateText) > 0  or
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%',  @emailsubject) > 0  or 
                              PATINDEX('%' +ShortName+ '%',  @DynamicFromName) > 0 or 
                              PATINDEX('%' +ShortName+ '%',  @DynamicFromEmail) > 0 or 
                              PATINDEX('%' +ShortName+ '%',  @DynamicReplyToEmail) > 0
                        )   
                  end
                  else
                  begin
                        insert into @gdf         
            --          select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g WITH (NOLOCK) join GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = g.GroupID      
                        --where  g.groupID = @GroupID and g.customerID = @CustomerID and gdf.IsDeleted = 0     
            --          union        
                        select distinct gdf.GroupDatafieldsID, gdf.ShortName      
                        from  blast WITH (NOLOCK) join         
                        Layout WITH (NOLOCK) on Blast.layoutID = Layout.layoutID left outer join        
                        Content WITH (NOLOCK) on  Content.ContentID = Layout.ContentSlot1 or         
                           Content.ContentID = Layout.ContentSlot2 or         
                           Content.ContentID = Layout.ContentSlot3 or         
                           Content.ContentID = Layout.ContentSlot4 or         
                           Content.ContentID = Layout.ContentSlot5 or         
                           Content.ContentID = Layout.ContentSlot6 or         
                           Content.ContentID = Layout.ContentSlot7 or         
                           Content.ContentID = Layout.ContentSlot8 or         
                           Content.ContentID = Layout.ContentSlot9 left outer join        
                        Template WITH (NOLOCK) on Layout.TemplateID  = Template.TemplateID and Template.IsDeleted = 0 left outer join        
                        contentFilter WITH (NOLOCK) on (contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID) and contentFilter.IsDeleted = 0  join        
                        Groups WITH (NOLOCK) on Groups.GroupID = blast.groupID and Groups.CustomerID = Blast.CustomerID join         
                        GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = Groups.GroupID         
                    where         
                        Blast.BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and                    
                        blast.StatusCode <> 'Deleted' and
                        Layout.IsDeleted = 0 and
                        Content.IsDeleted = 0 and
                        gdf.IsDeleted = 0 and
                        (
                              PATINDEX('%sort="' +ShortName+ '"%', Content.ContentSource) > 0 or           
                              PATINDEX('%filter_field="' +ShortName+ '"%', Content.ContentSource) > 0 or   
                       PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentTExt) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentTExt) > 0 or  
                              PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentSource) > 0 or   
                              PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentTExt) > 0 or                               
                              PATINDEX('%' +ShortName+ '%', contentfilter.whereclause) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateSource) > 0 or   
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateText) > 0  or
                              PATINDEX('%[%][%]' +ShortName+ '[%][%]%',  @emailsubject) > 0  or 
                              PATINDEX('%' +ShortName+ '%',  @DynamicFromName) > 0 or 
                              PATINDEX('%' +ShortName+ '%',  @DynamicFromEmail) > 0 or 
                              PATINDEX('%' +ShortName+ '%',  @DynamicReplyToEmail) > 0
                        )   
                  end
            
           end        
      end
      --JWelter adding this for multiple SS support 03042015
      --Sample XML for SmartSegments
      --<SmartSegments>
      --    <ssID id="1">
      --          <refBlastIDs>1,2,3</refBlastIDs>
      --    </ssID>
      --    <ssID id="2">
      --          <refBlastIDs>1,2,4</refBlastIDs>
      --    </ssID>
      --</SmartSegments>
      declare @docHandle int
      EXEC sp_xml_preparedocument @docHandle OUTPUT, @ActionType  
      INSERT INTO @actionTypes(SSID, action,refBlastIDs)
      SELECT SS, '',RefBlasts
      FROM OPENXML(@docHandle, N'/SmartSegments/ssID')   
      WITH   
      (  
            SS int '@id', RefBlasts varchar(1000) 'refBlastIDs'
      ) 
      exec sp_xml_removedocument @docHandle

      update @actionTypes
      set action = (Select SmartSegmentName from SmartSegment where SmartSegmentID = SSID)
      --JWelter changing the way it looks for SS
      DECLARE @refBlastID varchar(1000)
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
            set @EmailString = @EmailString +   ' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivityBounces bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.BounceCodes bc WITH (NOLOCK) on bab.BounceCodeID = bc.BounceCodeID where BlastID = 
' + convert(varchar(10),@BlastID) +   
      
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

	  --need to get split type for evenly split blasts
      declare @SplitType varchar(1) = ''
	  select @SplitType = ISNULL(SplitType,'') from BlastSchedule bs with(nolock)
									join Blast b with(nolock) on bs.BlastScheduleID = b.BlastScheduleID
									where b.BlastID = @BlastID
      if @BlastType <> 'Champion'
      BEGIN
      SELECT      @CountSent = COUNT(EmailID) FROM #AlreadySent a where a.Which = 'current'
      END
      Else if @BlastType = 'Champion' 
      BEGIN
		select @CountSent = 0
		if exists(select top 1 * from #EmailActivityLog_Sent)
		BEGIN
			select @CountSent = COUNT(distinct EmailID) from #EmailActivityLog_Sent
		END
      END
	  else if @SplitType = 'e'
	  BEGIN
		select @CountSent = 0
	  END

      SET         @CountToUse = 0  
	  declare @TestBlastLimit int

      --no UDFs   
      IF @UDFExists = 0 or not exists(select top 1 GID from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
      BEGIN

            --get all emails based on blast           
            EXEC ( 'insert into #ToSend  ' + 
            ' select Emails.EmailID, newID()  from  Emails with (NOLOCK) join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
            ' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
            ' and EmailGroups.GroupID = ' + @GroupID +        
            ' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID 
            ')

            IF @BlastType = 'Sample' 
			BEGIN
				--for sample we want to include already sent emails so we can get the right amount
				SELECT @CountAvailable = COUNT(ts.EmailID) + @SampleAlreadySent
				FROM #ToSend ts					
					LEFT OUTER JOIN #OtherSuppression os on ts.EmailID = os.EmailID
				WHERE 
					os.EmailID is null
			END
			ELSE IF @SplitType in( 'e','m') -- need to get full count of emails in group for evenly split blasts
			BEGIN
				SELECT @CountAvailable = COUNT(ts.EmailID) 
				FROM #ToSend ts									
					left outer join #OtherSuppression os on ts.EmailID = os.EmailID and os.Reason <> '7Day'
				WHERE 
					os.EmailID is null
			END
			ELSE
			BEGIN
				SELECT @CountAvailable = COUNT(ts.EmailID) 
				FROM #ToSend ts
					LEFT OUTER JOIN #AlreadySent al on ts.EmailID = al.EmailID and al.Which <> 'current'
					LEFT OUTER JOIN #OtherSuppression os on ts.EmailID = os.EmailID --and os.Reason <> '7Day'
				WHERE 
					al.EmailID is null 
					and os.EmailID is null
					
            END
            
            --if amount is % get actual number for @OverRideIsAmount
			IF(@OverRideIsAmount is not null and @OverRideIsAmount = 0) 
			BEGIN
				  SET @OverrideAmount = CEILING((convert(decimal(15,2),@CountAvailable) * convert(decimal(15,2),@OverrideAmount)) / 100)
				  IF @OverrideAmount = 0
				  BEGIN
						SET @OverrideAmount = 1
				  END
			END
            
            --use @OverrideAmount or @CountAvailable
            IF @OverrideAmount > 0
            BEGIN
				SET @CountToUse = @OverrideAmount
				
				SELECT @sentInEAL = COUNT(*) from #EmailActivityLog_Sent
				if ISNULL(@sentInEAL,0) > 0 and @sentInEAL < @CountToUse
				BEGIN
					SET @CountToUse = @CountToUse - @sentInEAL
				END
				--Commenting out below for future dev --  JWelter 4/28/2017
                --Special case for AB Blasts where the total to send for both A and B is greater than what is available to send when @OverrideIsAmount = 1
				--When this happens we will split the count available evenly across A and B, This code takes in to account that in the future
				--We might allow A,B,C,D blasts -- JWelter 4/28/2017
				--IF @BlastType = 'AB' and @OverrideIsAmount = 1
				--BEGIN
					
				--	select @BlastCount = COUNT(*) 
				--	from Blast b 
				--	where b.SampleID = @SampleID and b.BlastType = 'Sample'
					
				--	SET @TotalToSendForSample = @BlastCount * @OverrideAmount
					
				--	IF @CountAvailable < @TotalToSendForSample
				--	BEGIN
						
						
				--		SET @CountToUse = CEILING(convert(decimal(15,2),@CountAvailable) / convert(decimal(15,2),@BlastCount))
						
				--	END
				--	ELSE
				--	BEGIN
				--		SET @CountToUse = @OverrideAmount
				--	END
				--END
				--ELSE
				--BEGIN
    --              SET @CountToUse = @OverrideAmount
    --            END
            END
            ELSE
            BEGIN
				
           
                  IF @CountAvailable - @CountSent < 0
                  BEGIN
					
                        SET @CountToUse = 0
                  END
                  ELSE
                  BEGIN

                        SET @CountToUse = @CountAvailable - @CountSent

                  END
            END

			
		  --Getting test blast limit here
			if(@TestBlast = 'Y')
			BEGIN

				select @TestBlastLimit = ISNULL(TestBlastLimit,0) from ECN5_Accounts..Customer c where c.CustomerID = @CustomerID
				if(@TestBlastLimit = 0)
				BEGIN
					select @TestBlastLimit = ISNULL(TestBlastLimit,0) from ECN5_Accounts..BaseChannel bc where bc.BaseChannelID = @BasechannelID
					if(@TestBlastLimit = 0)
					BEGIN
						SET @CountToUse = 10
					END
					ELSE
					BEGIN
						SET @CountToUse = @TestBlastLimit
					END
				END
				ELSE
				BEGIN
					SET @CountToUse = @TestBlastLimit
				END

			END

            --get full records
            IF @OnlyCounts = 0      
            BEGIN
                  
                  --log suppressed
				  if @BlastType = 'Sample'
				  BEGIN
						SET @CountToSuppress = @CountToUse - @CountAvailable + @SampleAlreadySent
						--SELECT 
						--	@CountToSuppress = COUNT(ts.EmailID) - @CountAvailable + @SampleAlreadySent
						--FROM 
						--	#ToSend ts
						--	join #OtherSuppression os on ts.EmailID = os.EmailID 
						--	left outer join #AlreadySent a on os.EmailID = a.EmailID
						--Where a.EmailID is null

						if @CountToSuppress > @CountToUse
						BEGIN
							SET @CountToSuppress = @CountToUse
						END
				  END
				  ELSE IF @SplitType in('e','m') 
				  BEGIN				  
				      SELECT @CountAvailable = COUNT(ts.EmailID)
					  from #ToSend ts 
						left outer join #OtherSuppression os on ts.EmailID = os.EmailID
					  where os.EmailID is null
				  
					  SELECT @CountToSuppress = @CountToUse - @CountAvailable					
				  END
				  ELSE
				  BEGIN
						SELECT 
							@CountToSuppress = COUNT(ts.EmailID) - @CountAvailable
						FROM 
							#ToSend ts
                  END           

                  IF @CountToSuppress > 0
                  BEGIN
                        IF @LogSuppressed = 1
                        BEGIN 
                              -- order to pull ascending threshold, 7 day, list, mslist, cmslist, domain (upto counttoUse)
                              exec ( 'INSERT into EmailActivityLog '+
                              ' select TOP ' + @CountToSuppress + ' os.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), os.Reason, os.BlastsAlreadySent, ''Y'' '+
                              ' from #ToSend ts '+
                                    ' JOIN #OtherSuppression os ON ts.EmailID = os.EmailID '+
                                    ' LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID '+
                              ' WHERE  '+
                                    ' al.EmailID IS NULL '+                         
                              ' order by os.OrderID ASC')
                        END
                  END
      
                  --no dynamic content    
                  IF @DynamicSlotExists = 0 OR @BlastID = 0
                  BEGIN     
					if @blasttype = 'PERSONALIZATION'
					Begin  
                        EXEC ( 'select top ' + @CountToUse + ' Emails.EmailID,'''+@BlastID+''' as BlastID,'''+@mailRoute+''' as mailRoute, Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
                        ' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
                        '''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
                        '''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +   
                        ', ' + @IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID, isnull(pc.PersonalizedContentID,0) as  PersonalizedContentID ' +     
                        @DynamicTagColumns + ' ' +
                        ' from  #ToSend ts ' +
                                    ' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
									' left outer join ecn5_content..PersonalizedContent pc with (NOLOCK) on pc.blastID = ' + @blastID + ' and pc.emailaddress = Emails.emailaddress and isValid = 1 and isdeleted = 0 ' +
                                    ' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +
                                    ' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
                                    ' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
                                    ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null' +
                                    ' ' + @Filter + ' order by ts.ECNRandomID')     
					END
					ELSE
					BEGIN
                        EXEC ( 'select top ' + @CountToUse + ' Emails.EmailID,'''+@BlastID+''' as BlastID,'''+@mailRoute+''' as mailRoute, Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
                        ' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
                        '''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
                        '''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +   
                        ', ' + @IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID ' +     
                        @DynamicTagColumns + ' ' +
                        ' from  #ToSend ts ' +
                                    ' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
                                    ' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +
                                    ' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
                                    ' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
                                    ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null' +
                                    ' ' + @Filter + ' order by ts.ECNRandomID')   
					END
                  END
                  --dynamic content
                  ELSE
                  BEGIN
                        SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 

                              EXEC ( @selectslotstr + @DynamicTagColumns + ' ' + ' from ( select top ' +  @CountToUse + ' Emails.EmailID,'''+ @BlastID +''' as BlastID, '''+@mailRoute+''' as mailRoute,  Emails.EmailAddress, CustomerID, ' + @emailcolumns + 

                              ' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
                              '''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' +  @BlastID + ''' as ConversionTrkCDE, ' +        
                              '''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +  
                              ', ' + @IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID ' +          
                              ' from #ToSend ts ' +
                              ' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
                              ' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +
                              ' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
                              ' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
                              ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' ) inn2 order by inn2.ECNRandomID')  
                  END
            END
            --just get count
            ELSE
            BEGIN
                  --SELECT @CountToUse    -- 7/17/2013 - WGH: Need to use suppression and already sent in count
                  
                  SELECT
                        case when @CountToUse > COUNT(ts.EmailID) then COUNT(ts.EmailID) else @CountToUse end
                  FROM 
                        #ToSend ts
                              LEFT OUTER JOIN #OtherSuppression os ON ts.EmailID = os.EmailID
                              LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID
                  WHERE 
                        os.EmailID IS NULL AND
                        al.EmailID IS NULL
                        

            END         
      END 
      --UDFs exists        
      ELSE       
      BEGIN
      
            DECLARE --@StandAloneUDFs VARCHAR(2000),
                  ----@TransactionalUDFs VARCHAR(2000),
                  ------@StandAloneUDFIDs VARCHAR(1000), modified 7/10/2014 as we were exceeding 1000 characters
                  ------@TransactionalUDFIDs VARCHAR(1000), modified 7/10/2014 as we were exceeding 1000 characters
                  ----@StandAloneUDFIDs VARCHAR(2000),
                  ----@TransactionalUDFIDs VARCHAR(2000),
                  ----@StandAloneColumns VARCHAR(200),
                  ----@TransactionalColumns VARCHAR(200),
                  ----@StandAloneQuery VARCHAR(4000),
                  ----@TransactionalQuery VARCHAR(4000),
                  @StandAloneTempQuery VARCHAR(4000),
                  @TransactionalTempQuery VARCHAR(4000),
                  @StandAloneDrop VARCHAR(500),
                  @TransactionalDrop VARCHAR(500)
                  
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
                                          from  EmailDataValues edv WITH (NOLOCK) join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
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
                                          from  EmailDataValues edv WITH (NOLOCK)  join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
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
      

            --get all emails based on blast
            EXEC ( 
                  @StandAloneTempQuery + ';' + 
                  @TransactionalTempQuery + ';' +
            'insert into #ToSend  ' + 
            ' select Emails.EmailID, newID()  from  Emails with (NOLOCK) ' +@StandAloneQuery + @TransactionalQuery + 
                  ' join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
                  ' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
                  ' and EmailGroups.GroupID = ' + @GroupID +        
                  ' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID;' +
                  @StandAloneDrop + ';' + 
                  @TransactionalDrop + ';  ' 
                  )
            
              
			IF @BlastType = 'Sample' 
			BEGIN
				--for sample we want to include already sent emails so we can get the right amount
				SELECT @CountAvailable = COUNT(ts.EmailID) + @SampleAlreadySent
				FROM #ToSend ts					
					LEFT OUTER JOIN #OtherSuppression os on ts.EmailID = os.EmailID
				WHERE 
					os.EmailID is null
			END
			ELSE IF @SplitType in( 'e','m') -- need to get full count of emails in group for evenly split blasts
			BEGIN
				SELECT @CountAvailable = COUNT(ts.EmailID) 
				FROM #ToSend ts									
				left outer join #OtherSuppression os on ts.EmailID = os.EmailID and os.Reason <> '7Day'
				where os.EmailID is null
			END
			ELSE
			BEGIN
				SELECT @CountAvailable = COUNT(ts.EmailID) 
				FROM #ToSend ts
					LEFT OUTER JOIN #AlreadySent al on ts.EmailID = al.EmailID and al.Which <> 'current'
					LEFT OUTER JOIN #OtherSuppression os on ts.EmailID = os.EmailID --and os.Reason <> '7Day'
				WHERE 
					al.EmailID is null 
					and os.EmailID is null
            END
            
            --if amount is % get actual number for @OverRideIsAmount
			IF(@OverRideIsAmount is not null and @OverRideIsAmount = 0) 
			BEGIN
				  SET @OverrideAmount =  CEILING((convert(decimal(15,2),@CountAvailable) * convert(decimal(15,2),@OverrideAmount)) / 100)
				  IF @OverrideAmount = 0
				  BEGIN
						SET @OverrideAmount = 1
				  END
			END
            
            --use @OverrideAmount or @CountAvailable
            IF @OverrideAmount > 0
            BEGIN
				SET @CountToUse = @OverrideAmount
				
				SELECT @sentInEAL = COUNT(*) from #EmailActivityLog_Sent
				if ISNULL(@sentInEAL,0) > 0 and @sentInEAL < @CountToUse
				BEGIN
					SET @CountToUse = @CountToUse - @sentInEAL
				END
				--Commenting out below for future dev -- JWelter 4/28/2017
				--Special case for AB Blasts where the total to send for both A and B is greater than what is available to send when @OverrideIsAmount = 1
				--When this happens we will split the count available evenly across A and B, This code takes in to account that in the future
				--We might allow A,B,C,D blasts -- JWelter 4/28/2017
				--IF @BlastType = 'AB' and @OverrideIsAmount = 1
				--BEGIN
					
				--	select @BlastCount = COUNT(*) 
				--	from Blast b 
				--	where b.SampleID = @SampleID and b.BlastType = 'Sample'
					
				--	SET @TotalToSendForSample = @BlastCount * @OverrideAmount
					
				--	IF @CountAvailable < @TotalToSendForSample
				--	BEGIN
						
						
				--		SET @CountToUse = CEILING(convert(decimal(15,2),@CountAvailable) / convert(decimal(15,2),@BlastCount))
						
				--	END
				--	ELSE
				--	BEGIN
				--		SET @CountToUse = @OverrideAmount
				--	END
				--END
				--ELSE
				--BEGIN
    --              SET @CountToUse = @OverrideAmount
    --            END
            END
            ELSE
            BEGIN
                  IF @CountAvailable - @CountSent < 0
                  BEGIN
                        SET @CountToUse = 0
				  END
                  ELSE
                  BEGIN
                        SET @CountToUse = @CountAvailable - @CountSent
                  END
            END
			 --Getting test blast limit here
			if(@TestBlast = 'Y')
			BEGIN

				select @TestBlastLimit = ISNULL(TestBlastLimit,0) from ECN5_Accounts..Customer c where c.CustomerID = @CustomerID
				if(@TestBlastLimit = 0)
				BEGIN
					select @TestBlastLimit = ISNULL(TestBlastLimit,0) from ECN5_Accounts..BaseChannel bc where bc.BaseChannelID = @BasechannelID
					if(@TestBlastLimit = 0)
					BEGIN
						SET @CountToUse = 10
					END
					ELSE
					BEGIN
						SET @CountToUse = @TestBlastLimit
					END
				END
				ELSE
				BEGIN
					SET @CountToUse = @TestBlastLimit
				END

			END
            --get full records
            IF @OnlyCounts = 0      
            BEGIN
                  --log suppressed
				  if @BlastType = 'Sample'
				  BEGIN
						SET @CountToSuppress = @CountToUse - @CountAvailable + @SampleAlreadySent
						--SELECT 
						--	@CountToSuppress = COUNT(ts.EmailID) - @CountAvailable + @SampleAlreadySent
						--FROM 
						--	#ToSend ts
						--	join #OtherSuppression os on ts.EmailID = os.EmailID 
						--	left outer join #AlreadySent a on os.EmailID = a.EmailID
						--Where a.EmailID is null

						if @CountToSuppress > @CountToUse
						BEGIN
							SET @CountToSuppress = @CountToUse
						END
				  END
				  ELSE IF @SplitType in('e','m') 
				  BEGIN				  
				      SELECT @CountAvailable = COUNT(ts.EmailID)
					  from #ToSend ts 
						left outer join #OtherSuppression os on ts.EmailID = os.EmailID
					  where os.EmailID is null
				  
					  SELECT @CountToSuppress = @CountToUse - @CountAvailable					
				  END
				  Else
				  BEGIN
					  SELECT 
							@CountToSuppress = COUNT(ts.EmailID) - @CountAvailable
					  FROM 
							#ToSend ts
                  END           
                              
                  
                  IF @CountToSuppress > 0
                  BEGIN 
                        IF @LogSuppressed = 1
                        BEGIN
                              -- order to pull ascending threshold, 7 day, list, mslist, cmslist, domain (upto counttoUse)
                              exec ( 'INSERT into EmailActivityLog '+
                              ' select TOP ' + @CountToSuppress + ' os.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), os.Reason, os.BlastsAlreadySent, ''Y'' '+
                              ' from #ToSend ts '+
                                    ' JOIN #OtherSuppression os ON ts.EmailID = os.EmailID '+
                                    ' LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID '+
                              ' WHERE  '+
                                    ' al.EmailID IS NULL '+                         
                              ' order by os.OrderID ASC')
                        END
                  END
                        
                  --no dynamic content    
                  IF @DynamicSlotExists = 0 OR @BlastID = 0
                  BEGIN       
					
						if @blasttype = 'PERSONALIZATION'
						Begin
							exec (
								  @StandAloneTempQuery + ';' + 
								  @TransactionalTempQuery + ';' +
								  ' IF exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0 and IsDeleted = 0 )
								  BEGIN ' + 
								  'select Emails.EmailID,' + @BlastID + ' as BlastID, Emails.EmailAddress, '''+@mailRoute+''' as mailRoute, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
										'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
										'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + 
										@IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID, isnull(pc.PersonalizedContentID,0) as PersonalizedContentID, ' + 
										@emailcolumns + @StandAloneColumns + @TransactionalColumns + @DynamicTagColumns +    
										' into #tmpEmailList3  from #ToSend ts ' +  
										' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +  
										' left outer join ecn5_content..PersonalizedContent pc with (NOLOCK) on pc.blastID = ' + @blastID + ' and pc.emailaddress = Emails.emailaddress and isValid = 1 and isdeleted = 0 ' +
										' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' + @StandAloneQuery + @TransactionalQuery + 
										' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
										' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
										' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and al.EmailID is null and ot.EmailID is null ' + @Filter + ' order by ts.ECNRandomID;
                                    
										select t.* from #tmpEmailList3 t 
										where t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList3 t3) 
                                    
                                    
										drop table #tmpEmailList3
								  END
								  ELSE
								  BEGIN ' + 
								  'select top ' + @CountToUse + ' Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
										'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
										'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' +
										@IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID, isnull(pc.PersonalizedContentID,0) as PersonalizedContentID,  ' + 
										@emailcolumns +  @StandAloneColumns + @TransactionalColumns + @DynamicTagColumns + 
										'  into #tmpEmailList4  from #ToSend ts ' +
										' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
										' left outer join ecn5_content..PersonalizedContent pc with (NOLOCK) on pc.blastID = ' + @blastID + ' and pc.emailaddress = Emails.emailaddress and isValid = 1 and isdeleted = 0 ' +
										' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' + @StandAloneQuery + @TransactionalQuery + 
										' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
										' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
										' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' order by ts.ECNRandomID; 
                                    
										select t.* from #tmpEmailList4 t 
										where t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList4 t4) 
                                    
                                    
										drop table #tmpEmailList4
								  END;' +                       
								  @StandAloneDrop + ';' + 
								  @TransactionalDrop + ';' )
						End
						Else
						Begin
							exec (
								  @StandAloneTempQuery + ';' + 
								  @TransactionalTempQuery + ';' +
								  ' IF exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0 and IsDeleted = 0 )
								  BEGIN ' + 
								  'select Emails.EmailID,' + @BlastID + ' as BlastID, Emails.EmailAddress, '''+@mailRoute+''' as mailRoute, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
										'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
										'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + 
										@IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID,  ' + 
										@emailcolumns + @StandAloneColumns + @TransactionalColumns + @DynamicTagColumns +    
										' into #tmpEmailList3  from #ToSend ts ' +  
										' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +  
										' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' + @StandAloneQuery + @TransactionalQuery + 
										' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
										' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
										' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and al.EmailID is null and ot.EmailID is null ' + @Filter + ' order by ts.ECNRandomID;
                                    
										select t.* from #tmpEmailList3 t 
										where t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList3 t3) 
                                    
                                    
										drop table #tmpEmailList3
								  END
								  ELSE
								  BEGIN ' + 
								  'select top ' + @CountToUse + ' Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
										'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
										'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' +
										@IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID,  ' + 
										@emailcolumns +  @StandAloneColumns + @TransactionalColumns + @DynamicTagColumns + 
										'  into #tmpEmailList4  from #ToSend ts ' +
										' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
										' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' + @StandAloneQuery + @TransactionalQuery + 
										' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
										' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
										' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' order by ts.ECNRandomID; 
                                    
										select t.* from #tmpEmailList4 t 
										where t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList4 t4) 
                                    
                                    
										drop table #tmpEmailList4
								  END;' +                       
								  @StandAloneDrop + ';' + 
								  @TransactionalDrop + ';' )
							End
                  END
                  --dynamic content
                  ELSE
                  BEGIN
                  
                        SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 
						EXEC (                  
                              @StandAloneTempQuery + ';' + 
                              @TransactionalTempQuery + ';' +
                              ' ' +
                        @selectslotstr + @DynamicTagColumns + ' into #tmpEmailList5 from ( select  Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
                        '''eid='' + Convert(varchar,Emails.EmailID) +''&bid= '+ @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
                        '''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress,  '''+ @CurrDate + ''' as CurrDate, ' +  
                        @IsMTAPriority + ' as IsMTAPriority, ts.ECNRandomID,  ' + @emailcolumns +    
                        @StandAloneColumns + @TransactionalColumns +       
                        ' from #ToSend ts ' +
                        ' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
                        ' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +@StandAloneQuery + @TransactionalQuery + 
                        ' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
                        ' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
                        ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' ) inn2 order by inn2.ECNRandomID
                        
                        select t.* from #tmpEmailList5 t 
                        where t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList5 t5) 
                        
                        
                        drop table #tmpEmailList5;                      
                        
                        ' +                     
                        @StandAloneDrop + ';' + 
                        @TransactionalDrop + ';' )
                        
                  END
            END
            --just get count
            ELSE
            BEGIN
                  --SELECT @CountToUse    -- 7/17/2013 - WGH: Need to use suppression and already sent in count
                  
                  SELECT
                        case when @CountToUse > COUNT(ts.EmailID) then COUNT(ts.EmailID) else @CountToUse end
                  FROM 
                        #ToSend ts
                              LEFT OUTER JOIN #OtherSuppression os ON ts.EmailID = os.EmailID
                              LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID
                  WHERE 
                        os.EmailID IS NULL AND
                        al.EmailID IS NULL
            END  
      END              
      if len(ltrim(rtrim(@SGsqlstring))) > 0
            Begin
                  set @Suppressionsqlstring += '; 
                                     drop table #SuppressionSmartSegment; '
            End

      drop table #tempA        
      drop table #E        
      drop table #OtherSuppression
      drop table #ToSend
      drop table #AlreadySent
      drop table #ThresholdSuppression    
	  drop table #EmailActivityLog_Sent


END