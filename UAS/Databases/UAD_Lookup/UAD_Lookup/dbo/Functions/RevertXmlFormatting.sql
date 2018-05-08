CREATE FUNCTION [dbo].[RevertXmlFormatting]
(@Str varchar(max))
RETURNS varchar(max) AS
BEGIN
	DECLARE @Result varchar(max) = @Str

	set @Result = replace(@Result, '&amp;', '&')
	set	@Result = replace(@Result, '&quot;', '"' )
	set @Result = replace(@Result, '&lt;', '<')
	set	@Result = replace(@Result, '&gt;', '>')
	set	@Result = replace(@Result, '&apos;', '''')

	RETURN @Result
END
GO
