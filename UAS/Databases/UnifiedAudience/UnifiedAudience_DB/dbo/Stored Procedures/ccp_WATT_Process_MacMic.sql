CREATE PROCEDURE [dbo].[ccp_WATT_Process_MacMic]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		Pubcode varchar(500),
		FoxColumnName varchar(500),		
		CodeSheetValue varchar(8000)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 Pubcode,FoxColumnName,CodeSheetValue
	)  
	
	SELECT 
		Pubcode,FoxColumnName,CodeSheetValue
	FROM OPENXML(@docHandle, N'/XML/WATT')  
	WITH   
	(
		Pubcode varchar(500) 'Pubcode',
		FoxColumnName varchar(500) 'FoxColumnName',
		CodeSheetValue varchar(8000) 'CodeSheetValue'
	)  
	EXEC sp_xml_removedocument @docHandle    

	Insert into tempWATTMicTable
	SELECT * FROM @import WHERE FoxColumnName = 'Micro'
	
	Insert into tempWATTMacTable
	SELECT * FROM @import WHERE FoxColumnName = 'Macro'

	INSERT INTO tempWattMicTableFinal (Pubcode,FoxColumnName,CodeSheetValue)	
		SELECT * FROM 
		(
			SELECT Pubcode,'MICRO' as FoxColumnName,
				[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.CodeSheetValue AS [text()] FROM tempWattMicTable SUB
				WHERE SUB.Pubcode = MAIN.Pubcode For Xml Path(''), type
				).value('.', 'nvarchar(max)')
				, 1, 1, ''), ',') As CodeSheetValue
			FROM tempWattMicTable MAIN With(NoLock)
			GROUP BY Pubcode,FoxColumnName
		) as A
		WHERE A.CodeSheetValue != '' AND A.CodeSheetValue is not NULL

		
	INSERT INTO tempWattMacTableFinal (Pubcode,FoxColumnName,CodeSheetValue)
		SELECT * FROM 
		(	
			SELECT Pubcode,'MACRO' as FoxColumnName,
				[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.CodeSheetValue AS [text()] FROM tempWattMacTable SUB
				WHERE SUB.Pubcode = MAIN.Pubcode For Xml Path(''), type
				).value('.', 'nvarchar(max)')
				, 1, 1, ''), ',') As CodeSheetValue
			FROM tempWattMacTable MAIN With(NoLock)
			GROUP BY Pubcode,FoxColumnName
		) as B
		WHERE B.CodeSheetValue != '' AND B.CodeSheetValue is not NULL

END
GO