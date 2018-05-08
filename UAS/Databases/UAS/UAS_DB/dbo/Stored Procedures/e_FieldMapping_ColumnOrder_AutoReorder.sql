CREATE PROCEDURE [dbo].[e_FieldMapping_ColumnOrder_AutoReorder]
	@SourceFileID int
as
BEGIN

	set nocount on

	Declare @FieldMappingID int = 0
	Declare @FieldMappingID2 int = 0
	Declare @NewColumnOrder int = 1

	if exists
	(
		Select * from (
			Select SourceFileID, Max(ColumnOrder) as 'Top', Count(*) as 'Total' from FieldMapping 
			where SourceFileID = @SourceFileID group by SourceFileID
		) as A where A.[Top] != A.Total
	)
	BEGIN
		DECLARE db_cursor CURSOR FOR  
		Select FieldMappingID
		From FieldMapping
		Where SourceFileID = @SourceFileID
			and IsNonFileColumn = 'false'
		order by ColumnOrder

		OPEN db_cursor  
		FETCH NEXT FROM db_cursor INTO @FieldMappingID 

		WHILE @@FETCH_STATUS = 0  
			BEGIN  
       		   
				Update FieldMapping
				set ColumnOrder = @NewColumnOrder
				where FieldMappingID = @FieldMappingID

				set @NewColumnOrder = @NewColumnOrder + 1		   
				FETCH NEXT FROM db_cursor INTO @FieldMappingID  
			END  

		CLOSE db_cursor  
		DEALLOCATE db_cursor 

		DECLARE db_cursor2 CURSOR FOR  
		Select FieldMappingID
		From FieldMapping
		Where SourceFileID = @SourceFileID
			and IsNonFileColumn = 'true'
		order by ColumnOrder

		OPEN db_cursor2  
		FETCH NEXT FROM db_cursor2 INTO @FieldMappingID2 

		WHILE @@FETCH_STATUS = 0  
			BEGIN  
       		   
				Update FieldMapping
				set ColumnOrder = @NewColumnOrder
				where FieldMappingID = @FieldMappingID2

				set @NewColumnOrder = @NewColumnOrder + 1		   
				FETCH NEXT FROM db_cursor2 INTO @FieldMappingID2  
			END  

		CLOSE db_cursor2 
		DEALLOCATE db_cursor2
	END

END