CREATE PROCEDURE [dbo].[e_BlastActivitySocial_FBHasBeenShared]
	@BlastID int
AS
	if exists(select top 1 * FROM BlastActivitySocial bas with(nolock) where bas.BlastID = @BlastID and bas.SocialMediaID = 1)
		select 1
	else
		select 0
