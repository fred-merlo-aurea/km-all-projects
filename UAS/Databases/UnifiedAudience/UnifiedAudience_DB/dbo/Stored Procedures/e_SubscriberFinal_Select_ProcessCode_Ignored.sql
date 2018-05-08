CREATE PROCEDURE [dbo].[e_SubscriberFinal_Select_ProcessCode_Ignored]
	@ProcessCode varchar(50),
	@isIgnored bit
AS
BEGIN

	SET NOCOUNT ON
	
	Select SF.*
	from SubscriberFinal SF
	where SF.ProcessCode = @ProcessCode AND SF.Ignore = @isIgnored

END
