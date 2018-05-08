CREATE PROCEDURE [dbo].[spEmailDataValuesDelete]
@DataValue varchar(500)
AS
DELETE FROM [EmailDataValues]
WHERE DataValue = @DataValue
