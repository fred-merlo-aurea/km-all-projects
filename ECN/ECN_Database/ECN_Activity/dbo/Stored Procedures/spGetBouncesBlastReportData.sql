CREATE  PROCEDURE [dbo].[spGetBouncesBlastReportData] (      
 @blastID varchar(10),      
 @imagePath varchar(100),      
 @bounceType varchar(25),      
 @ISP varchar(100)      
)      
as      
Begin      
	declare @groupID int      
      
	select @groupID = groupID from ecn5_communicator..[BLAST] with (nolock) where BlastID = @blastID      
 
	if(len(@ISP) <> 0)
	BEGIN 
		if lower(@bounceType) = '*'      
		Begin      
			SELECT        
				bab.EMailID,       
				e.EmailAddress,       
				bab.BounceTime as ActionDate,       
				bc.BounceCode as ActionValue,       
				bab.BounceMessage as ActionNotes,       
				@imagePath+'/resend.gif' As ResendImage,       
				'EmailID='+CONVERT(VARCHAR,bab.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'       
			FROM       
				ecn5_communicator..Emails e with (nolock) 
				JOIN BlastActivityBounces bab with (nolock) on e.EMailID=bab.EMailID 
				join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID        
			WHERE       
				bab.BlastID= @blastID AND     
				e.emailaddress like '%' + @ISP        
			ORDER BY 
				BounceID DESC      
			end       
			else       
			Begin
			SELECT        
				bab.EMailID,       
				e.EmailAddress,       
				bab.BounceTime as ActionDate,       
				bc.BounceCode as ActionValue,       
				bab.BounceMessage as ActionNotes,       
				@imagePath+'/resend.gif' As ResendImage,       
				'EmailID='+CONVERT(VARCHAR,bab.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'       
			FROM       
				ecn5_communicator..Emails e with (nolock) 
				JOIN BlastActivityBounces bab with (nolock) on e.EMailID=bab.EMailID 
				join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID        
			WHERE       
				bab.BlastID= @blastID AND
				bc.BounceCode = @bounceType AND     
				e.emailaddress like '%' + @ISP        
			ORDER BY 
				BounceID DESC   
		end      
	END	 
	ELSE 
	BEGIN 
		 if lower(@bounceType) = '*'      
		 Begin      
			SELECT        
				bab.EMailID,       
				e.EmailAddress,       
				bab.BounceTime as ActionDate,       
				bc.BounceCode as ActionValue,       
				bab.BounceMessage as ActionNotes,       
				@imagePath+'/resend.gif' As ResendImage,       
				'EmailID='+CONVERT(VARCHAR,bab.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'       
			FROM       
				ecn5_communicator..Emails e with (nolock) 
				JOIN BlastActivityBounces bab with (nolock) on e.EMailID=bab.EMailID 
				join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID        
			WHERE       
				bab.BlastID= @blastID        
			ORDER BY 
				BounceID DESC      
		  end       
		else       
		 Begin
			SELECT        
				bab.EMailID,       
				e.EmailAddress,       
				bab.BounceTime as ActionDate,       
				bc.BounceCode as ActionValue,       
				bab.BounceMessage as ActionNotes,       
				@imagePath+'/resend.gif' As ResendImage,       
				'EmailID='+CONVERT(VARCHAR,bab.EmailID)+'&GroupID='+CONVERT(VARCHAR,@groupID) AS 'URL'       
		  FROM       
				ecn5_communicator..Emails e with (nolock) 
				JOIN BlastActivityBounces bab with (nolock) on e.EMailID=bab.EMailID 
				join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID        
		  WHERE       
				bab.BlastID= @blastID AND
				bc.BounceCode = @bounceType       
		  ORDER BY 
				BounceID DESC   
		 end      
	 END 
End
