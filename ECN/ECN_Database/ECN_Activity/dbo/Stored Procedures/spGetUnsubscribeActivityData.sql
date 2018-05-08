CREATE PROCEDURE [dbo].[spGetUnsubscribeActivityData]   
(  
 @CustomerID int,  
 @Frequency  varchar(10)  
)  
as  
Begin  
 declare @sqlstring varchar(4000)  
  
 if UPPER(@Frequency) = 'DAILY'  
 begin  
  set @sqlstring =   
    'SELECT e.EmailAddress, ''U'' as ''ActionType'', baus.UnsubscribeTime '+  
    ' FROM BlastActivityUnSubscribes baus '+  
    'JOIN ecn5_communicator..Emails e ON baus.emailID = e.EmailID '+  
    'JOIN UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID '+  
    'WHERE e.CustomerID = 1214 '+       
    'AND (usc.UnsubscribeCode=''subscribe'' OR usc.UnsubscribeCode=''ABUSERPT_UNSUB'' OR usc.UnsubscribeCode=''FEEDBACK_UNSUB'') '+  
    'AND CONVERT(VARCHAR,baus.UnsubscribeTime,101) = CONVERT(VARCHAR, getDate()-1,101) '+  
    'ORDER BY baus.UnsubscribeTime'  
  exec (@sqlstring)  
   
 end  
End
