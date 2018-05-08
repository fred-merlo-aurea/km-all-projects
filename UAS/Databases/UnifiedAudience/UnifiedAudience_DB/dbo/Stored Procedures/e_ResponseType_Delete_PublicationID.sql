CREATE PROCEDURE [dbo].[e_ResponseType_Delete_PublicationID]
	@PublicationID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE ResponseType 
	WHERE PublicationID = @PublicationID

END