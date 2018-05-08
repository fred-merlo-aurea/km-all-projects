CREATE PROCEDURE [dbo].[e_ProductTypes_Select_BrandID]
	@BrandID int = 0	
AS
BEGIN
	SELECT DISTINCT 
	pt.PubTypeID, 
	ColumnReference, 
	PubTypeDisplayName,  
	pt.IsActive, 
	pt.SortOrder 
	FROM  Brand b WITH (NOLOCK) 
	JOIN branddetails bd WITH (NOLOCK) ON b.BrandID = bd.BrandID 
	JOIN pubs p WITH (NOLOCK) ON bd.pubid = p.pubid 
	JOIN PubTypes pt  WITH (NOLOCK) ON pt.PubTypeID = p.PubTypeID 
	WHERE b.IsDeleted = 0 AND pt.IsActive = 1 AND b.BrandID = @brandID 
	ORDER BY pt.SortOrder
END
