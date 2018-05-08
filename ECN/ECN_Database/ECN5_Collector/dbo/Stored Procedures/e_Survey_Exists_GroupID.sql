CREATE PROCEDURE [dbo].[e_Survey_Exists_GroupID]
	@GroupID int
AS
	IF exists (Select top 1 * from Survey with(nolock) where GroupID = @GroupID and IsDeleted = 0)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
