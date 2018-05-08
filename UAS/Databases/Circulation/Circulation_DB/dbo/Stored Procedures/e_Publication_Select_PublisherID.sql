CREATE PROCEDURE [dbo].[e_Publication_Select_PublisherID]
@PublisherID int
AS
	SELECT * FROM Publication With(NoLock)
	WHERE PublisherID = @PublisherID