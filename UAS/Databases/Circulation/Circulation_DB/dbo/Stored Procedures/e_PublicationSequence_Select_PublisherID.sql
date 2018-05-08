CREATE PROCEDURE e_PublicationSequence_Select_PublisherID
@PublisherID int
AS
	SELECT ps.* 
	FROM PublicationSequence ps With(NoLock)
	JOIN Publication p With(NoLock) ON ps.PublicationID = ps.PublicationID
	WHERE p.PublisherID = @PublisherID
