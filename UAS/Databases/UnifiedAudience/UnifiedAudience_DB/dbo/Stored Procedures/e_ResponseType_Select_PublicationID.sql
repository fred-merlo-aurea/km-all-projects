CREATE PROCEDURE e_ResponseType_Select_PublicationID
@PublicationID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM ResponseType With(NoLock) 
	WHERE PublicationID = @PublicationID

END