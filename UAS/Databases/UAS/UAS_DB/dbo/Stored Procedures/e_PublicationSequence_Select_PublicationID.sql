CREATE PROCEDURE e_PublicationSequence_Select_PublicationID
@PublicationID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM PublicationSequence With(NoLock)
	WHERE PublicationID = @PublicationID

END