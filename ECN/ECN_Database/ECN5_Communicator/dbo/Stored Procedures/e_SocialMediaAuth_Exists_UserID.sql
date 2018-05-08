CREATE PROCEDURE [dbo].[e_SocialMediaAuth_Exists_UserID]
	@UserID varchar(100),
	@CustomerID int,
	@SocialMediaID int
AS
	IF exists(Select top 1 * FROM SocialMediaAuth sma with(nolock) where sma.UserID = @UserID and sma.SocialMediaID = @SocialMediaID and sma.IsDeleted = 0 and sma.CustomerID = @CustomerID)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END

