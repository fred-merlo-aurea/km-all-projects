CREATE PROCEDURE [dbo].[e_Filter_Select_FilterID]   
@FilterID int = NULL
AS
	SELECT * FROM Filter WITH (NOLOCK) WHERE FilterID = @FilterID and IsDeleted = 0