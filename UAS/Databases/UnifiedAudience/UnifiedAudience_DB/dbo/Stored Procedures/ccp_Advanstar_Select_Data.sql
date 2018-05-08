CREATE PROCEDURE [dbo].[ccp_Advanstar_Select_Data]
AS
BEGIN

	set nocount on

	DECLARE @temp1 TABLE (person_id varchar(75), sourcecode varchar(75))
	DECLARE @temp2 TABLE (person_id varchar(75), sourcecode varchar(75), mastervalue varchar(75), masterdesc varchar(75))
	DECLARE @temp3 TABLE (person_id varchar(75), sourcecode varchar(75), mastervalue varchar(75), masterdesc varchar(75), igroupno varchar(75))
	
	INSERT INTO @temp1
	SELECT DISTINCT * FROM tempAdvanstarSourceCode t
	WHERE (t.Person_ID != '' AND t.Person_ID is not NULL) AND
	(t.SourceCode != '' AND t.SourceCode is not NULL)
	
	INSERT INTO @temp2
	SELECT t2.person_id, t2.sourcecode, t.PriCode, t.PriCode FROM tempAdvanstarPriCode t
	JOIN @temp1 t2 ON t.SourceCode = t2.sourcecode
	WHERE (t.SourceCode != '' AND t.SourceCode is not NULL) AND
	(t.PriCode != '' AND t.PriCode is not NULL)
	
	INSERT INTO @temp3
	SELECT t2.person_id, t2.sourcecode, t2.mastervalue, t2.masterdesc, t.IGroupNo FROM tempAdvanstarRefreshDupes t
	JOIN @temp2 t2 ON t.Sequence = SUBSTRING(t2.Person_ID, PATINDEX('%[^0]%', t2.Person_ID+'.'), LEN(t2.Person_ID))
	
	SELECT 'CBIATTEN' as PubCode, IGroupNo, MasterValue, MasterDesc
	FROM @temp3

END
GO