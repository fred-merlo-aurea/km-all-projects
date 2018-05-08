
CREATE PROCEDURE [e_Product_Select_PubID]
@PubID int
AS
BEGIN

	set nocount on
	
	SELECT *
	FROM Pubs With(NoLock)
	WHERE PubID = @PubID

end