CREATE PROCEDURE [dbo].[spGetGraphicalBlastBounceReportData] (   
 @blastID int  
)  
AS  
BEGIN   
 SELECT  DISTINCT  
   substring(BounceMessage,0,50) as 'ActionNotes',  
   count(EmailID) as 'BounceCount',   
   bc.BounceCode as 'BounceType'  
 FROM   
   BlastActivityBounces babc JOIN BounceCodes bc on babc.BounceCodeID = bc.BounceCodeID  
 WHERE   
   BlastID = @blastID AND   
   isnull(babc.BounceMessage,'') <> ''  
 GROUP BY  
   substring(babc.BounceMessage,0,50), bc.BounceCode  
 ORDER BY  
   bc.BounceCode ASC, count(EmailID) DESC    
END
