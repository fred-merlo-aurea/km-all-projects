CREATE PROCEDURE [dbo].[e_Code_Exists]
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 CodeID from Code with (nolock)) SELECT 1 ELSE SELECT 0
END

