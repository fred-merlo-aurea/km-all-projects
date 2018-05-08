-- ================================================
--Created 4/18/2014 by Tristan McCormick

-- ================================================
CREATE PROCEDURE [dbo].[e_ChampionAudit_Insert]
    @SampleID int,
    @BlastIDA int,
    @BlastIDB int,
    @BlastIDChampion int,
    @ClicksA int,
    @ClicksB int,
    @OpensA int,
    @OpensB int,
    @BouncesA int,
    @BouncesB int,
    @BlastIDWinning int,
    @SendToNonWinner bit,
    @Reason varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	Insert into ChampionAudit (
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
	Reason)
	Values
	(
	GETDATE(),
	@SampleID,
	@BlastIDA,
	@BlastIDB,
	@BlastIDChampion,
	@ClicksA,
	@ClicksB,
	@OpensA,
	@OpensB,
	@BouncesA,
	@BouncesB,
	@BlastIDWinning,
	@SendToNonWinner,
	@Reason)
	Select @@IDENTITY
END