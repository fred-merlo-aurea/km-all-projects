Create PROCEDURE [dbo].[e_ChampionAudit_Exists_BySampleID] 
	@SampleID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 ChampionAuditID FROM ecn5_communicator..ChampionAudit WHERE SampleID = @SampleID) SELECT 1 ELSE SELECT 0
END