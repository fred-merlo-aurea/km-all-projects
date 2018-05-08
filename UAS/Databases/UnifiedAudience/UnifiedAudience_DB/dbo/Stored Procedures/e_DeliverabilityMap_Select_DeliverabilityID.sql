CREATE PROCEDURE e_DeliverabilityMap_Select_DeliverabilityID
@DeliverabilityID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM DeliverabilityMap With(NoLock) 
	WHERE DeliverabilityID = @DeliverabilityID

END