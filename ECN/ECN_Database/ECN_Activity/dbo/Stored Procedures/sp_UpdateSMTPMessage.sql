CREATE PROCEDURE [dbo].[sp_UpdateSMTPMessage] 
	(
	@logdata xml	
	)
AS
BEGIN
	
	--declare @LogFileData TABLE (EmailID int, BlastID int, SMTPMessage varchar(255),SourceIP  varchar(50));	


	--insert into tmp_UpdateSMTPMessage
	--SELECT EmailValues.Email.value('./@EmailID','INT'),
	--EmailValues.Email.value('./@BlastID','INT'),
	--EmailValues.Email.value('./@SMTPMessage','varchar(255)'),	
	--EmailValues.Email.value('./@SourceIP','varchar(50)')
	--FROM @logdata.nodes('/Email') as EmailValues(Email) ;

		
	insert into SMTPMessage_LogFileData(EmailID, BlastID, SMTPMessage, SourceIP)
	SELECT EmailValues.Email.value('./@EmailID','INT'),
	EmailValues.Email.value('./@BlastID','INT'),
	EmailValues.Email.value('./@SMTPMessage','varchar(255)'),	
	EmailValues.Email.value('./@SourceIP','varchar(50)')
	FROM @logdata.nodes('/Email') as EmailValues(Email) ;


	--UPDATE BlastActivitySends
	--SET 
	--	BlastActivitySends.SMTPMessage = lf.SMTPMessage,
	--	BlastActivitySends.SourceIP=lf.SourceIP
	--FROM 
	--	BlastActivitySends
	--INNER JOIN 
	--	@LogFileData lf
	--ON 
	--	BlastActivitySends.BlastID = lf.BlastID 
	--	and BlastActivitySends.EmailID = lf.EmailID ;		

END

