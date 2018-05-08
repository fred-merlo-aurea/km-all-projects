CREATE PROCEDURE [dbo].[e_BlastActivityOpensInternal_Insert]
	@EmailID int,
	@BlastID int,
	@BrowserInfo varchar(2048)
	
AS
	INSERT INTO BlastActivityOpensInternal(EmailID, BlastID, OpenTime, BrowserInfo, EmailClientID, PlatformID)
	VALUES(@EmailID, @BlastID, GETDATE(), @BrowserInfo, [ecn_Activity].dbo.[fn_GetEmailClientID] (@BrowserInfo),[ecn_Activity].dbo.[fn_GetPlatformID] (@BrowserInfo))
	SELECT @@IDENTITY;	
