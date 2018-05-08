CREATE PROCEDURE [dbo].[e_SuppressionFile_Select]
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT * 
	FROM SuppressionFile WITH(NOLOCK)

END
go