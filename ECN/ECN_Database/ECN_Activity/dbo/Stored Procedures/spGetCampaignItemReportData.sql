CREATE PROCEDURE [dbo].[spGetCampaignItemReportData] 
	@CampaignItemID int
AS
BEGIN
	  SET NOCOUNT ON;
      declare  @blastIDs table (bID int primary key)
    
      Insert into @blastIDs
      SELECT distinct BlastID
      from ecn5_communicator..CampaignItemBlast  where CampaignItemID=@CampaignItemID and IsDeleted=0 and BlastID is not null

      SELECT sum(DistinctCount) AS 'DistinctCount', sum(total) AS 'total' , ActionTypeCode from 
      (
            SELECT  
                        ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
                        ISNULL(COUNT(bac.EmailID),0) AS total, 
                        'UNIQCLIQ' as ActionTypeCode,
                        BlastID
            FROM  
                        BlastActivityClicks bac WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bac.BlastID
            GROUP BY BlastID 
            UNION      
            SELECT  
                        ISNULL(SUM(DistinctCount),0) AS DistinctCount, 
                        ISNULL(SUM(total),0) AS total,
                        'click'  as ActionTypeCode,
                        BlastID       
            FROM (        
                        SELECT  COUNT(distinct URL) AS DistinctCount, COUNT(bac.EmailID) AS total, BlastID        
                        FROM   BlastActivityClicks bac WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bac.blastID
                        GROUP BY  URL, bac.EmailID, BlastID        
                  ) AS inn Group by BlastID
            UNION  
            SELECT ISNULL(COUNT(DISTINCT bac.EmailID),0) AS DistinctCount,
                     ISNULL(COUNT(bac.EmailID),0) as total,
                     'clickthrough' as ActionTypeCode,
                     BlastID
            FROM BlastActivityClicks bac with(nolock)
                  join @blastIDs ids ON ids.bID = bac.BlastID
            GROUP BY BlastID
            UNION 
            SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'send' AS ActionTypeCode, BlastID FROM BlastActivitySends bas WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bas.blastID GROUP BY BlastID
            UNION 
            SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'resend' AS ActionTypeCode, BlastID FROM BlastActivityResends bar WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bar.blastID GROUP BY BlastID
            UNION 
            SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'open' AS ActionTypeCode, BlastID FROM BlastActivityOpens bao WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bao.blastID GROUP BY BlastID
            UNION 
            SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'bounce' AS ActionTypeCode, BlastID FROM BlastActivityBounces bab WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bab.blastID GROUP BY BlastID      
            UNION
            SELECT  
                        ISNULL(COUNT(DISTINCT bau.EmailID),0) AS DistinctCount,
                        ISNULL(COUNT(bau.EmailID),0) AS total, 
                        uc.UnsubscribeCode  as ActionTypeCode,
                        BlastID
            FROM  
                        BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) ON bau.UnsubscribeCodeID = uc.UnsubscribeCodeID JOIN @blastIDs ids ON ids.bID = bau.blastID 
            GROUP BY  uc.UnsubscribeCode, bau.BlastID
            UNION
            SELECT  
                        ISNULL(COUNT(DISTINCT basupp.EmailID),0) AS DistinctCount,
                        ISNULL(COUNT(basupp.EmailID),0) AS total, 
                        sc.SupressedCode  as ActionTypeCode,
                        BlastID
            FROM  
                        BlastActivitySuppressed basupp WITH (NOLOCK) JOIN SuppressedCodes sc WITH (NOLOCK) ON basupp.SuppressedCodeID = sc.SuppressedCodeID JOIN @blastIDs ids ON ids.bID = basupp.blastID 
            GROUP BY  sc.SupressedCode, BlastID
            UNION 
            SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'conversion' AS ActionTypeCode, BlastID FROM BlastActivityConversion bac WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bac.blastID GROUP BY BlastID                   
            UNION 
            SELECT ISNULL(COUNT( DISTINCT EmailID),0) AS DistinctCount, ISNULL(COUNT(EmailID),0) AS total, 'refer' AS ActionTypeCode, BlastID FROM BlastActivityRefer bar WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bar.blastID GROUP BY BlastID
            --UNION
            --SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'noclick' AS ActionTypeCode, bas.BlastID 
            --FROM BlastActivitySends bas WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bas.blastID 
            --left outer join BlastActivityClicks bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
            --WHERE bac.ClickID is null
            --GROUP BY bas.BlastID
            --UNION
            --SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'open_noclick' AS ActionTypeCode, bas.BlastID 
            --FROM BlastActivityOpens bas WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bas.blastID 
            --left outer join BlastActivityClicks bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
            --WHERE bac.ClickID is null
            --GROUP BY bas.BlastID
            --UNION
            --SELECT ISNULL(COUNT( DISTINCT bas.EmailID),0) AS DistinctCount, ISNULL(COUNT(bas.EmailID),0) AS total, 'noopen' AS ActionTypeCode, bas.BlastID 
            --FROM BlastActivitySends bas WITH (NOLOCK) JOIN @blastIDs ids ON ids.bID = bas.blastID 
            --left outer join BlastActivityOpens bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
            --WHERE bac.OpenID is null
            --GROUP BY bas.BlastID
            --UNION
            --SELECT ISNULL(COUNT( DISTINCT eg.EmailID),0) AS DistinctCount, ISNULL(COUNT(eg.EmailID),0) AS total, 'notsent' AS ActionTypeCode, bas.BlastID 
            --FROM ECN5_COMMUNICATOR..Blast b with(nolock) 
            --join ECN5_COMMUNICATOR..EmailGroups eg WITH (NOLOCK) on b.GroupID =  eg.GroupID and eg.SubscribeTypeCode = 's'
            --JOIN @blastIDs ids ON ids.bID = b.blastID 
            --left outer join BlastActivitySends bas with(nolock) on bas.blastid = b.blastid and eg.EmailID = bas.EmailID
            --WHERE bas.SendID is null
            --GROUP BY bas.BlastID
      ) outer1 
      GROUP BY ActionTypeCode
END