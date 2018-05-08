CREATE PROCEDURE [dbo].[e_BlastRSS_Exists_BlastID]
	@BlastID int
AS
BEGIN
	if exists(Select top 1 BlastRSSID from BlastRSS br with(nolock) where br.BlastID = @BlastID)
		select 1
	else
		select 0

END
