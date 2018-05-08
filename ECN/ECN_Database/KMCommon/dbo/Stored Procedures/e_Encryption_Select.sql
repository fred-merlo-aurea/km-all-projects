CREATE PROCEDURE [dbo].[e_Encryption_Select]
AS
SELECT * 
FROM 
	Encryption WITH(NOLOCK) 
ORDER BY 
	IsActive DESC, IsCurrent DESC
