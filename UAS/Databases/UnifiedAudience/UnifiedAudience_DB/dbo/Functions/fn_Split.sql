
CREATE FUNCTION [dbo].[fn_Split](@String varchar(8000), @Delimiter char(1))   
RETURNS   
  @Results TABLE (Items nvarchar(4000))   
AS       
  
BEGIN       
  
	DECLARE @INDEX INT,       
 	 		@SLICE 	nvarchar(4000)       
  
	SELECT @INDEX = 1       

	IF @String IS NULL RETURN       
  
	set @string = ltrim(rtrim(@string))
  
	 WHILE @INDEX !=0           
	 BEGIN  
		
		SELECT @INDEX = CHARINDEX(@Delimiter,@STRING)           

		IF @INDEX !=0           
			SELECT @SLICE = LEFT(@STRING,@INDEX - 1)
		ELSE           
			SELECT @SLICE = @STRING           

		if @SLICE <> ''      
			  INSERT INTO @Results(Items) VALUES(@SLICE)           

		SELECT @STRING = RIGHT(@STRING,LEN(@STRING) - @INDEX)           

		IF LEN(@STRING) = 0   
			BREAK       
	END    
   
	RETURN   

END