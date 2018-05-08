CREATE FUNCTION [dbo].[fn_ValidateEmailAddress] (@EmailAddr varchar(255))   
	RETURNS BIT

AS 
BEGIN
	--address cannot be null or empty
	IF @EmailAddr IS NULL OR LEN(LTRIM(RTRIM(@EmailAddr))) <= 0 
		RETURN(0)
		
	SET @EmailAddr = LTRIM(RTRIM(@EmailAddr))

	--address must have only one '@', no spaces and not have two periods in a row('..')
	IF LEN(@EmailAddr) - LEN(REPLACE(@EmailAddr, '@', '')) != 1 OR NOT @EmailAddr LIKE '_%@_%.__%'  OR CHARINDEX(' ',LTRIM(RTRIM(@EmailAddr))) > 0 OR LEN(@EmailAddr) - LEN(REPLACE(@EmailAddr, '..', '')) > 0
		RETURN(0)
		
	DECLARE @EmailLocal VARCHAR(100)
	DECLARE @EmailDomain VARCHAR(100)
	SET @EmailLocal = SUBSTRING(@EmailAddr,1,(CHARINDEX('@',@EmailAddr)) - 1)
	SET @EmailDomain = SUBSTRING(@EmailAddr,(CHARINDEX('@',@EmailAddr)) + 1, LEN(@EmailAddr) - CHARINDEX('@',@EmailAddr))

	--check Local
	--Local must not start or end with a period('.')
	IF SUBSTRING(@EmailLocal, 1, 1) = '.' OR SUBSTRING(@EmailLocal, LEN(@EmailLocal), 1) = '.'
		RETURN(0)

	--only allow the following characters
	DECLARE @ValidChars VARCHAR(100)
	DECLARE @Max INT -- Length of the address
	DECLARE @Pos INT -- Position in @EmailAddr
	DECLARE @OK BIT  -- Is @EmailAddr OK
	SET @ValidChars = '.abcdefghijklmnopqrstuvwxyz0123456789!#$&*+-/=?^_`{|}~''' --also should allow%
	SET @Max = LEN(@EmailLocal)
	SET @Pos = 0
	SET @OK = 1

	WHILE @Pos < @Max AND @OK = 1 
	BEGIN
		SET @Pos = @Pos + 1
		IF NOT @ValidChars LIKE '%' + SUBSTRING(@EmailLocal, @Pos, 1) + '%' 
			SET @OK = 0
	END

	IF @OK = 0
		RETURN(0)	
		
	--check Domain
	--Domain must not start or end with either a period('.') or a hyphen('-')
	IF SUBSTRING(@EmailDomain, 1, 1) = '-' OR SUBSTRING(@EmailDomain, LEN(@EmailDomain), 1) = '-' OR SUBSTRING(@EmailDomain, 1, 1) = '.' OR SUBSTRING(@EmailDomain, LEN(@EmailDomain), 1) = '.'
		RETURN(0)

	--only allow the following characters	
	SET @ValidChars = '.abcdefghijklmnopqrstuvwxyz0123456789-'
	SET @Max = LEN(@EmailDomain)
	SET @Pos = 0
	SET @OK = 1

	WHILE @Pos < @Max AND @OK = 1 
	BEGIN
		SET @Pos = @Pos + 1
		IF NOT @ValidChars LIKE '%' + SUBSTRING(@EmailDomain, @Pos, 1) + '%' 
			SET @OK = 0
	END

	IF @OK = 0
		RETURN(0)
		
	RETURN(1)
END
