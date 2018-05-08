CREATE PROCEDURE [dbo].[sp_UpdateSMTP] 
	(
	@logdata xml	
	)
AS
BEGIN
	
	
	insert into SmtpLog (EmailID, BlastID,  dlvSourceIp, timeLogged, Timequeued, jobid, vmtaPool)
	SELECT EmailValues.Email.value('./@EmailID','INT'),
	EmailValues.Email.value('./@BlastID','INT'),	
	EmailValues.Email.value('./@SourceIP','varchar(50)'),
	EmailValues.Email.value('./@Timelogged','DATETIME'),
	EmailValues.Email.value('./@Timequeued','DATETIME'),
	EmailValues.Email.value('./@jobid','INT'),
	EmailValues.Email.value('./@vmtapool','varchar(100)')
	FROM @logdata.nodes('/Email') as EmailValues(Email) ;
	

END
