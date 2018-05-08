CREATE VIEW [dbo].vw_RecentBrandConsensus
AS

WITH cte 
     AS (SELECT bd.brandid, 
                psd.subscriptionid, 
                vw.mastergroupid, 
                Max(psd.datecreated) AS DateCreated 
         FROM   dbo.pubsubscriptions AS ps WITH (nolock) 
                INNER JOIN dbo.pubsubscriptiondetail AS psd ON psd.pubsubscriptionid = ps.pubsubscriptionid 
                INNER JOIN dbo.branddetails AS bd WITH (nolock) ON bd.pubid = ps.pubid 
                INNER JOIN dbo.brand AS b WITH (nolock) ON b.brandid = bd.brandid 
                INNER JOIN dbo.vw_mapping AS vw  ON vw.codesheetid = psd.codesheetid 
         WHERE  ( b.isdeleted = 0 ) 
         GROUP  BY bd.brandid, 
                   psd.subscriptionid, 
                   vw.mastergroupid) 
SELECT DISTINCT cte.brandid, 
                cte.subscriptionid, 
                vw.masterid, 
                cte.mastergroupid 
FROM   dbo.pubsubscriptions AS ps WITH (nolock) 
       INNER JOIN dbo.pubsubscriptiondetail AS psd ON psd.pubsubscriptionid = ps.pubsubscriptionid 
       INNER JOIN dbo.branddetails AS bd WITH (nolock) ON bd.pubid = ps.pubid 
       INNER JOIN dbo.vw_mapping AS vw ON vw.codesheetid = psd.codesheetid 
       INNER JOIN cte ON psd.subscriptionid = cte.subscriptionid AND psd.datecreated = cte.datecreated  AND vw.mastergroupid = cte.mastergroupid  AND cte.BrandID = bd.BrandID