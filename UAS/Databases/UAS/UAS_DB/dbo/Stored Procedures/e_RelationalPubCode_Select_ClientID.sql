CREATE PROCEDURE e_RelationalPubCode_Select_ClientID
@ClientID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM RelationalPubCode With(NoLock)
	WHERE ClientID = @ClientID 
	ORDER BY SpecialFileName,PubCode

END