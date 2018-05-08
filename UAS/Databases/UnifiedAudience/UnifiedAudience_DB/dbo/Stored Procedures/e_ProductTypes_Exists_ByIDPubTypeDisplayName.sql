CREATE PROCEDURE [dbo].[e_ProductTypes_Exists_ByIDPubTypeDisplayName]
@PubTypeID int, 
@PubTypeDisplayName varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 PubTypeID
		FROM PubTypes WITH (NOLOCK)
		WHERE PubTypeDisplayName = @PubTypeDisplayName and PubTypeID != @PubTypeID
	) SELECT 1 ELSE SELECT 0

END
