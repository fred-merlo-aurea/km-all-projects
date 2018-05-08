CREATE PROCEDURE [dbo].[e_Transformation_Select_AssignedMappings]
@ColumnName varchar(100)
AS
BEGIN

	set nocount on

	Select T.* from Transformation T With(NoLock)
	left outer join TransformationFieldMap TFM With(NoLock)
	on T.TransformationID = TFM.TransformationID
	left outer join FieldMapping FM With(NoLock)
	on TFM.FieldMappingID = FM.FieldMappingID
	where FM.IncomingField = @ColumnName and T.IsActive = 'true'

END