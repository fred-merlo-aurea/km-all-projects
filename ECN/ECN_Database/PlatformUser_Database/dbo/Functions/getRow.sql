CREATE FUNCTION [dbo].[getRow](@start int,@end int,@splitString varchar(500))
RETURNS varchar(200)
AS
BEGIN
	DECLARE @testString varchar(200)
	DECLARE @tempDB TABLE (id int IDENTITY(1,1) PRIMARY KEY, string varchar(200))
	INSERT INTO @tempDB SELECT * FROM dbo.fn_Split(@splitString, ':')
	SELECT @testString = 
	string FROM (
		SELECT string, ROW_NUMBER() OVER (ORDER BY id) AS RowNum
		FROM @tempDB
	) AS MyDerivedTable
	WHERE MyDerivedTable.RowNum BETWEEN @start and @end
	
	RETURN @testString
	--DECLARE @testString varchar(50)
	--DECLARE @tempDB TABLE (id varchar(200))
	--INSERT INTO @tempDB SELECT * FROM dbo.fn_Split(@splitString, ':')
	--SELECT @testString = id FROM (
 --   SELECT id,
 --   ROW_NUMBER() OVER (ORDER BY id DESC) AS 'RowNumber'
 --   FROM @tempDB 
	--) as DerivedTable
	--WHERE DerivedTable.RowNumber = @start;
	--RETURN @testString
END
GO
