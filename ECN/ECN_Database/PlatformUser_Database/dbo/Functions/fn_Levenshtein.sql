﻿--CREATE FUNCTION [dbo].[fn_Levenshtein](@S1 [nvarchar](4000), @S2 [nvarchar](4000))
--RETURNS [float] WITH EXECUTE AS CALLER
--AS 
----EXTERNAL NAME [UserFunctions].[StoredFunctions].[Levenshtein]
--GO