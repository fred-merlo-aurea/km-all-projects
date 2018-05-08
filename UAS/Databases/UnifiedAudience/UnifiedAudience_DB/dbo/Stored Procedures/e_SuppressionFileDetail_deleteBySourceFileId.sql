CREATE PROCEDURE [dbo].[e_SuppressionFileDetail_deleteBySourceFileId]
	@SuppressionFileId int = 0
AS
BEGIN
	
	SET NOCOUNT ON
	
	DELETE SuppressionFileDetail 
	WHERE SuppressionFileId = @SuppressionFileId

END