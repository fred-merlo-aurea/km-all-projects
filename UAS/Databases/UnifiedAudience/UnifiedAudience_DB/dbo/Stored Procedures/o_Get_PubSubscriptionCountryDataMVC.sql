  
CREATE PROCEDURE [dbo].[o_Get_PubSubscriptionCountryDataMVC]    
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
    
      
   SELECT ISNULL(c.ShortName, 'No Response') as Country, SUM(ps.Copies) as Copies    
   FROM PubSubscriptions ps    
    JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID    
    LEFT JOIN UAD_Lookup..Country c ON c.CountryID = ps.CountryID    
   GROUP BY ISNULL(c.ShortName, 'No Response')    
  END    
 ELSE --Query Archive    
  BEGIN    
     
      
   SELECT ISNULL(c.ShortName, 'No Response') as Country, SUM(ps.Copies) as Copies    
   FROM IssueArchiveProductSubscription ps    
    JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID    
    LEFT JOIN UAD_Lookup..Country c ON c.CountryID = ps.CountryID    
   GROUP BY ISNULL(c.ShortName, 'No Response')    
  END    
END    