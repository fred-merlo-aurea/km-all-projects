--************************************** --      
-- Name: Split Function -- Description:Enables SQL Server to perform the Split Function in stored procedures/views/functions 
-- By: Sunil 
-- Inputs:A STRING that you would like to 
--   split down into individual elements, based 
--   on the DELIMITER specified -- 
-- Returns:a table with a row for each i 
--     item found between the delimiter you specify 
-- 
CREATE FUNCTION [fn_Split](@String varchar(8000), @Delimiter char(1)) 
RETURNS 
		@Results TABLE (Items nvarchar(4000)) 
AS     

BEGIN     

DECLARE @INDEX INT,     
		@SLICE nvarchar(4000)     

-- HAVE TO SET TO 1 SO IT DOESNT EQUAL Z     
--     ERO FIRST TIME IN LOOP     

SELECT @INDEX = 1     
-- following line added 10/06/04 as null     
--      values cause issues     
IF @String IS NULL RETURN     

	WHILE @INDEX !=0         
	BEGIN
	         
		-- GET THE INDEX OF THE FIRST OCCURENCE OF THE SPLIT CHARACTER         
		SELECT @INDEX = CHARINDEX(@Delimiter,@STRING)         
		-- NOW PUSH EVERYTHING TO THE LEFT OF IT INTO THE SLICE VARIABLE         
		IF @INDEX !=0         
			SELECT @SLICE = LEFT(@STRING,@INDEX - 1)         
		ELSE         
			SELECT @SLICE = @STRING         
		
		-- PUT THE ITEM INTO THE RESULTS SET         
		INSERT INTO @Results(Items) VALUES(@SLICE)         
		-- CHOP THE ITEM REMOVED OFF THE MAIN STRING         
		SELECT @STRING = RIGHT(@STRING,LEN(@STRING) - @INDEX)         
		-- BREAK OUT IF WE ARE DONE         
		IF LEN(@STRING) = 0 
			BREAK     
	END     
	RETURN 
END
