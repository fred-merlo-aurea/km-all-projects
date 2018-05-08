CREATE PROCEDURE e_DeliverabilityMap_Select_DeliverabilityID
@DeliverabilityID int
AS
	SELECT * FROM DeliverabilityMap With(NoLock) WHERE DeliverabilityID = @DeliverabilityID
