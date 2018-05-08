CREATE PROCEDURE e_DeliverabilityMap_Select_DeliverabilityID_PublicationID
@DeliverabilityID int,
@PublicationID int
AS
	SELECT * FROM DeliverabilityMap With(NoLock) WHERE DeliverabilityID = @DeliverabilityID AND PublicationID = @PublicationID
