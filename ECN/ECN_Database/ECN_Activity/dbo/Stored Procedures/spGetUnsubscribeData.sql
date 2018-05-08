CREATE  PROCEDURE [dbo].[spGetUnsubscribeData] (@blastID varchar(10), @ISP varchar(100), @ActionTypeCode varchar(100))  
as  
  
Begin  
  
 SET NOCOUNT ON  
  
 declare @groupID int  
  
 select @groupID = groupID from ecn5_communicator..[BLAST] where BlastID = @blastID  
if len(ltrim(rtrim(@ISP))) = 0  
 begin  
  SELECT  bau.EMailID,   
    e.EmailAddress as EmailAddress,   
    bau.UnsubscribeTime as UnsubscribeTime, bau.Comments as Reason,  
    uc.UnsubscribeCode as SubscriptionChange,   
    'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'  
  FROM   
    BlastActivityUnSubscribes bau 
    join ecn5_communicator..emails e on bau.emailid = e.emailID  
    join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
  where  
    bau.BlastID=@blastID AND   
    uc.UnsubscribeCode=@ActionTypeCode 
  ORDER BY   
   UnsubscribeID desc  
 end  
 else  
 begin  
  SELECT  bau.EMailID,   
    e.EmailAddress as EmailAddress,   
    bau.UnsubscribeTime as UnsubscribeTime, bau.Comments as Reason,  
    uc.UnsubscribeCode as SubscriptionChange,   
    'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'  
  FROM   
    BlastActivityUnSubscribes bau 
    join ecn5_communicator..emails e on bau.emailid = e.emailID  
    join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
  where  
    bau.BlastID=@blastID AND   
    uc.UnsubscribeCode=@ActionTypeCode AND    
	e.emailaddress like '%' + @ISP  
  ORDER BY   
   UnsubscribeID desc  
 end  
  
 SET NOCOUNT OFF  
End
