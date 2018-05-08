CREATE PROCEDURE e_RelationalPubCode_Select_ClientID_SpecialFileName
@ClientID int,
@SpecialFileName varchar(250)
AS
BEGIN

	set nocount on

	SELECT *
	FROM RelationalPubCode With(NoLock)
	WHERE ClientID = @ClientID 
	AND SpecialFileName = @SpecialFileName
	ORDER BY PubCode 

END