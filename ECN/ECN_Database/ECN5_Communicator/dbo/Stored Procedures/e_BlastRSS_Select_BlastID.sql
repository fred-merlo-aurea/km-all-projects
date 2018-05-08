CREATE PROCEDURE [dbo].[e_BlastRSS_Select_BlastID]
	@BlastID int
AS
	SELECT * 
	FROM BlastRSS br with(nolock)
	where br.BlastID = @BlastID
