CREATE FUNCTION dbo.DistinctList
(
@List VARCHAR(MAX),
@Delim CHAR
)
RETURNS
VARCHAR(MAX)
AS
BEGIN
	DECLARE @ParsedList TABLE
	(
	Item VARCHAR(MAX)
	)
	DECLARE @list1 VARCHAR(MAX), @Pos INT, @rList VARCHAR(MAX)
	IF (@List is NULL)
		BEGIN
			SET @rlist = ''
		END
	ELSE
		BEGIN
			SET @list = LTRIM(RTRIM(@list)) + @Delim
			SET @pos = CHARINDEX(@delim, @list, 1)
			WHILE @pos > 0
			BEGIN
				SET @list1 = LTRIM(RTRIM(LEFT(@list, @pos - 1)))
				IF @list1 <> ''
					INSERT INTO @ParsedList VALUES (CAST(@list1 AS VARCHAR(MAX)))
				SET @list = SUBSTRING(@list, @pos+1, LEN(@list))
				SET @pos = CHARINDEX(@delim, @list, 1)
			END
			SELECT @rlist = COALESCE(@rlist+',','') + item
			FROM (SELECT DISTINCT Item FROM @ParsedList) t
		END
	RETURN @rlist
END