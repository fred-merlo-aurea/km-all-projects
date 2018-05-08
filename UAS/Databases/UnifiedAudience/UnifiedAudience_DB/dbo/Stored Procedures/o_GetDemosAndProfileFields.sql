CREATE PROCEDURE o_GetDemosAndProfileFields
	@ProductID int
AS
BEGIN

	SET NOCOUNT ON

	IF 1=0 BEGIN
    SET FMTONLY OFF
	END
	
	SELECT DisplayName, ResponseGroupName, DisplayOrder 
	FROM ResponseGroups
	WHERE PubID = @ProductID and DisplayName <> 'Pubcode' and DisplayName <> 'Subscription'
	--UNION ALL
	--SELECT COLUMN_NAME as DisplayName, 100 as DisplayOrder
	--FROM INFORMATION_SCHEMA.COLUMNS
	--WHERE TABLE_NAME = 'PubSubscriptions' AND COLUMN_NAME in ('RegionCode', 'County', 'Country', 'Company', 'Title', 'ZipCode', 'City', 'Age', 'Income', 'Gender')
	--ORDER BY DisplayOrder, DisplayName
END