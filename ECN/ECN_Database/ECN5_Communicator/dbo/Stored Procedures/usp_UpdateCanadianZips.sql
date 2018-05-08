

CREATE PROCEDURE usp_UpdateCanadianZips

AS

SET NOCOUNT ON 

-----------------------------------------------------------
/* Create List of Emails with Canadian ZIP  having a dash*/
-----------------------------------------------------------

IF EXISTS (SELECT 1 FROM ECN_TEMP.SYS.TABLES WHERE NAME = 'CanadianZIPFix' ) DROP TABLE ECN_TEMP.dbo.CanadianZIPFix

SELECT 
	EmailID, 
	Zip, 
	SUBSTRING(RTRIM(LTRIM(zip)), 1, 3) + ' ' + SUBSTRING(RTRIM(LTRIM(zip)), 5, 3) as NewZip,
	GETDATE() AS CreateDate
INTO 
	ECN_TEMP.dbo.CanadianZIPFix 
FROM
    Emails with (nolock) 
WHERE 
	UPPER(RTRIM(LTRIM(Country))) = 'CANADA' 
	AND Zip like '___-___'
   
  
--------------------------------------------------------------
/* Update Emails in batches to prevent large scale blocking */
--------------------------------------------------------------

START:

	SET ROWCOUNT 10000

	UPDATE
		E
	SET 
		Zip = czf.NewZip 
	FROM
		ECN5_COMMUNICATOR.dbo.Emails e 
		INNER JOIN ECN_TEMP.dbo.CanadianZIPFix czf on e.EmailId = czf.EmailID
		AND e.Zip != czf.NewZip
	
IF EXISTS (SELECT TOP 1 * FROM ECN5_COMMUNICATOR.dbo.Emails e WITH (NOLOCK) INNER JOIN ECN_TEMP.dbo.CanadianZIPFix czf on e.EmailId = czf.EmailID AND e.Zip != czf.NewZip) 
GOTO START

------------------------
/*Clean up work table */
------------------------
SET ROWCOUNT 0

DROP TABLE ECN_TEMP.dbo.CanadianZIPFix