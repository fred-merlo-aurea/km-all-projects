CREATE PROCEDURE [e_ClientCustomProcedure_Select_ClientID] 
@ClientID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM ClientCustomProcedure With(NoLock)
	Where ClientID = @ClientID

END