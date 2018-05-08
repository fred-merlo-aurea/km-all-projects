CREATE PROCEDURE [e_ClientFTP_Select_ClientID] @ClientID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM ClientFTP With(NoLock)
	WHERE ClientID = @ClientID
	AND IsDeleted = 0

END