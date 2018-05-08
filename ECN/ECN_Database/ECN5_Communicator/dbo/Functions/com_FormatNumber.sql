CREATE FUNCTION dbo.com_FormatNumber(@value BIGINT) RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @minus CHAR, @working VARCHAR(MAX), @result VARCHAR(MAX), @section VARCHAR(4)
    -- First, handle the sign
    IF (@value < 0) SET @minus = '-' ELSE SET @minus = ''
    SET @working = CAST(@value AS VARCHAR)
    if (@minus <> '') SET @working = SUBSTRING(@working, 2, LEN(@working)-1)
    
    SET @result  = ''
    
	-- break apart the number into sections of 3 digits and insert commas
    WHILE LEN(@working) > 3
    BEGIN
        SET @section = ',' + SUBSTRING(@working, LEN(@working)-2, 3)
        SET @working = SUBSTRING(@working, 1, LEN(@working)-3)
        SET @result  = @section + @result
    END
    
	-- add the remaining and tack on that sign if needed
    IF (@minus <> '')
        RETURN @minus + @working + @result
        
    RETURN @working + @result
END