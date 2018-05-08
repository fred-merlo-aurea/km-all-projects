CREATE PROCEDURE [dbo].[e_Issue_Select_PublicationID]
@PublicationID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM Issue With(NoLock)
	WHERE PublicationId = @PublicationID AND Issue.IsComplete = 0

END