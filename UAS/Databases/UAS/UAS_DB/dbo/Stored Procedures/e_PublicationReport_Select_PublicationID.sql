CREATE PROCEDURE [dbo].[e_PublicationReport_Select_PublicationID]
@ClientID int
AS
BEGIN

	set nocount on

	SELECT r.* 
	FROM PublicationReport r with(nolock)
	JOIN Client c with(nolock) ON r.PublicationID = c.ClientID 
	WHERE isnull(r.status,1) = 1 and p.PublicationID = @ClientID 
	ORDER BY ReportName

END
go