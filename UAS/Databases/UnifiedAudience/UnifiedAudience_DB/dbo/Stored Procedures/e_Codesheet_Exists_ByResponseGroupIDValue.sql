create  PROC [dbo].[e_Codesheet_Exists_ByResponseGroupIDValue] 
(
	@ResponseGroupID int,
	@CodeSheetID int,
	@ResponseValue varchar(255) 
)
AS 
BEGIN

	set nocount on

	IF EXISTS (
		SELECT 
			TOP 1 CodeSheetID
		FROM 
			CodeSheet WITH (NOLOCK)
		WHERE 
			ResponseValue = @ResponseValue and CodeSheetID != @CodeSheetID and ResponseGroupID = @ResponseGroupID
	) SELECT 1 ELSE SELECT 0
END