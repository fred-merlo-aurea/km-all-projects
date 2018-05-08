CREATE PROCEDURE [dbo].[e_TransformationFieldMap_Delete]
@TransformationName varchar(100),
@ClientID int,
@ColumnName varchar(50)
AS
BEGIN

	set nocount on

	Declare @ID int
	set @ID = (Select TransformationFieldMapID 
	from TransformationFieldMap TFM
	inner join Transformation T
	on TFM.TransformationId = T.TransformationId
	inner join FieldMapping FM
	on TFM.FieldMappingId = FM.FieldMappingID
	where T.TransformationName = @TransformationName
	and FM.IncomingField = @ColumnName
	and T.ClientId = @ClientID);
	Delete from TransformationFieldMap 
	where TransformationFieldMapId = @ID;
	Select @ID;
	
END