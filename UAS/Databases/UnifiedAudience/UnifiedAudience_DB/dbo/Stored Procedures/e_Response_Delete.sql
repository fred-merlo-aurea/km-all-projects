CREATE PROCEDURE [dbo].[e_Response_Delete]
	@ResponseID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE Response 
	WHERE ResponseID = @ResponseID

END