CREATE PROCEDURE [dbo].[e_ProductTypes_Select_ID]
@PubTypeID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM PubTypes With(NoLock)
	WHERE PubTypeID = @PubTypeID

END
