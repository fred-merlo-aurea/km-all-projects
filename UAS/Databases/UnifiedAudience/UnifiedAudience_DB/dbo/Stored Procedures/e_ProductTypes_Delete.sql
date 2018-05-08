CREATE PROCEDURE [dbo].[e_ProductTypes_Delete]
	@PubTypeID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE PubTypes 
	WHERE PubTypeID = @PubTypeID

END