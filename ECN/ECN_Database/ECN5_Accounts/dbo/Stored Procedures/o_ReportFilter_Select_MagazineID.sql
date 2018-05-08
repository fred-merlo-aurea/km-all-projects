CREATE PROCEDURE dbo.o_ReportFilter_Select_MagazineID
@MagazineID int
AS
	SELECT DISTINCT  r.ResponseGroupID, ResponseGroup, RG.DisplayName, RG.DisplayOrder 
	FROM Responses r With(NoLock) 
	JOIN ResponseGroups rg With(NoLock) ON r.ResponseGroupID = rg.ResponseGroupID 
	WHERE r.MagazineID = @MagazineID 
	AND responsegroup NOT IN ('DEMO7','EXPIRE') 
	AND rg.DisplayOrder IS NOT NULL
	ORDER BY DisplayOrder