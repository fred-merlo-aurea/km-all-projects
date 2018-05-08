CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getUnsubscribeData] (@blastID varchar(10), @ISP varchar(100), @ActionTypeCode varchar(100))  
as  
  
Begin  
  INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getUnsubscribeData', GETDATE())
 SET NOCOUNT ON  
  
 declare @groupID int  
  
 select @groupID = groupID from [BLAST] where BlastID = @blastID  
   
  
 if len(ltrim(rtrim(@ISP))) = 0  
 begin  
  SELECT  eal.EMailID,   
    e.EmailAddress as EmailAddress,   
    eal.ActionDate as UnsubscribeTime, eal.ActionNotes as Reason,  
    eal.ActionValue as SubscriptionChange,   
    'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'  
  FROM   
    EmailActivityLog eal join emails e on eal.emailid = e.emailID  
  where  
    eal.BlastID=@blastID AND   
    eal.ActionTypeCode=@ActionTypeCode      
/*EAL.EAID in   
    (  
    SELECT  EAL.EAID  
    FROM   
      EmailActivityLog eal   
    WHERE   
      eal.BlastID=@blastID AND   
      eal.ActionTypeCode=@ActionTypeCode   
    )*/  
  ORDER BY   
   eal.EAID desc  
 end  
 else  
 Begin  
  SELECT  eal.EMailID,   
    e.EmailAddress as EmailAddress,   
    eal.ActionDate as UnsubscribeTime,   
    eal.ActionValue as SubscriptionChange, eal.ActionNotes as Reason,  
    'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'  
  FROM   
    EmailActivityLog eal join emails e on eal.emailid = e.emailID  
  where  
    eal.BlastID=@blastID AND   
    eal.ActionTypeCode=@ActionTypeCode and  
    e.emailaddress like '%' + @ISP    
  ORDER BY   
   eal.EAID desc  
 End  
  
 SET NOCOUNT OFF  
End
