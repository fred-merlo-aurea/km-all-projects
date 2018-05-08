create PROCEDURE dbo.o_ReportFilter_Select_PublicationID
@PublicationID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT DISTINCT  ResponseTypeID, ResponseTypeName, DisplayName, DisplayOrder 
	FROM ResponseType With(NoLock)
	WHERE PublicationID = @PublicationID 
		AND ResponseTypeName NOT IN ('DEMO7','EXPIRE') 
		and IsActive = 1
	ORDER BY DisplayOrder

END