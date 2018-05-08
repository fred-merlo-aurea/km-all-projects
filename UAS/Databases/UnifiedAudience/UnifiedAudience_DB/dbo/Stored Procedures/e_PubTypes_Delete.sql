CREATE PROC [dbo].[e_PubTypes_Delete]
(
@PubTypeID INT
)
AS
BEGIN

	SET NOCOUNT ON
	
	DELETE 
	FROM PubTypes 
	WHERE PubTypeID = @PubTypeID

END