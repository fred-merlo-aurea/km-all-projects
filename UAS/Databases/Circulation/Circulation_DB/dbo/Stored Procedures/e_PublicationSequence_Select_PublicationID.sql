
CREATE PROCEDURE e_PublicationSequence_Select_PublicationID
@PublicationID int
AS
	SELECT * 
	FROM PublicationSequence With(NoLock)
	WHERE PublicationID = @PublicationID
