CREATE PROCEDURE [dbo].[spEmailDataValuesSelectCountByDataValue]
@DataValue varchar(500)
AS
SELECT COUNT(*)
  FROM [EmailDataValues]
  WHERE DataValue = @DataValue
