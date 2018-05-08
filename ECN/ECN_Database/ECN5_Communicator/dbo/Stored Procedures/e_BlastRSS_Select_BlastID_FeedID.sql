CREATE PROCEDURE [dbo].[e_BlastRSS_Select_BlastID_FeedID]
	@BlastID int,
	@FeedID int
AS
	Select * from BlastRSS br with(nolock)
	WHERE br.BlastID = @BlastID and br.FeedID = @FeedID
