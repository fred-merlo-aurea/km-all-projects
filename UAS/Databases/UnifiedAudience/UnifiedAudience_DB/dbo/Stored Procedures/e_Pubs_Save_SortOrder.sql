CREATE PROCEDURE e_Pubs_Save_SortOrder
(
	@PubID int, 
	@SortOrder int
)
AS
BEGIN

	SET NOCOUNT ON


	UPDATE Pubs 
	SET SortOrder = @SortOrder 
	WHERE PubID = @PubID
		
END
GO