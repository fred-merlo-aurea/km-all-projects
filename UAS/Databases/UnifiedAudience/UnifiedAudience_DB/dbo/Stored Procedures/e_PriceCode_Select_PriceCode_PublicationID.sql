CREATE PROCEDURE e_PriceCode_Select_PriceCode_PublicationID
@PriceCode varchar(50),
@PublicationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM PriceCode With(NoLock)
	WHERE PriceCodes = @PriceCode AND PublicationID = @PublicationID AND IsActive = 'true'

END