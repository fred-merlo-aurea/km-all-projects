/****** Object:  StoredProcedure dbo.e_ChampionAudit_Select    Script Date: 04/25/2014 10:52:23 ******/
Create  PROC [dbo].[e_ChampionAudit_Select]
(
	@ChampionAuditID int
)
AS 
SET NOCOUNT ON
BEGIN
select 
	ChampionAuditID,
	AuditTime,
	SampleID,
	BlastIDA,
	BlastIDB,
	BlastIDChampion,
	ClicksA,
	ClicksB,
	OpensA,
	OpensB,
	BouncesA,
	BouncesB,
	BlastIDWinning,
	SendToNonWinner,
	Reason
from 
	ECN5_Communicator.dbo.ChampionAudit 
where 
	ChampionAuditID = @ChampionAuditID
END