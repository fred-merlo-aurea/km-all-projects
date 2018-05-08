CREATE PROCEDURE [dbo].[spGetCampaignItemReportDataforMA] 
	@CampaignItemID int,
	@ReportType varchar(50) = 'all'
AS
BEGIN
      SET NOCOUNT ON;
      declare  @blastIDs table (bID int primary key,GroupID int, CampaignItemID int)
    
      Insert into @blastIDs
      SELECT distinct BlastID,GroupID, CampaignItemID
      from ecn5_communicator..CampaignItemBlast  where CampaignItemID=@CampaignItemID and IsDeleted=0 and BlastID is not null

	  if exists(select top 1 * from @blastIDs where GroupID is null)
	BEGIN
		declare @currentBlastID int,
				 @refBlastID int,					
					@groupID int,
					@layoutPlanID int,
					@IsFormTrig bit,
					@campItemID int,
					@index int = 0
		declare mycursor cursor
		FOR
		Select bID, GroupID from @blastIDs where GroupID is null
		OPEN mycursor
		FETCH NEXT FROM mycursor into @currentBlastID, @groupID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET  @refBlastID = null
			SET  @layoutPlanID = null
			SET  @IsFormTrig = null
			SET @campItemID = null
			SET @refBlastID = @currentBlastID
			while @groupID is null and @index < 10
			BEGIN
				SET @IsFormTrig = null
				SET @campItemID = null
				SET @layoutPlanID = null
				select @IsFormTrig = case when EventType in('abandon','submit') then 1 else 0 end, @campItemID = CampaignItemID , @layoutPlanID = LayoutPlanID
				from ECN5_COMMUNICATOR..LayoutPlans lp with(nolock)
				where lp.BlastID = @refBlastID and ISNULL(lp.IsDeleted,0) = 0 and lp.Status = 'y'
				
				
				if @layoutPlanID is not null and @campItemID is not null
				BEGIN
					--must be a layout plan
					select @refBlastID = BlastID 
					from ECN5_Communicator..CampaignItemBlast cib with(nolock) 
					where cib.CampaignItemID = @campItemID
			
					select @groupID = GroupID from ECN5_Communicator..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				ELSE IF @layoutPlanID is not null and @campItemID is null
				BEGIN
					select @layoutPlanID = MIN(LayoutPlanID), @refBlastID = MIN(bs.refblastID)
					from ECN5_Communicator..BlastSingles bs with(nolock) 
					where bs.BlastID = @refBlastID
			
					select @groupID = GroupID from ECN5_Communicator..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				ELSE
				BEGIN		
					--must be a trigger plan
					select @refBlastID = tp.RefTriggerID 
					from ECN5_Communicator..TriggerPlans tp with(nolock)
					where tp.BlastID = @refBlastID and ISNULL(tp.IsDeleted,0) = 0 and tp.Status = 'y'
			
					select @campItemID = CampaignItemID
					from ECN5_Communicator..CampaignItemBlast cib with(nolock) 
					where cib.BlastID = @refBlastID
			
					select @groupID = GroupID from ECN5_Communicator..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				SET @index = @index + 1
			END
			
			SET @index = 0
			if @groupID is not null
			BEGIN
				update @blastIDs
				set GroupID = @groupID
				where bID = @currentBlastID
				SET @groupID = null
			END
			FETCH NEXT FROM mycursor into @refBlastID, @groupID
		END		
		close mycursor 
		deallocate mycursor
	END			





      declare @SeedListIDs table(pKey int identity(1,1),EmailID int)
      
      insert into @SeedListIDs(EmailID)
      Select eg.EmailID
      from ECN5_COMMUNICATOR..EmailGroups eg with(nolock)
      join ECN5_COMMUNICATOR..Groups g with(nolock) on eg.groupid = g.GroupID
      left outer join ECN5_COMMUNICATOR..EmailGroups eg2 with(nolock) on eg.EmailID = eg2.EmailID and eg2.GroupID in (Select GroupID from @blastIDs)
      where g.CustomerID in (Select CustomerID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID in (select bID from @blastIDs))
                  and isnull(g.IsSeedList,0) = 1
                  and eg2.EmailGroupID is null
        
      
      if @ReportType = 'All'
	  BEGIN      
      SELECT sum(DistinctCount) AS 'DistinctCount', sum(total) AS 'total' , ActionTypeCode from 
      (
            SELECT  
                        ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
                        ISNULL(COUNT(bac.EmailID),0) AS total, 
                        'UNIQCLIQ' as ActionTypeCode,
                        CampaignItemID
            FROM  
                        BlastActivityClicks bac WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bac.BlastID
						join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                        left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
            where eg2.pKey is null
            GROUP BY CampaignItemID 
            UNION      
            SELECT  
                        ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
                        ISNULL(SUM(total),0) AS total,
                        'click'  as ActionTypeCode,
                        CampaignItemID       
            FROM (        
                        SELECT  COUNT(distinct URL) AS DistinctCount, COUNT(bac.EmailID) AS total, CampaignItemID        
                        FROM   BlastActivityClicks bac WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bac.blastID
						join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                        left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
                        where eg2.pKey is null
                        GROUP BY  URL, bac.EmailID, CampaignItemID        
                  ) AS inn Group by CampaignItemID
            UNION  
            SELECT ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
                     ISNULL(COUNT(bac.EmailID),0) as total,
                     'clickthrough' as ActionTypeCode,
                     CampaignItemID
            FROM BlastActivityClicks bac with(nolock)
                  join @blastIDs ids ON ids.bID = bac.BlastID
				  join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                  left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
            where eg2.pKey is null
           GROUP BY CampaignItemID 
            UNION 
				SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'send' AS ActionTypeCode, CampaignItemID 
				FROM BlastActivitySends bas WITH (NOLOCK) 
					JOIN @blastIDs ids ON ids.bID = bas.blastID 
					join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
					left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID
				where eg2.pKey is null
				GROUP BY CampaignItemID 
            UNION 
            SELECT ISNULL(COUNT( DISTINCT bar.EmailID),0) AS DistinctCount, ISNULL(COUNT(bar.EmailID),0) AS total, 'resend' AS ActionTypeCode, CampaignItemID 
			FROM BlastActivityResends bar WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bar.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bar.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                left outer join @SeedListIDs eg2 on bar.EmailID = eg2.EmailID
			where eg2.pKey is null
			GROUP BY CampaignItemID 
            UNION 
            SELECT ISNULL(COUNT( DISTINCT bao.EmailID),0) AS DistinctCount, ISNULL(COUNT(bao.EmailID),0) AS total, 'open' AS ActionTypeCode, CampaignItemID 
			FROM BlastActivityOpens bao WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bao.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bao.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                left outer join @SeedListIDs eg2 on bao.EmailID = eg2.EmailID
            where eg2.pKey is null
            GROUP BY CampaignItemID 
            UNION 
            SELECT ISNULL(COUNT( DISTINCT bab.EmailID),0) AS DistinctCount, ISNULL(COUNT(bab.EmailID),0) AS total, 'bounce' AS ActionTypeCode, CampaignItemID 
			FROM BlastActivityBounces bab WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bab.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bab.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                left outer join @SeedListIDs eg2 on bab.EmailID = eg2.EmailID
            where eg2.pKey is null
           GROUP BY CampaignItemID     
            UNION
            SELECT  
                        ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
                        ISNULL(COUNT(bau.EmailID),0) AS total, 
                        uc.UnsubscribeCode  as ActionTypeCode,
                        CampaignItemID
            FROM  
                        BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
						JOIN @blastIDs ids ON ids.bID = bau.blastID 
						join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bau.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                        left outer join @SeedListIDs eg2 on bau.EmailID = eg2.EmailID
                        where eg2.pKey is null
                        GROUP BY  uc.UnsubscribeCode, CampaignItemID
            UNION
            SELECT  
                        ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
                        ISNULL(COUNT(basupp.EmailID),0) AS total, 
                        sc.SupressedCode  as ActionTypeCode,
                        CampaignItemID
            FROM  
                        BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID 
						JOIN @blastIDs ids ON ids.bID = basupp.blastID 
						join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on basupp.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                        left outer join @SeedListIDs eg2 on basupp.EmailID = eg2.EmailID
                        where eg2.pKey is null
                        GROUP BY  sc.SupressedCode, CampaignItemID
            UNION 
            SELECT ISNULL(COUNT( DISTINCT bac.EmailID),0) AS DistinctCount, ISNULL(COUNT(bac.EmailID),0) AS total, 'conversion' AS ActionTypeCode, CampaignItemID 
			FROM BlastActivityConversion bac WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bac.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
            where eg2.pKey is null
            GROUP BY CampaignItemID                   
            UNION 
            SELECT ISNULL(COUNT( DISTINCT bar.EmailID),0) AS DistinctCount, ISNULL(COUNT(bar.EmailID),0) AS total, 'refer' AS ActionTypeCode, CampaignItemID 
			FROM BlastActivityRefer bar WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bar.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bar.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
			GROUP BY CampaignItemID
              UNION
			SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'noclick' AS ActionTypeCode, CampaignItemID
			FROM BlastActivitySends bas WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bas.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
				left outer join BlastActivityClicks bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
				left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID             
			WHERE bac.ClickID is null and eg2.pKey is null
            GROUP BY CampaignItemID
            UNION
            SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'open_noclick' AS ActionTypeCode, CampaignItemID 
            FROM BlastActivityOpens bas WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bas.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
				left outer join BlastActivityClicks bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
				left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID       
            Where eg2.pKey is null and bac.ClickID is null
            GROUP BY CampaignItemID
            UNION
            SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'noopen' AS ActionTypeCode, CampaignItemID
            FROM BlastActivitySends bas WITH (NOLOCK) 
				JOIN @blastIDs ids ON ids.bID = bas.blastID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
				left outer join BlastActivityOpens bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
				left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID       
            WHERE bac.OpenID is null and eg2.pKey is null
            GROUP BY CampaignItemID
            UNION
            SELECT ISNULL(COUNT( DISTINCT eg.EmailID),0) AS DistinctCount, ISNULL(COUNT(eg.EmailID),0) AS total, 'notsent' AS ActionTypeCode, CampaignItemID
            FROM @blastIDs ids             
				left outer join BlastActivitySends bas with(nolock) on bas.blastid = ids.bID 
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on ids.GroupID = eg.GroupID and bas.EmailID = eg.EMailID and eg.SubscribeTypeCode = 's'
				left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID       
            WHERE bas.SendID is null and eg2.pKey is null
            GROUP BY CampaignItemID
           
      ) outer1 
      GROUP BY ActionTypeCode
	  END
	  ELSE if @ReportType = 'send'
	  BEGIN
		SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'send' AS ActionTypeCode, CampaignItemID 
		FROM BlastActivitySends bas WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bas.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
			left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID
		where eg2.pKey is null
		GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'UNIQCLIQ'
	  BEGIN
		 SELECT  
                    ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
                    ISNULL(COUNT(bac.EmailID),0) AS total, 
                    'UNIQCLIQ' as ActionTypeCode,
                    CampaignItemID
        FROM  
                    BlastActivityClicks bac WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bac.BlastID
					join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
                    left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
        where eg2.pKey is null
        GROUP BY CampaignItemID 
	  END
	  ELSE IF @ReportType = 'click'
	  BEGIN
	  SELECT  
            ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
            ISNULL(SUM(total),0) AS total,
            'click'  as ActionTypeCode,
            
			CampaignItemID
        FROM (        
            SELECT  COUNT(distinct URL) AS DistinctCount, COUNT(bac.EmailID) AS total, BlastID, MIN(CampaignItemID) as CampaignItemID
            FROM   BlastActivityClicks bac WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bac.blastID
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
            left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
            where eg2.pKey is null
            GROUP BY  URL, bac.EmailID, BlastID        
         ) AS inn Group by CampaignItemID
	  END
	  else if @ReportType = 'clickthrough'
	  BEGIN
	  SELECT ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
            ISNULL(COUNT(bac.EmailID),0) as total,
            'clickthrough' as ActionTypeCode,
            CampaignItemID
		FROM BlastActivityClicks bac with(nolock)
				join @blastIDs ids ON ids.bID = bac.BlastID
				join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
				left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
		where eg2.pKey is null
		GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'resend'
	  BEGIN
	   SELECT ISNULL(COUNT( DISTINCT bar.EmailID),0) AS DistinctCount, ISNULL(COUNT(bar.EmailID),0) AS total, 'resend' AS ActionTypeCode, CampaignItemID 
		FROM BlastActivityResends bar WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bar.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bar.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
            left outer join @SeedListIDs eg2 on bar.EmailID = eg2.EmailID
		where eg2.pKey is null
		GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'open'
	  BEGIN
	  SELECT ISNULL(COUNT( DISTINCT bao.EmailID),0) AS DistinctCount, ISNULL(COUNT(bao.EmailID),0) AS total, 'open' AS ActionTypeCode, CampaignItemID 
		FROM BlastActivityOpens bao WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bao.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bao.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
            left outer join @SeedListIDs eg2 on bao.EmailID = eg2.EmailID
        where eg2.pKey is null
        GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'bounce'
	  BEGIN
	   SELECT ISNULL(COUNT( DISTINCT bab.EmailID),0) AS DistinctCount, ISNULL(COUNT(bab.EmailID),0) AS total, 'bounce' AS ActionTypeCode, CampaignItemID 
		FROM BlastActivityBounces bab WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bab.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bab.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
            left outer join @SeedListIDs eg2 on bab.EmailID = eg2.EmailID
        where eg2.pKey is null
        GROUP BY CampaignItemID      
	  END
	  else if @ReportType = 'unsubscribe'
	  BEGIN
	   SELECT  
            ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
            ISNULL(COUNT(bau.EmailID),0) AS total, 
            uc.UnsubscribeCode  as ActionTypeCode,
            CampaignItemID
        FROM  
            BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
			JOIN @blastIDs ids ON ids.bID = bau.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bau.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
            left outer join @SeedListIDs eg2 on bau.EmailID = eg2.EmailID
        where eg2.pKey is null
        GROUP BY  uc.UnsubscribeCode, CampaignItemID
	  END
	  else if @ReportType = 'suppressed'
	  BEGIN
	  SELECT  
            ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
            ISNULL(COUNT(basupp.EmailID),0) AS total, 
            sc.SupressedCode  as ActionTypeCode,
            CampaignItemID
		FROM  
            BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID 
			JOIN @blastIDs ids ON ids.bID = basupp.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on basupp.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
            left outer join @SeedListIDs eg2 on basupp.EmailID = eg2.EmailID
        where eg2.pKey is null
        GROUP BY  sc.SupressedCode, CampaignItemID
	  END
	  else if @ReportType = 'conversion'
	  BEGIN
		SELECT ISNULL(COUNT( DISTINCT bac.EmailID),0) AS DistinctCount, ISNULL(COUNT(bac.EmailID),0) AS total, 'conversion' AS ActionTypeCode, CampaignItemID 
		FROM BlastActivityConversion bac WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bac.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bac.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
            left outer join @SeedListIDs eg2 on bac.EmailID = eg2.EmailID
        where eg2.pKey is null
        GROUP BY CampaignItemID      
	  END
	  else if @ReportType = 'refer'
	  BEGIN
		SELECT ISNULL(COUNT( DISTINCT bar.EmailID),0) AS DistinctCount, ISNULL(COUNT(bar.EmailID),0) AS total, 'refer' AS ActionTypeCode, CampaignItemID 
		FROM BlastActivityRefer bar WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bar.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bar.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
		GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'noclick'
	  BEGIN
	  SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'noclick' AS ActionTypeCode, CampaignItemID 
		FROM BlastActivitySends bas WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bas.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
			left outer join BlastActivityClicks bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
			left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID             
		WHERE bac.ClickID is null and eg2.pKey is null
		GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'open_noclick'
	  BEGIN
	   SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'open_noclick' AS ActionTypeCode, CampaignItemID
        FROM BlastActivityOpens bas WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bas.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
			left outer join BlastActivityClicks bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
			left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID       
        Where eg2.pKey is null and bac.ClickID is null
        GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'noopen'
	  BEGIN
	   SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'noopen' AS ActionTypeCode, CampaignItemID
        FROM BlastActivitySends bas WITH (NOLOCK) 
			JOIN @blastIDs ids ON ids.bID = bas.blastID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on bas.EmailID = eg.EmailID and ids.GroupID = eg.GroupID 
			left outer join BlastActivityOpens bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
			left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID       
        WHERE bac.OpenID is null and eg2.pKey is null
        GROUP BY CampaignItemID
	  END
	  else if @ReportType = 'notsent'
	  BEGIN
	  SELECT ISNULL(COUNT( DISTINCT eg.EmailID),0) AS DistinctCount, ISNULL(COUNT(eg.EmailID),0) AS total, 'notsent' AS ActionTypeCode, CampaignItemID 
        FROM @blastIDs ids             
			left outer join BlastActivitySends bas with(nolock) on bas.blastid = ids.bID 
			join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on ids.GroupID = eg.GroupID and bas.EmailID = eg.EMailID and eg.SubscribeTypeCode = 's'
			left outer join @SeedListIDs eg2 on bas.EmailID = eg2.EmailID       
        WHERE bas.SendID is null and eg2.pKey is null
        GROUP BY CampaignItemID
	  END
END



