CREATE function [dbo].[fn_getOptions](@QuestionID int)
RETURNS VARCHAR(8000)
AS       
 
BEGIN       
  
	declare @options varchar(8000)
	select @options = ''
	select @options = case when @options = '' then OptionValue  else + @options + ' | ' + OptionValue end from response_options where QuestionID=@QuestionID

	RETURN @options
END
