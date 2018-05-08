CREATE PROCEDURE [dbo].[e_UniqueLink_Select_BlastID_UniqueID]
	@BlastID int,
	@UniqueID varchar(50)
AS
	Select * from UniqueLink ul with(nolock)
	where ul.BlastID = @BlastID and ul.UniqueID = @UniqueID