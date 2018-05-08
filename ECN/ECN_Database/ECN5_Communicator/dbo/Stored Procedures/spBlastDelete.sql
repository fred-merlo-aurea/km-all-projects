CREATE PROCEDURE [dbo].[spBlastDelete]
@BlastID int
AS
DELETE FROM [BLAST]
WHERE BlastID = @BlastID
