CREATE PROCEDURE [dbo].[e_Issue_Select_PublisherID]
@PublisherID int
AS
	SELECT * FROM Issue With(NoLock)
	WHERE Issue.IsComplete = 0