CREATE PROCEDURE e_DeliverabilityMap_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM DeliverabilityMap With(NoLock) WHERE PublicationID = @PublicationID
