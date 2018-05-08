CREATE PROCEDURE [dbo].[e_BlastRSS_Exists_BlastID_FeedID]
	@BlastID int,
	@FeedID int
AS
	if exists(Select top 1 BlastRSSID from BlastRSS br with(nolock) where br.BlastID = @BlastID and br.FeedID = @FeedID)
		SELECT 1
	ELSE
		SELECT 0