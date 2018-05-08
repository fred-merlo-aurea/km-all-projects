CREATE PROCEDURE [dbo].[e_Issue_Select_PublisherID]
@PublisherID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM Issue With(NoLock)
	WHERE Issue.IsComplete = 0

END