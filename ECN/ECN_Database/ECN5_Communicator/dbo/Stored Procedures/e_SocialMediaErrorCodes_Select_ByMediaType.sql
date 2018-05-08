create PROCEDURE [dbo].[e_SocialMediaErrorCodes_Select_ByMediaType]	
	@mediaType int
AS
	SELECT * 
	FROM [ECN5_COMMUNICATOR].[dbo].[SocialMediaErrorCodes] smec with(nolock)
	WHERE smec.mediaType = @mediaType


GO