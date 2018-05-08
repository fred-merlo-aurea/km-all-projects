CREATE PROCEDURE e_AdHocDimension_Delete_SourceFileID
@SourceFileID int
AS
BEGIN

	set nocount on

	DELETE AdHocDimension 
	WHERE AdHocDimensionGroupId IN (Select AdHocDimensionGroupId From AdHocDimensionGroup Where SourceFileID = @SourceFileID)

END
GO