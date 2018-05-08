


CREATE VIEW [dbo].vw_RecentBrandConsensus_CLV
	AS 
WITH cte 
     AS (SELECT bd.brandid, 
                s.cgrp_no, 
                vw.mastergroupid, 
                Max(psd.datecreated) AS DateCreated 
         FROM   dbo.subscriptions AS s WITH (nolock) 
                INNER JOIN dbo.pubsubscriptions AS ps WITH (nolock) ON s.subscriptionid = ps.subscriptionid 
                INNER JOIN dbo.pubsubscriptiondetail AS psd ON psd.pubsubscriptionid = ps.pubsubscriptionid 
                INNER JOIN dbo.branddetails AS bd WITH (nolock) ON bd.pubid = ps.pubid 
                INNER JOIN dbo.brand AS b WITH (nolock)  ON b.brandid = bd.brandid 
                INNER JOIN dbo.vw_mapping AS vw  ON vw.codesheetid = psd.codesheetid 
         WHERE  ( b.isdeleted = 0 ) 
         GROUP  BY bd.brandid, 
                   s.cgrp_no, 
                   vw.mastergroupid) 
SELECT DISTINCT cte.brandid, 
                cte.cgrp_no, 
                vw.masterid, 
                cte.mastergroupid 
FROM   dbo.subscriptions AS s WITH (nolock) 
       INNER JOIN dbo.pubsubscriptions AS ps WITH (nolock) ON s.subscriptionid = ps.subscriptionid 
       INNER JOIN dbo.pubsubscriptiondetail AS psd ON psd.pubsubscriptionid = ps.pubsubscriptionid 
       INNER JOIN dbo.branddetails AS bd WITH (nolock)  ON bd.pubid = ps.pubid 
       INNER JOIN dbo.vw_mapping AS vw  ON vw.codesheetid = psd.codesheetid 
       INNER JOIN cte  ON s.cgrp_no = cte.cgrp_no AND psd.datecreated = cte.datecreated AND vw.mastergroupid = cte.mastergroupid AND cte.BrandID = bd.BrandID