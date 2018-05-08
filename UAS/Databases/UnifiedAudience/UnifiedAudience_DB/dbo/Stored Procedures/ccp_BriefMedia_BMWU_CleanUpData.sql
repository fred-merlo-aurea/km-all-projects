
CREATE PROCEDURE [dbo].[ccp_BriefMedia_BMWU_CleanUpData]
AS
BEGIN

	set nocount on

	--Drop Records where Email is invalid (Email was checked in code and if not valid INVALID EMAIL was passed
	IF EXISTS (SELECT * FROM tempBriefMediaBMWU WHERE Email = 'INVALID EMAIL' OR Email = '')
		BEGIN	
			DELETE tempBriefMediaBMWU 
			WHERE Email = 'INVALID EMAIL' OR Email = ''
		END

	--Roll Multiple Drupal_ID's into distinct(Stuff TopicCode, SearchTerm, PageID, AccessID
	INSERT INTO tempBriefMediaBMWUFinal
	SELECT Distinct DrupalID, 
		[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.AccessID AS [text()] FROM tempBriefMediaBMWU SUB
			WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
			).value('.', 'nvarchar(max)')
			, 1, 1, ''), ',') As AccessID,    
		MAIN.FirstName,
		MAIN.LastName,
		MAIN.Email,    
		[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.TopicCodes AS [text()] FROM tempBriefMediaBMWU SUB
			WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
			).value('.', 'nvarchar(max)')
			, 1, 1, ''), ',') As TopicCodes,    
		[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.PageID AS [text()] FROM tempBriefMediaBMWU SUB
			WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
			).value('.', 'nvarchar(max)')
			, 1, 1, ''), ',') As PageID,    
		[UAS].[DBO].[Remove_Dup_Entry](STUFF((SELECT ',' + SUB.SearchTerm AS [text()] FROM tempBriefMediaBMWU SUB
			WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
			).value('.', 'nvarchar(max)')
			, 1, 1, ''), ',') As SearchTerm
	FROM tempBriefMediaBMWU MAIN WITH(NOLOCK)
	--SELECT Distinct DrupalID, 
	--	STUFF((SELECT ',' + SUB.AccessID AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
	--        ).value('.', 'nvarchar(max)')
	--        , 1, 2, '') As AccessID,    
	--    MAIN.FirstName,
	--    MAIN.LastName,
	--    MAIN.Email,    
	--	STUFF((SELECT ',' + SUB.TopicCodes AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
	--        ).value('.', 'nvarchar(max)')
	--        , 1, 2, '') As TopicCodes,    
	--    STUFF((SELECT ',' + SUB.PageID AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
	--        ).value('.', 'nvarchar(max)')
	--        , 1, 2, '') As PageID,    
	--    STUFF((SELECT ',' + SUB.SearchTerm AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID For Xml Path(''), type
	--        ).value('.', 'nvarchar(max)')
	--        , 1, 2, '') As SearchTerm
	--FROM tempBriefMediaBMWU MAIN WITH(NOLOCK)

	--SELECT Distinct DrupalID, 
	--	[UAS].[DBO].[RemoveDups](STUFF((SELECT ',' + SUB.AccessID AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID FOR XML PATH('')), 1, 1, '' ), ',') As AccessID,    
	--    MAIN.FirstName,
	--    MAIN.LastName,
	--    MAIN.Email,    
	--	[UAS].[DBO].[RemoveDups](STUFF((SELECT ',' + SUB.TopicCodes AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID FOR XML PATH('')), 1, 1, '' ), ',') As TopicCodes,    
	--    [UAS].[DBO].[RemoveDups](STUFF((SELECT ',' + SUB.PageID AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID FOR XML PATH('')), 1, 1, '' ), ',') As PageID,    
	--    [UAS].[DBO].[RemoveDups](STUFF((SELECT ',' + SUB.SearchTerm AS [text()] FROM tempBriefMediaBMWU SUB
	--		WHERE SUB.DrupalID = MAIN.DrupalID FOR XML PATH('')), 1, 1, '' ), ',') As SearchTerm
	--FROM tempBriefMediaBMWU MAIN WITH(NOLOCK)

END
GO