CREATE FUNCTION [dbo].[isReallyInteger]    
(    
    @num VARCHAR(64)    
)    
RETURNS BIT    
BEGIN    
    IF LEFT(@num, 1) = '-'    
        SET @num = SUBSTRING(@num, 2, LEN(@num))    
   
    RETURN CASE    
    WHEN PATINDEX('%[^0-9-]%', @num) = 0    
        AND CHARINDEX('-', @num) <= 1    
        AND @num NOT IN ('.', '-', '+', '^')   
        AND LEN(@num)>0    
        AND @num NOT LIKE '%-%'   
    THEN    
        1    
    ELSE    
        0    
    END    
END