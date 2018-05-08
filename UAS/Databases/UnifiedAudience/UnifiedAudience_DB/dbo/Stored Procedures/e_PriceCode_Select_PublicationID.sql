CREATE PROCEDURE [dbo].[e_PriceCode_Select_PublicationID]
@PublicationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM PriceCode With(NoLock)
	WHERE PublicationID = @PublicationID AND IsActive = 'true'

END