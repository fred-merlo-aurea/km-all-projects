CREATE  PROC [dbo].[e_Layout_Exists_TemplateID] 
(
	@TemplateID int = NULL
)
AS 
BEGIN
	IF EXISTS (
		SELECT 
			TOP 1 LayoutID
		FROM 
			Layout WITH (NOLOCK)
		WHERE 
			TemplateID = @TemplateID and 
			IsDeleted = 0
	) SELECT 1 ELSE SELECT 0
END
