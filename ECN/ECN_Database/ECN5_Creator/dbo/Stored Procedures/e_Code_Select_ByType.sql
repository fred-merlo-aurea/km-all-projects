create PROCEDURE [dbo].[e_Code_Select_ByType]   
@CodeType varchar(50)
AS
	SELECT * FROM Code WHERE CodeType = @CodeType AND SystemFlag = 'Y' ORDER BY SortCode