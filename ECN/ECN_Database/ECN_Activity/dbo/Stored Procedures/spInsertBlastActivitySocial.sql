CREATE proc [dbo].[spInsertBlastActivitySocial] (
	@BlastID int,
	@EmailID int = null,
	@RefEmailID int = null,
	@SocialActivityCodeID int,
	@URL varchar(2048),
	@SocialMediaID int)
as

BEGIN
 	SET NOCOUNT ON
 	
	IF NOT EXISTS 
				(
					SELECT TOP 1 SocialID 
					FROM BlastActivitySocial 
					WHERE 
						BlastID = @BlastID AND
						((@EmailID is not null and EmailID = @EmailID) or (@RefEmailID is not null and RefEmailID = @RefEmailID)) AND 
						SocialActivityCodeID = @SocialActivityCodeID AND 
						SocialMediaID = @SocialMediaID AND
						DATEDIFF(ss,ActionDate,GETDATE()) <= 5
				)  
 
	INSERT INTO BlastActivitySocial
		(BlastID, EmailID, RefEmailID, SocialActivityCodeID, ActionDate, URL, SocialMediaID)
		VALUES
		(@BlastID, @EmailID, @RefEmailID, @SocialActivityCodeID, GETDATE(), @URL, @SocialMediaID)
	SELECT @@IDENTITY 
END

select top 100 * from BlastActivitySocial order by ActionDate desc