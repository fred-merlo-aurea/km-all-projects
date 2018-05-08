CREATE  PROC [dbo].[e_EmailGroup_GetColumnNames] 

AS 
BEGIN
	SELECT  distinct syscolumns.name as columnName  
	FROM sysobjects 
		JOIN syscolumns ON sysobjects.id = syscolumns.id  
		JOIN systypes ON syscolumns.xtype=systypes.xtype 
	WHERE sysobjects.xtype='U' AND sysobjects.name = 'EmailGroups'
	ORDER BY syscolumns.name
	
END
