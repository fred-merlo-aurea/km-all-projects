﻿CREATE PROCEDURE [dbo].[sp_SSB_ProcessEmailAction_BlastActivityConversion]    
	@message_body XML
	
AS
/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/07/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create SP to parse new Email Conversion data into BlastActivityConversion table and update BlastSummary table

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Nathan C. Hoialmen			ECN Phase III 			Initial Release
2012-02-13		Sunil Theenathayalu			ECN Phase III 			Added EAID


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
	WHERE a.ActionTypeCode IN ('conversion')

	EXEC sp_xml_removedocument @hdoc
		
	--Insert New Conversion Records
	INSERT dbo.BlastActivityConversion(
		BlastID,
		EmailID,
		ConversionTime,
		URL,
		EAID)
	SELECT a.BlastID,
		a.EmailID,
		a.ActionDate,
		a.ActionValue,
		a.EAID
	FROM @tempActivity a  
	
	
	/** SUMMARY UPDATE REMOVED **/
	----UPDATE Blast Summary Counts
	--MERGE dbo.BlastSummary AS target
 --   USING (SELECT BlastID,
	--			COUNT(DISTINCT EmailID) DistinctTotal,
	--			COUNT(1) Total
	--		FROM dbo.BlastActivityConversions c
	--		WHERE BlastID IN (SELECT DISTINCT BlastID FROM @tempActivity)
	--		GROUP BY BlastID) AS source (BlastID, DistinctTotal, Total)
 --   ON (target.BlastID = source.BlastID AND target.ActionTypeCode = 'conversion')
 --   WHEN MATCHED THEN 
 --       UPDATE SET DistinctTotal = source.DistinctTotal,
	--		Total = source.Total
	--WHEN NOT MATCHED THEN	
	--    INSERT (BlastID, DistinctTotal, Total, ActionTypeCode)
	--    VALUES (source.BlastID, source.DistinctTotal, source.Total, 'conversion');
	
END