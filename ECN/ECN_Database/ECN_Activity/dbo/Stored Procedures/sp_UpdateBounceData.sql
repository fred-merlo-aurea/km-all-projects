CREATE PROCEDURE [dbo].[sp_UpdateBounceData] 
	(
	@logdata xml	
	)
AS
BEGIN
	
	declare @LogFileData TABLE (EmailID int, BlastID int, bounceDomain varchar(100),EmailAddress  varchar(100), dsnDiag varchar(2000), bounceCat varchar(100), timeLogged datetime);	
			
	insert into @LogFileData
	SELECT EmailValues.Email.value('./@EmailID','INT'),
	EmailValues.Email.value('./@BlastID','INT'),
	EmailValues.Email.value('./@bounceDomain','varchar(100)'),	
	EmailValues.Email.value('./@EmailAddress','varchar(100)'),
	EmailValues.Email.value('./@dsnDiag','varchar(2000)'),
	EmailValues.Email.value('./@bounceCat','varchar(100)'),
	EmailValues.Email.value('./@timeLogged','datetime')
	FROM @logdata.nodes('/Email') as EmailValues(Email) ;


	insert into tmp_Port25Bounces
	select lf.EmailID, lf.BlastID, lf.bounceDomain, lf.EmailAddress, lf.dsnDiag ,lf.bounceCat, lf.timeLogged from @LogFileData lf

END
