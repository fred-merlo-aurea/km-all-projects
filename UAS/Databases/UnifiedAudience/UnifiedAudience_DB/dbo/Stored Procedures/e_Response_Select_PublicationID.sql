CREATE PROCEDURE e_Response_Select_PublicationID
@PublicationID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM Response With(NoLock) 
	WHERE PublicationID = @PublicationID

END