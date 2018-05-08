CREATE PROCEDURE [dbo].[ccp_Vcast_Process_File_MX_Books]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SUBSCRBNUM varchar(500),
		SUB02ANS varchar(500),		
		SUB03ANS varchar(500)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 SUBSCRBNUM,SUB02ANS,SUB03ANS
	)  
	
	SELECT 
		SUBSCRBNUM,SUB02ANS,SUB03ANS
	FROM OPENXML(@docHandle, N'/XML/WATT')  
	WITH   
	(
		SUBSCRBNUM varchar(500) 'SUBSCRBNUM',
		SUB02ANS varchar(500) 'SUB02ANS',
		SUB03ANS varchar(500) 'SUB03ANS'
	)  
	EXEC sp_xml_removedocument @docHandle    

	Insert into tempVcastMXBooks(SubNum,SO2,SO3)
	SELECT SUBSCRBNUM,SUB02ANS,SUB03ANS FROM @import
	
		
	INSERT INTO tempVcastMXBooksFinal (SubNum,SO2,SO3)	
	SELECT Distinct SubNum,
		[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.SO2 AS [text()] FROM tempVcastMXBooks SUB
		WHERE SUB.SubNum = MAIN.SubNum For Xml Path(''), type
		).value('.', 'nvarchar(max)')
		, 1, 1, ''), ',') As SO2,
		[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.SO3 AS [text()] FROM tempVcastMXBooks SUB
		WHERE SUB.SubNum = MAIN.SubNum For Xml Path(''), type
		).value('.', 'nvarchar(max)')
		, 1, 1, ''), ',') As SO3
	FROM tempVcastMXBooks MAIN With(NoLock)
	GROUP BY SubNum

END					
GO