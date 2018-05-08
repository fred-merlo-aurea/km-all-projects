CREATE PROCEDURE [dbo].[e_Filter_Delete]
	@FilterID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE 
	FROM Filter 
	WHERE FilterID = @FilterID

END