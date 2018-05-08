CREATE PROCEDURE [dbo].[e_FilterSegmentation_Exists_ByIDName]
@FilterSegmentationID int, 
@FilterSegmentationName varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS (
		SELECT TOP 1 FilterSegmentationID
		FROM FilterSegmentation WITH (NOLOCK)
		WHERE FilterSegmentationName = @FilterSegmentationName and FilterSegmentationID <> @FilterSegmentationID
	) SELECT 1 ELSE SELECT 0

END
