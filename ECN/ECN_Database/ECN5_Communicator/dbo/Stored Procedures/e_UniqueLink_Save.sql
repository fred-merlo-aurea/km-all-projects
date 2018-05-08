CREATE PROCEDURE [dbo].[e_UniqueLink_Save]
	@BlastID int,
	@BlastLinkID int,
	@UniqueID varchar(50)
AS
	INSERT INTO UniqueLink(BlastID, BlastLinkID, UniqueID)
	VALUES(@BlastID, @BlastLinkID, @UniqueID)
	Select @@IDENTITY;