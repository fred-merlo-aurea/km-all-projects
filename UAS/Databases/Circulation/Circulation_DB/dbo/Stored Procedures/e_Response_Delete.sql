CREATE PROCEDURE [dbo].[e_Response_Delete]
	@ResponseID int
AS
	DELETE Response WHERE ResponseID = @ResponseID
