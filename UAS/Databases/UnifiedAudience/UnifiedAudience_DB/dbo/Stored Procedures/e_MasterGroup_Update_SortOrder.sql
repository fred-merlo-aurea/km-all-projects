CREATE PROCEDURE [dbo].[e_MasterGroup_Update_SortOrder]
@MasterGroupXml XML
AS
BEGIN

	IF OBJECT_ID('tempdb..#TempMasterGroups') IS NOT NULL 
	BEGIN 
			DROP TABLE #TempMasterGroups;
	END 	
		
	CREATE TABLE #TempMasterGroups (MasterGroupID int, SortOrder int); 

	insert into #TempMasterGroups
	select	t.c.value('(ID/text())[1]', 'int'),
			t.c.value('(SortOrder/text())[1]', 'int')
	from @MasterGroupXml.nodes('//MasterGroup') as T(C);
	
	Update 
		MasterGroups
    set 
		SortOrder = tmg.SortOrder
	from 
		MasterGroups mg
		join #TempMasterGroups tmg on mg.MasterGroupID = tmg.MasterGroupID
	
	DROP TABLE #TempMasterGroups
			
END
