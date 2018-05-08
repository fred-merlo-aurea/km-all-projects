CREATE PROCEDURE [dbo].[e_Blast_Set_HasEmailPreview]
@BlastID int,
@HasEmailPreview bit = 1
AS
UPDATE [BLAST]
SET HasEmailPreview = @HasEmailPreview
WHERE BlastID = @BlastID
