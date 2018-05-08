CREATE PROCEDURE [dbo].[e_SmartFormsPrePopFields_Exists_ByProfileFieldName] 
	@SFID int = NULL,
	@PrePopFieldID int = NULL,
	@ProfileFieldName varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 PrePopFieldID FROM SmartFormsPrePopFields WITH (NOLOCK) WHERE SFID = @SFID AND PrePopFieldID != ISNULL(@PrePopFieldID, -1) AND ProfileFieldName = @ProfileFieldName AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END