CREATE PROCEDURE [dbo].[spGetCustomEmailActivityData]   
(  
 @CustomerID int,  
 @actioncode varchar(400),  
 @actionvalue varchar(400),  
 @Frequency  varchar(10),  
 @daysBlock int  
)  
AS  
BEGIN   
   
 DECLARE @sqlstring varchar(4000)  
 PRINT (@actioncode)  
 IF UPPER(@Frequency) = 'DAILY'  
 BEGIN  
   SET @sqlstring=        
     'SELECT baus.blastID, e.EmailAddress, baus.EmailID, '+       
     ' (CASE usc.UnsubscribeCode '+     
       ' WHEN ''subscribe''     THEN ''UnSubscribe''   '+     
       ' WHEN ''MASTSUP_UNSUB''    THEN ''UnSubscribe''   '+     
       ' WHEN ''ABUSERPT_UNSUB''  THEN ''UnSubscribe''   '+    
       ' WHEN ''FEEDBACK_UNSUB''   THEN ''UnSubscribe''   '+    
      ' ELSE usc.UnsubscribeCode END) AS ActionType, '+     
     ' (CASE bc.BounceCode '+  
      '  WHEN ''hardbounce''     THEN ''hard''  '+   
      '  WHEN ''softbounce''     THEN ''soft'' '+   
      '  WHEN ''spamnotification''    THEN ''blocked''  '+  
      '  WHEN ''autoresponder''    THEN ''notify'' '+    
      '  WHEN ''U''     THEN ''U''  '+  
      ' ELSE ''unknown'' END) AS ActionValue,  '+  
     ' baus.UnsubscribeTime as ActionDate, baus.Comments as ActionNotes '+  
     ' FROM ecn5_communicator..[BLAST] b join '+  
     ' BlastActivityUnSubscribes baus on baus.blastID = b.blastID '+    
     ' join UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID '+   
     ' join BlastActivityBounces babc on babc.BlastID = baus.BlastID '+  
     ' join BounceCodes bc on bc.BounceCodeID = babc.BounceCodeID '+   
     ' join ecn5_communicator..Emails e on e.emailID = baus.emailID ' + ' WHERE b.CustomerID = ' + Convert(varchar,@CustomerID) + ''   
  IF(LEN(@actionvalue) > 0 and @actioncode = '''bounce''')   
  BEGIN  
   SET @sqlstring = @sqlstring + ' AND babc.BounceCode IN ( ' + @actionvalue + ')'   
  END  
     
  SET @sqlstring = @sqlstring + ' AND CONVERT(VARCHAR,eal.ActionDate,101) = CONVERT(VARCHAR, GETDATE()-1,101) ' +   
             ' ORDER BY usc.UnsubscribeCodes, baus.UnsubscribeTime '   
  PRINT (@sqlstring)  
  EXEC (@sqlstring)  
 END  
 ELSE  
 BEGIN  
    SET @sqlstring=        
     'SELECT baus.blastID, e.EmailAddress, baus.EmailID, '+       
     ' (CASE usc.UnsubscribeCode '+     
       ' WHEN ''subscribe''     THEN ''UnSubscribe''   '+     
       ' WHEN ''MASTSUP_UNSUB''    THEN ''UnSubscribe''   '+     
       ' WHEN ''ABUSERPT_UNSUB''  THEN ''UnSubscribe''   '+    
       ' WHEN ''FEEDBACK_UNSUB''   THEN ''UnSubscribe''   '+    
      ' ELSE usc.UnsubscribeCode END) AS ActionType, '+     
     ' (CASE bc.BounceCode '+  
      '  WHEN ''hardbounce''     THEN ''hard''  '+   
      '  WHEN ''softbounce''     THEN ''soft'' '+   
      '  WHEN ''spamnotification''    THEN ''blocked''  '+  
      '  WHEN ''autoresponder''    THEN ''notify'' '+    
      '  WHEN ''U''     THEN ''U''  '+  
      ' ELSE ''unknown'' END) AS ActionValue,  '+  
     ' baus.UnsubscribeTime as ActionDate, baus.Comments as ActionNotes '+  
     ' FROM ecn5_communicator..[BLAST] b join '+  
     ' BlastActivityUnSubscribes baus on baus.blastID = b.blastID '+    
     ' join UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID '+   
     ' join BlastActivityBounces babc on babc.BlastID = baus.BlastID '+  
     ' join BounceCodes bc on bc.BounceCodeID = babc.BounceCodeID '+   
     ' join ecn5_communicator..Emails e on e.emailID = baus.emailID ' + ' WHERE b.CustomerID = ' + Convert(varchar,@CustomerID) + ''   
  IF(LEN(@actionvalue) > 0 and @actioncode = '''bounce''')   
  BEGIN  
   SET @sqlstring = @sqlstring + ' AND babc.BounceCode IN ( ' + @actionvalue + ')'   
  END   
     
  SET @sqlstring = @sqlstring + ' AND baus.UnsubscribeTime BETWEEN DATEADD(dd,' +  CONVERT(VARCHAR(10),-1 * (@daysBlock)) + ',GETDATE()) AND DATEADD(dd,-1,getDate()) ' +      
           ' ORDER BY usc.UnsubscribeCodes, baus.UnsubscribeTime '  
  PRINT (@sqlstring)  
  EXEC (@sqlstring)  
 END  
END
