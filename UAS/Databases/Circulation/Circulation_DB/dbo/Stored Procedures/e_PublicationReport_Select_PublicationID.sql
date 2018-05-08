CREATE PROCEDURE [dbo].[e_PublicationReport_Select_PublicationID]
@PublicationID int
AS
	SELECT r.* FROM Publicationreports r 
	JOIN Publication p ON r.PublicationID = p.PublicationID 
	WHERE isnull(r.status,1) = 1 and p.PublicationID = @PublicationID 
	ORDER BY ReportName
