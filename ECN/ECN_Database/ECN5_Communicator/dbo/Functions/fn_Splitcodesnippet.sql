CREATE FUNCTION [dbo].[fn_Splitcodesnippet](@String varchar(8000), @Delimiter char(2))   
RETURNS   
  @Results TABLE (Items varchar(4000))   
AS       
 
BEGIN       
  
	DECLARE @INDEX INT,       
	  @SLICE nvarchar(4000)       
	  
	-- HAVE TO SET TO 1 SO IT DOESNT EQUAL Z       
	--     ERO FIRST TIME IN LOOP       
	
	select @string = ltrim(rtrim(@string))
  
	SELECT @INDEX = 1       
	-- following line added 10/06/04 as null       
	--      values cause issues       
	IF @String IS NULL RETURN       
	  
		WHILE @INDEX !=0           
		BEGIN  
			-- GET THE INDEX OF THE FIRST OCCURENCE OF THE SPLIT CHARACTER           
			SELECT @INDEX = CHARINDEX(@Delimiter,@STRING)        
			SELECT @STRING = RIGHT(@STRING,LEN(@STRING) - (@INDEX+1))
			SELECT @INDEX = CHARINDEX(@Delimiter,@STRING)           
		 

			IF @INDEX !=0           
			Begin
				SELECT @SLICE = LEFT(@STRING,(@INDEX-1))           

				-- PUT THE ITEM INTO THE RESULTS SET  
				if not exists (select items from @results where items = @slice)         
					INSERT INTO @Results(Items) VALUES(@SLICE)           

				-- CHOP THE ITEM REMOVED OFF THE MAIN STRING           
				SELECT @STRING = RIGHT(@STRING,LEN(@STRING) - (@INDEX+1))           
				-- BREAK OUT IF WE ARE DONE           

				IF LEN(@STRING) = 0   
					BREAK       
			END
		END  
	RETURN    
END
