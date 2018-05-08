Create PROCEDURE [dbo].[e_ChampionAudit_Exists_ByChampionAuditID] 
	@ChampionAuditID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 ChampionAuditID FROM ecn5_communicator..ChampionAudit WHERE ChampionAuditID = @ChampionAuditID) SELECT 1 ELSE SELECT 0
END