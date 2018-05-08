CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Update_SortOrder]
@MasterCodeSheetXml XML
AS
BEGIN

	IF OBJECT_ID('tempdb..#TempMasterCodeSheets') IS NOT NULL 
	BEGIN 
			DROP TABLE #TempMasterCodeSheets;
	END 	
		
	CREATE TABLE #TempMasterCodeSheets (MasterID int, SortOrder int); 

	insert into #TempMasterCodeSheets
	select	t.c.value('(ID/text())[1]', 'int'),
			t.c.value('(SortOrder/text())[1]', 'int')
	from @MasterCodeSheetXml.nodes('//MasterCodeSheet') as T(C);
	
	Update 
		MasterCodeSheet
    set 
		SortOrder = tmcs.SortOrder
	from 
		MasterCodeSheet mcs
		join #TempMasterCodeSheets tmcs on mcs.MasterID = tmcs.MasterID
	
	DROP TABLE #TempMasterCodeSheets
			
END
