CREATE PROC [dbo].[spGetGraphicalBlastReportData] (  
  @blastID int  
)  
as  
Begin  
 SELECT  EmailID, 'send' as ActionTypeCode, SendTime as ActionDate,    
   convert(varchar, SendTime, 101) as 'ActionDateMMDDYYYY', '' as ActionValue, SMTPMessage as  'ActionNotes'   
 FROM   
   BlastActivitySends where BlastID = @blastID  
 UNION  
 SELECT  EmailID, 'bounce' as ActionTypeCode, BounceTime as ActionDate,    
   convert(varchar, BounceTime, 101) as 'ActionDateMMDDYYYY', bc.BounceCode as ActionValue, BounceMessage as  'ActionNotes'    
 FROM   
   BlastActivityBounces join BounceCodes bc on BlastActivityBounces.BounceCodeID = bc.BounceCodeID      
   where BlastID = @blastID  
 UNION  
 SELECT  EmailID, 'click' as ActionTypeCode, ClickTime as ActionDate,    
   convert(varchar, ClickTime, 101) as 'ActionDateMMDDYYYY', '' as ActionValue, BlastActivityClicks.URL as  'ActionNotes'   
 FROM   
   BlastActivityClicks where BlastID = @blastID  
 UNION  
 SELECT  EmailID, 'open' as ActionTypeCode, OpenTime as ActionDate,    
   convert(varchar, OpenTime, 101) as 'ActionDateMMDDYYYY', '' as ActionValue, BlastActivityOpens.BrowserInfo as  'ActionNotes'   
 FROM   
   BlastActivityOpens where BlastID = @blastID  
 UNION  
 SELECT  EmailID, 'conv' as ActionTypeCode, ConversionTime as ActionDate,    
   convert(varchar, ConversionTime, 101) as 'ActionDateMMDDYYYY', '' as ActionValue, BlastActivityConversion.URL as  'ActionNotes'   
 FROM   
   BlastActivityConversion where BlastID = @blastID   
 UNION  
 SELECT  EmailID, 'resend' as ActionTypeCode, ResendTime as ActionDate,    
   convert(varchar, ResendTime, 101) as 'ActionDateMMDDYYYY', '' as ActionValue, '' as  'ActionNotes'   
 FROM   
   BlastActivityResends where BlastID = @blastID  
 UNION  
 SELECT  EmailID, 'refer' as ActionTypeCode, ReferTime as ActionDate,    
   convert(varchar, ReferTime, 101) as 'ActionDateMMDDYYYY', '' as ActionValue, '' as 'ActionNotes'  
 FROM   
   BlastActivityRefer where BlastID = @blastID    
End
