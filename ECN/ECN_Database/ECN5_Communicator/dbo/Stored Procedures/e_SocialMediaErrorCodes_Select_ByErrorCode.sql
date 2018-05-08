CREATE PROCEDURE [dbo].[e_SocialMediaErrorCodes_Select_ByErrorCode]
	@errorCode int,
	@mediaType int
AS
	SELECT * 
	FROM [ECN5_COMMUNICATOR].[dbo].[SocialMediaErrorCodes] smec with(nolock)
	WHERE smec.mediaType = @mediaType AND smec.errorCodeRepost = @errorCode


GO