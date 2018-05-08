CREATE PROCEDURE e_ResponseType_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM ResponseType With(NoLock)

END