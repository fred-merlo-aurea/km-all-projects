
CREATE PROCEDURE e_MarketingMap_Select_PublicationID
@PublicationID int
AS
	SELECT * FROM MarketingMap With(NoLock) WHERE PublicationID = @PublicationID
