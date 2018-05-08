CREATE  PROCEDURE [dbo].[spDownloadBouncesData] (
	@blastID varchar(10),
	@bounceType varchar(15),
	@ISP varchar(100)
)
as
Begin
	SELECT 
			e.EmailAddress as EmailAddress, 
			bab.BounceTime as BounceTime, 
			bc.BounceCode as BounceType, 
			bab.BounceMessage as BounceSignature 
	FROM 
			BlastActivityBounces bab with (nolock) 
			join ecn5_communicator..Emails e with (nolock) on bab.EMailID=e.EMailID 
			join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID
	WHERE 
			e.emailaddress like '%' + @ISP and
			bab.BounceID in 
			(
				select 	max(BounceID) as EAID
				FROM 
						BlastActivityBounces bab with (nolock)
						join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID
				WHERE  
						BlastID = @blastID AND
						bc.BounceCode = (case when len(ltrim(rtrim(@bounceType))) > 0 then @bounceType else bc.BounceCode end)
				group by emailID
			)	
			
	ORDER BY bab.BounceTime DESC
End
