CREATE PROCEDURE [dbo].[e_SocialMediaAuth_Select_SocialMediaAuthID]
	@SocialMediaAuthID int
AS
	SELECT * FROM SocialMediaAuth with(nolock)
	WHERE SocialMediaAuthId = @SocialMediaAuthID and IsDeleted = 0