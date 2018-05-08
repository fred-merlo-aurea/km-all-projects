CREATE PROCEDURE e_DeliverabilityMap_Select_PublicationID
@PublicationID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM DeliverabilityMap With(NoLock) 
	WHERE PublicationID = @PublicationID

END