CREATE PROCEDURE [dbo].[ccp_BriefMedia_BMWU_Update_TaxBehavior]
@Xml xml
AS
BEGIN

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		AccessID varchar(8000),
		TopicCode varchar(8000)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 AccessID,TopicCode		 
	)  
	
	SELECT 
		AccessID,TopicCode
	FROM OPENXML(@docHandle, N'/XML/Tax')  
	WITH   
	(
		AccessID varchar(8000) 'AccessID',
		TopicCode varchar(8000) 'TopicCode'
	)  
	EXEC sp_xml_removedocument @docHandle    

	SELECT Distinct AccessID, STUFF((SELECT ',' + SUB.TopicCode AS [text()]
                FROM @import SUB
                WHERE
                SUB.AccessID = CAT.AccessID
                FOR XML PATH('')
                ), 1, 1, '' ) As TopicCodes
    INTO #tmptable
    FROM @import CAT
    

	UPDATE tempBriefMediaBMWU
		SET TopicCodes = x.TopicCodes
	FROM #tmptable x WHERE tempBriefMediaBMWU.AccessID = x.AccessID

END
GO