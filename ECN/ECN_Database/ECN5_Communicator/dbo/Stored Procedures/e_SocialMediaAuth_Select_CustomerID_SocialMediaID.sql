CREATE PROCEDURE [dbo].[e_SocialMediaAuth_Select_CustomerID_SocialMediaID]
	@CustomerID int,
	@SocialMediaID int
AS
	SELECT * 
	FROM SocialMediaAuth sma with(nolock)
	where sma.CustomerID = @CustomerID and sma.SocialMediaID = @SocialMediaID and sma.IsDeleted = 0
