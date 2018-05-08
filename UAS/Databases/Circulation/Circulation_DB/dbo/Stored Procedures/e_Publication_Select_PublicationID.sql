CREATE PROCEDURE e_Publication_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM Publication With(NoLock)
	WHERE PublicationID = @PublicationID
