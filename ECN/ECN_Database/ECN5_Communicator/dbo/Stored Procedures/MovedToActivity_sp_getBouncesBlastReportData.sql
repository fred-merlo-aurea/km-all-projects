CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getBouncesBlastReportData] (      
 @blastID varchar(10),      
 @imagePath varchar(100),      
 @bounceType varchar(25),      
 @ISP varchar(100)      
)      
as      
Begin
		INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getBouncesBlastReportData', GETDATE())       
 declare @groupID int      
      
 select @groupID = groupID from [BLAST] where BlastID = @blastID      
 
 if(len(@ISP) <> 0)
 BEGIN 
 if lower(@bounceType) = '*'      
 Begin      
  SELECT        
         eal.EMailID,       
    e.EmailAddress,       
    eal.ActionDate,       
    eal.ActionValue,       
    eal.ActionNotes,       
    @imagePath+'/resend.gif' As ResendImage,       
    'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'       
  FROM       
    Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID         
  WHERE       
    eal.BlastID= @blastID AND ActionTypeCode='bounce'  AND      
    e.emailaddress like '%' + @ISP        
  ORDER BY eal.EAID DESC      
  end       
  else       
 Begin      
  SELECT  eal.EMailID,       
    e.EmailAddress, eal.ActionDate, eal.ActionValue, eal.ActionNotes,       
    @imagePath + '/resend.gif' As ResendImage,       
    'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'         
  FROM       
    Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID       
  WHERE       
    eal.BlastID= @blastID AND       
    ActionTypeCode='bounce' AND       
    ActionValue = @bounceType AND      
    e.emailaddress like '%' + @ISP      
  ORDER BY eal.EAID DESC      
 end      
 END
 
 ELSE 

 BEGIN
     
  SELECT e.EMailID,       
    e.EmailAddress,       
    inn.ActionDate,       
    inn.ActionValue,       
    inn.ActionNotes,       
    @imagePath+'/resend.gif' As ResendImage,       
    'EmailID='+CONVERT(VARCHAR,e.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL' 
  FROM Emails e JOIN 
  ( 
	  SELECT        
	    eal.EMailID,       
	    e.EmailAddress,       
	    eal.ActionDate,       
	    eal.ActionValue,       
	    eal.ActionNotes       
	     
	  FROM       
	    Emails e JOIN EmailActivityLog eal on e.EMailID=eal.EMailID         
	  WHERE       
	    eal.BlastID= @blastID AND ActionTypeCode='bounce' AND ActionValue = (CASE WHEN @bounceType = '*' THEN ActionValue ELSE @bounceType END)
	  ) inn ON e.EmailID = inn.EmailID
          ORDER BY ActionDate DESC
 
 END    

End
