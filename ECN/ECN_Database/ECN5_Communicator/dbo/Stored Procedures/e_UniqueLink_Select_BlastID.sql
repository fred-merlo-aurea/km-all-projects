CREATE PROCEDURE [dbo].[e_UniqueLink_Select_BlastID]
	@BlastID int
AS
	Select * from UniqueLink with(nolock)
	where BlastID = @BlastID
