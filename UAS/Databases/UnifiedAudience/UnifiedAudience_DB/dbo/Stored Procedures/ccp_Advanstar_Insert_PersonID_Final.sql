CREATE PROCEDURE [dbo].[ccp_Advanstar_Insert_PersonID_Final]
AS
BEGIN

	set nocount on

	INSERT INTO tempAdvanstarPersonIDFinal
	SELECT DISTINCT  
       p.person_id,
       IndyCode = ISNULL(Stuff((SELECT DISTINCT  ', ' + SUBSTRING(SourceCode,1,1)  AS [text()]
                                                FROM tempAdvanstarPersonID C1
                                                WHERE c1.person_id = p.person_id
                                                AND  SUBSTRING(SourceCode,1,1) != 'Z'
                                                FOR XML PATH ('')),1,1,''),''),
       CatCode = ISNULL(Stuff((SELECT DISTINCT  ', ' + SUBSTRING(SourceCode,2,1)  AS [text()]
                                                FROM tempAdvanstarPersonID C2
                                                WHERE c2.person_id = p.person_id
                                                AND  SUBSTRING(SourceCode,2,1) != 'Z'
                                                FOR XML PATH ('')),1,1,''),'')
	FROM tempAdvanstarPersonID p
	ORDER BY 1

END
GO