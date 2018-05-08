CREATE PROCEDURE [dbo].[e_Transformation_Delete_TransformationID]
	@TransformationID int
AS
BEGIN

	set nocount on

	declare @TransformationType int
	declare @TransformationTypeTable varchar(50)
	declare @SqlDeleteStmt varchar(max)

	set @TransformationType = (select TransformationTypeID from Transformation Where TransformationID = @TransformationID)

	set @TransformationTypeTable = case when @TransformationType = 69 then 'TransformDataMap'
										when @TransformationType = 70 then 'TransformJoin'
										when @TransformationType = 71 then 'TransformSplit'
										when @TransformationType = 72 then 'TransformAssign'
										when @TransformationType = 73 then 'TransformSplitTrans' else 'Unknown Table' end

	update TransformationFieldMap set IsActive = 0 where TransformationID = @TransformationID
	
	update TransformationPubMap set IsActive = 0 where TransformationID = @TransformationID
	
	set @SqlDeleteStmt = 'Update '+@TransformationTypeTable+' set IsActive = 0 where TransformationID = '+ cast(@TransformationID as varchar(10))+''
	exec(@SqlDeleteStmt)

	update Transformation set isActive = 0 where TransformationId = @TransformationID

END
GO