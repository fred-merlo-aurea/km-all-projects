CREATE PROCEDURE e_Response_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM Response With(NoLock) WHERE PublicationID = @PublicationID
