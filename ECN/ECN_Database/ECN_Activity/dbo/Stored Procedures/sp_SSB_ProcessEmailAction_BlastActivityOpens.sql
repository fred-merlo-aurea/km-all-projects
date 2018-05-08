CREATE PROCEDURE [dbo].[sp_SSB_ProcessEmailAction_BlastActivityOpens]    
	@message_body XML
	
AS
/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/07/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create SP to parse new Email Opens data into BlastActivityOpens table and update BlastSummary table

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Nathan C. Hoialmen			ECN Phase III 			Initial Release
2012-02-13		Sunil Theenathayalu			ECN Phase III 			Added EAID
2012-05-24		Rohit Pooserla				ECN EmailClientRpt		Added EmailClientID, PlatformID

==============================================================================*/

BEGIN    

DECLARE @hdoc int
	
--Define temp table to capture new activity records
DECLARE @tempActivity table(
	EAID int,
	EmailID int,
	BlastID int,
	ActionTypeCode varchar(50),
	ActionDate datetime,
	ActionValue varchar(2048),
	ActionNotes varchar(255),
	Processed char(10))
	
	EXEC sp_xml_preparedocument @hdoc OUTPUT, @message_body
	
	--Insert XML stream into temp table
	INSERT @tempActivity(
		EAID,
		EmailID,
		BlastID,
		ActionTypeCode,
		ActionDate,
		ActionValue,
		ActionNotes,
		Processed)
	SELECT EAID,
		EmailID,
		BlastID,
		ActionTypeCode,
		ActionDate,
		ActionValue,
		ActionNotes,
		Processed
	FROM OPENXML(@hdoc, N'/ROOT/ACTION')    
	WITH (EAID int,
		EmailID int,
		BlastID int,
		ActionTypeCode varchar(50),
		ActionDate datetime,
		ActionValue varchar(2048),
		ActionNotes varchar(255),
		Processed char(10),
		BlastLinkID int) a 
	WHERE a.ActionTypeCode IN ('open')

		
	--Insert New Open Records
	INSERT dbo.BlastActivityOpens(
		BlastID,
		EmailID,
		OpenTime,
		BrowserInfo,
		EAID,
		EmailClientID,
		PlatformID)
	SELECT a.BlastID,
		a.EmailID,
		a.ActionDate,
		a.ActionValue,
		a.EAID,
		[ecn_Activity].dbo.[fn_GetEmailClientID] (a.ActionValue),
		[ecn_Activity].dbo.[fn_GetPlatformID] (a.ActionValue)		
	FROM @tempActivity a 
		
	/** SUMMARY UPDATE REMOVED **/
	----UPDATE Blast Summary Counts
	--MERGE dbo.BlastSummary AS target
 --   USING (SELECT BlastID,
	--			COUNT(DISTINCT EmailID) DistinctTotal,
	--			COUNT(1) Total
	--		FROM dbo.BlastActivityOpens c
	--		WHERE BlastID IN (SELECT DISTINCT BlastID FROM @tempActivity)
	--		GROUP BY BlastID) AS source (BlastID, DistinctTotal, Total)
 --   ON (target.BlastID = source.BlastID AND target.ActionTypeCode = 'open')
 --   WHEN MATCHED THEN 
 --       UPDATE SET DistinctTotal = source.DistinctTotal,
	--		Total = source.Total
	--WHEN NOT MATCHED THEN	
	--    INSERT (BlastID, DistinctTotal, Total, ActionTypeCode)
	--    VALUES (source.BlastID, source.DistinctTotal, source.Total, 'open');

	EXEC sp_xml_removedocument @hdoc	
	
END