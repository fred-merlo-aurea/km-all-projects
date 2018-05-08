CREATE PROCEDURE [dbo].[ccp_BriefMedia_BMWU_Update_SearchBehavior]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		AccessID varchar(8000),
		SearchTerm varchar(8000)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 AccessID,SearchTerm		 
	)  
	
	SELECT 
		AccessID,SearchTerm
	FROM OPENXML(@docHandle, N'/XML/Search')  
	WITH   
	(
		AccessID varchar(8000) 'AccessID',
		SearchTerm varchar(8000) 'SearchTerm'
	)  
	EXEC sp_xml_removedocument @docHandle    

	SELECT Distinct AccessID, STUFF((SELECT ',' + SUB.SearchTerm AS [text()]
                FROM @import SUB
                WHERE
                SUB.AccessID = CAT.AccessID
                FOR XML PATH('')
                ), 1, 1, '' ) As SearchTerm
    INTO #tmptable
    FROM @import CAT
    

	UPDATE tempBriefMediaBMWU
		SET SearchTerm = x.SearchTerm
	FROM #tmptable x WHERE tempBriefMediaBMWU.AccessID = x.AccessID

END
GO