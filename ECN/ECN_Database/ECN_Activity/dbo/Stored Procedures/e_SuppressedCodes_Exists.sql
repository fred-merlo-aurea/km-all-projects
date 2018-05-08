CREATE PROCEDURE [dbo].[e_SuppressedCodes_Exists]
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 SuppressedCodeID from SuppressedCodes with (nolock)) SELECT 1 ELSE SELECT 0
END
