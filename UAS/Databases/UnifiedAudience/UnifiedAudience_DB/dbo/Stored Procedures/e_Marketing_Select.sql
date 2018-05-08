CREATE PROCEDURE e_Marketing_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT * FROM Marketing With(NoLock) 

END