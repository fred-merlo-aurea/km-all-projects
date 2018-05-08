CREATE PROCEDURE [dbo].[ccp_BriefMedia_BMWU_Update_TopicCode]
@Xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		EmailAddress varchar(150),
		TopicID varchar(8000),
		SearchTerms varchar(8000)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(		 
		 EmailAddress,TopicID,SearchTerms	 
	)  
	
	SELECT 
		EmailAddress,TopicID,SearchTerms
	FROM OPENXML(@docHandle, N'/XML/Topic')  
	WITH   
	(
		EmailAddress varchar(150) 'EmailAddress',
		TopicID varchar(8000) 'TopicID',
		SearchTerms varchar(8000) 'SearchTerms'
	)  
	EXEC sp_xml_removedocument @docHandle    

	SELECT Distinct EmailAddress as Email, STUFF((SELECT ',' + SUB.TopicID AS [text()]
                FROM @import SUB
                WHERE
                SUB.EmailAddress = CAT.EmailAddress
                FOR XML PATH('')
                ), 1, 1, '' ) As TopicCodes,
                STUFF((SELECT ',' + SUB.SearchTerms AS [text()]
                FROM @import SUB
                WHERE
                SUB.EmailAddress = CAT.EmailAddress
                FOR XML PATH('')
                ), 1, 1, '' ) As SearchTerm
    INTO #tmpTable
    FROM @import CAT
    
    

	UPDATE tempBriefMediaBMWU
		SET TopicCodes = CASE WHEN tempBriefMediaBMWU.TopicCodes is NULL THEN x.TopicCodes
								ELSE tempBriefMediaBMWU.TopicCodes + ',' + x.TopicCodes END,
			SearchTerm = CASE WHEN tempBriefMediaBMWU.SearchTerm is NULL THEN x.SearchTerm
					ELSE tempBriefMediaBMWU.SearchTerm + ',' + x.SearchTerm END			
	FROM #tmpTable x WHERE LTRIM(RTRIM(tempBriefMediaBMWU.Email)) = LTRIM(RTRIM(x.Email))

END
GO