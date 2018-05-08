  
CREATE PROCEDURE [dbo].[o_Get_PubSubscriptionStateDataMVC]    
 @Queries VARCHAR(MAX),   
 @IssueID int = 0    
AS    
BEGIN    
    
 SET NOCOUNT ON    
    
 IF 1=0 BEGIN    
    SET FMTONLY OFF    
 END    
     
 CREATE TABLE #SubscriptionID (PubSubscriptionID int)      
 INSERT INTO #SubscriptionID    
 EXEC (@Queries)    
      
 IF @IssueID = 0 --Query Current Issue    
  BEGIN    
   SELECT ps.RegionCode, SUM(ps.Copies) as Copies    
   FROM PubSubscriptions ps    
    JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID    
   GROUP BY ps.RegionCode    
  END    
 ELSE --Query Archive    
  BEGIN    
   SELECT ps.RegionCode, SUM(ps.Copies) as Copies    
   FROM IssueArchiveProductSubscription ps    
    JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID    
   GROUP BY ps.RegionCode    
  END    
END    