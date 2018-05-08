CREATE PROCEDURE [dbo].[rpt_GetSubscriptionIDs_Copies_From_FilterQuery]    
(  @Queries varchar(MAX) = ''    
) AS    
BEGIN    
set nocount on    
    
  CREATE TABLE #SubscriptionID (SubscriptionID int)        
  CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)      
      
  INSERT INTO #SubscriptionID    
  EXEC (@Queries)    
    
  SELECT SubscriptionID FROM #SubscriptionID    
    
  DROP TABLE #SubscriptionID    
END 