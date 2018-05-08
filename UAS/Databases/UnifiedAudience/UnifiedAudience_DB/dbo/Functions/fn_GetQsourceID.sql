CREATE FUNCTION [dbo].[fn_GetQsourceID]
(  
	@value varchar(255)  
)   
RETURNS INT  
Begin  
  	declare @ID int  

	select @value = CASE WHEN RTRIM(LTRIM(ISNULL(@value, ''))) = '' THEN '' ELSE RTRIM(LTRIM(@value)) end  
  
	if len(@value) > 1   
		select @value = case when left(@value,1) = '0' and dbo.isReallyInteger(@value) =1 then CONVERT(VARCHAR,CONVERT(INT,@value)) else @value end   
  
	select @ID= codeID 
	from 
			UAD_Lookup..Code 
	where 
			CodeTypeId in (select codetypeID from UAD_Lookup..CodeType where codetypename = 'Qualification Source')
			and codeValue = @value  
   
	RETURN @ID  
  
End