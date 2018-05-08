CREATE FUNCTION [dbo].[fn_AddCommas]
(
	@String varchar(8000),
	@PubName varchar(100),
	@ResponseGroup varchar(100)
)     
RETURNS VARCHAR(1000)
AS         
    
BEGIN         

	DECLARE @INDEX INT,
			@return varchar(1000),
			@val varchar(10)	      

	SET @return = ''
	
	IF @String is null
		RETURN @return        

	if len(ltrim(rtrim(@string))) = 0
		RETURN @return        

	set @string = replace(@string, ' ', '')

	DECLARE @T TABLE (value VARCHAR(100))
	INSERT INTO @T 
	select responsevalue from codesheet c join pubs p on c.pubID = p.pubID where p.pubname= @PubName and Responsegroup = @ResponseGroup and dbo.isReallyInteger(responsevalue) <> 1

	if exists (select * from @t)
	Begin
		     
		DECLARE c_SUB CURSOR FOR SELECT value from @T
	  
		OPEN c_SUB  
	   
		FETCH NEXT FROM c_SUB INTO @val
	   
		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			SET @String = REPLACE(@String, @val, @val + ',') 

			FETCH NEXT FROM c_SUB INTO @val
		End  
	   	
		if right(@string, 1) = ','
			set @string = substring(@string, 1, len(@string)-1)


		CLOSE c_SUB  
		DEALLOCATE c_SUB  
	End

	SET @return = @String
	     
	RETURN @return

END