CREATE function [dbo].[RemoveDups](@String nvarchar(max), @Delim char) returns nvarchar(max)
as
BEGIN
  DECLARE @FinalString nvarchar(max)
  DECLARE @Word nvarchar(max)
  DECLARE @Return nvarchar(max)
  SET @FinalString = ''

	IF (@String is null)
	BEGIN
		SET @String  = ''
		SET @Return = ''
	END
	Else
		BEGIN
		  WHILE len(@String) > 0
		  BEGIN
			-- Get the first word
			SET @Word = left(@String, charindex('' + @Delim + '', @String + '' + @Delim + '') - 1)

			-- Add word to result if not already added
			IF '' + @Delim + '' + @FinalString not like '%' + @Delim + '' + @Word + '' + @Delim + '%'
			BEGIN
			  SET @FinalString = @FinalString + @Word + '' + @Delim + ''
			END 

			-- Remove first word
			SET @String = stuff(@String, 1, charindex('' + @Delim + '', @String + '' + @Delim + ''), '')
		  END

		  SET @Return = left(@FinalString, len(@FinalString)- 1)
		END
		RETURN @Return
END
GO