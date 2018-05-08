CREATE PROCEDURE [dbo].[o_GetProfileFields]
AS
BEGIN

	SET NOCOUNT ON

	IF 1=0 BEGIN
    SET FMTONLY OFF
	END
	
	SELECT COLUMN_NAME as DisplayName, 100 as DisplayOrder
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE TABLE_NAME = 'PubSubscriptions' AND COLUMN_NAME in ('RegionCode', 'County', 'Country', 'Company', 'Title', 'ZipCode', 'City', 'Age', 'Income', 'Gender')
	UNION
	SELECT 'SCF', 100
	ORDER BY DisplayOrder, DisplayName
END