CREATE PROCEDURE [dbo].[ccp_BriefMedia_BMWU_Update_PageBehavior]
@Xml xml
AS
BEGIN
	
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		AccessID varchar(8000),
		PageID varchar(8000)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 AccessID,PageID		 
	)  
	
	SELECT 
		AccessID,PageID
	FROM OPENXML(@docHandle, N'/XML/Page')  
	WITH   
	(
		AccessID varchar(8000) 'AccessID',
		PageID varchar(8000) 'PageID'
	)  
	EXEC sp_xml_removedocument @docHandle    

	SELECT Distinct AccessID, STUFF((SELECT ',' + SUB.PageID AS [text()]
                FROM @import SUB
                WHERE
                SUB.AccessID = CAT.AccessID
                FOR XML PATH('')
                ), 1, 1, '' ) As PageID
    INTO #tmptable
    FROM @import CAT
    

	UPDATE tempBriefMediaBMWU
		SET PageID = x.PageID
	FROM #tmptable x WHERE tempBriefMediaBMWU.AccessID = x.AccessID

END
GO