CREATE  PROCEDURE [dbo].[MovedToActivity_sp_DownloadBouncesData] (
	@blastID varchar(10),
	@bounceType varchar(15),
	@ISP varchar(100)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_DownloadBouncesData', GETDATE()) 
	SELECT 
			e.EmailAddress as EmailAddress, 
			eal.ActionDate as BounceTime, 
			eal.ActionValue as BounceType, 
			eal.ActionNotes as BounceSignature 
	FROM 
			EmailActivityLog eal join Emails e on eal.EMailID=e.EMailID 
	WHERE 
			e.emailaddress like '%' + @ISP and
			eal.eaid in 
			(
				select 	max(EAID) as EAID
				FROM 
						EmailActivityLog 
				WHERE  
						BlastID = @blastID AND ActionTypeCode='bounce' 
						AND ActionValue= (case when len(ltrim(rtrim(@bounceType))) > 0 then @bounceType else ActionValue end)
				group by emailID
			)	
			
	ORDER BY ActionDate DESC
End
