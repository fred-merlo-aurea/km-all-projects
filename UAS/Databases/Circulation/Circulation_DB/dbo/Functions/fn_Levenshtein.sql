CREATE FUNCTION [dbo].[fn_Levenshtein]
(@S1 NVARCHAR (4000), @S2 NVARCHAR (4000))
RETURNS FLOAT (53)
AS
 EXTERNAL NAME [UserFunctions].[StoredFunctions].[Levenshtein]




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Levenshtein] TO [meghan.salim]
    AS [dbo];

