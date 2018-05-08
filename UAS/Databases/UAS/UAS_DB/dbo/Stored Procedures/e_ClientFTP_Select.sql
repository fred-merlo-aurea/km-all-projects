CREATE PROCEDURE [e_ClientFTP_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM ClientFTP With(NoLock)

END