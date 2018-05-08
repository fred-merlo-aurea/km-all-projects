CREATE PROCEDURE [dbo].[e_Issue_Select_PublicationID]
@PublicationID int
AS
	SELECT * FROM Issue With(NoLock)
	WHERE PublicationId = @PublicationID
	AND Issue.IsComplete = 0