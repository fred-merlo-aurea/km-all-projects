CREATE TABLE [dbo].[EmailActivityLog] (
    [EAID]           INT            IDENTITY (1, 1) NOT NULL,
    [EmailID]        INT            NOT NULL,
    [BlastID]        INT            NOT NULL,
    [ActionTypeCode] VARCHAR (50)   NOT NULL,
    [ActionDate]     DATETIME       NULL,
    [ActionValue]    VARCHAR (2048) NULL,
    [ActionNotes]    VARCHAR (355)  NULL,
    [Processed]      CHAR (10)      CONSTRAINT [DF_EmailActivityLog_Processed] DEFAULT ('n') NULL,
    CONSTRAINT [PK_EmailActivityLog] PRIMARY KEY CLUSTERED ([EAID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_EmailActivityLog_20_37575172__K3_K4_K2_K1]
    ON [dbo].[EmailActivityLog]([BlastID] ASC, [ActionTypeCode] ASC, [EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_index_EmailActivityLog_All]
    ON [dbo].[EmailActivityLog]([ActionTypeCode] ASC, [BlastID] ASC, [EmailID] ASC, [ActionDate] ASC, [EAID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_BlastID_ActionTypeCode]
    ON [dbo].[EmailActivityLog]([BlastID] ASC, [ActionTypeCode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_BlastID_EmailID]
    ON [dbo].[EmailActivityLog]([BlastID] ASC, [EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EmailActivityLog_blastID]
    ON [dbo].[EmailActivityLog]([BlastID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EmailID]
    ON [dbo].[EmailActivityLog]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [_dta_stat_37575172_1_3_2_5]
    ON [dbo].[EmailActivityLog]([EAID], [BlastID], [EmailID], [ActionDate]);


GO
CREATE STATISTICS [_dta_stat_37575172_2_4_3_6_5_1]
    ON [dbo].[EmailActivityLog]([EmailID], [ActionTypeCode], [BlastID], [ActionValue], [ActionDate], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_2_5_4]
    ON [dbo].[EmailActivityLog]([EmailID], [ActionDate], [ActionTypeCode], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_3_5]
    ON [dbo].[EmailActivityLog]([BlastID], [ActionDate], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_4_2_1]
    ON [dbo].[EmailActivityLog]([ActionTypeCode], [EmailID], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_4_3_2_1]
    ON [dbo].[EmailActivityLog]([ActionTypeCode], [BlastID], [EmailID], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_5_4_1_3]
    ON [dbo].[EmailActivityLog]([ActionDate], [ActionTypeCode], [EAID], [BlastID]);


GO
CREATE STATISTICS [_dta_stat_37575172_5_4_3_2]
    ON [dbo].[EmailActivityLog]([ActionDate], [ActionTypeCode], [BlastID], [EmailID], [EAID]);


GO
CREATE TRIGGER [dbo].[tr_SSB_EmailActivityLog_NewAction]   
ON [dbo].[EmailActivityLog]
FOR INSERT --, UPDATE

	
AS
/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/07/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create trigger on EmailActivityLog table to fire SSB_EmailActions_SendNewMessage SP when new email action record is created/modified

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Nathan C. Hoialmen			ECN Phase III 			Initial Release


==============================================================================*/

BEGIN  
  
DECLARE @xmlData as XML
/*
	IF EXISTS (SELECT 1 FROM inserted)
	Begin
	

		--IF EXISTS (SELECT 1 FROM inserted WHERE ActionTypeCode in ('click', 'open')
		--BEGIN        
		--	SET @xmlData = (SELECT inserted.*,
		--						l.BlastLinkID   
		--					FROM inserted  
		--					LEFT JOIN dbo.BlastLinks l
		--					ON inserted.BlastID = l.BlastID  
		--					AND inserted.ActionValue = l.LinkURL
		--					FOR XML RAW ('ACTION'), ROOT('ROOT'))  
						
		--END

		BEGIN        

			SET @xmlData = (SELECT inserted.*,
								NULL BlastLinkID   
							FROM inserted  
							FOR XML RAW ('ACTION'), ROOT('ROOT'))  
						
		END
	
		EXEC [dbo].[sp_SSB_EmailActions_SendNewMessage] @xmlData
		
		--IF EXISTS (SELECT 1 FROM inserted WHERE ActionTypeCode in ('click', 'open'))
		--BEGIN        
		--	update Emails 
		--	set BounceScore = 0, 
		--		DateUpdated=GETDATE()
		--	where EmailID in (select EmailID from inserted WHERE ActionTypeCode in ('click', 'open')) 
		--END	
	
	End	*/
END


GO
GRANT DELETE
    ON OBJECT::[dbo].[EmailActivityLog] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EmailActivityLog] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EmailActivityLog] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EmailActivityLog] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EmailActivityLog] TO [reader]
    AS [dbo];

