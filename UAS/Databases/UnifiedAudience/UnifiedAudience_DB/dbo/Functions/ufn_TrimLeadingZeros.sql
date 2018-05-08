CREATE FUNCTION [dbo].[ufn_TrimLeadingZeros] ( @Input VARCHAR(50) )
RETURNS VARCHAR(50)
AS
BEGIN
    RETURN REPLACE(LTRIM(REPLACE(@Input, '0', ' ')), ' ', '0')
END