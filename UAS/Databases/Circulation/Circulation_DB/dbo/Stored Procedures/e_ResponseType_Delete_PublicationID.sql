CREATE PROCEDURE [dbo].[e_ResponseType_Delete_PublicationID]
	@PublicationID int
AS
	DELETE ResponseType WHERE PublicationID = @PublicationID
