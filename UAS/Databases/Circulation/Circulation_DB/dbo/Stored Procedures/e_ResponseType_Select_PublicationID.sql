CREATE PROCEDURE e_ResponseType_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM ResponseType With(NoLock) WHERE PublicationID = @PublicationID
