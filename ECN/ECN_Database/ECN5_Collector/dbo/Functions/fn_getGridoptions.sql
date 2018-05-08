create function [dbo].[fn_getGridoptions](@QuestionID int)
RETURNS VARCHAR(8000)
AS       
 
BEGIN       
  
	declare @options varchar(8000)
	select @options = ''
	select @options = case when @options = '' then GridStatement  else + @options + ' | ' + GridStatement end from grid_statements where QuestionID=@QuestionID

	RETURN @options
END
