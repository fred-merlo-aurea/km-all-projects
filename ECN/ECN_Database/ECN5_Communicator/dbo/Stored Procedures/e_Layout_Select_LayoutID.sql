CREATE PROCEDURE [dbo].[e_Layout_Select_LayoutID]   
@LayoutID int = NULL
AS
	SELECT * FROM Layout WITH (NOLOCK) WHERE LayoutID = @LayoutID and IsDeleted = 0