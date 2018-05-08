CREATE PROCEDURE [dbo].[e_SocialMediaAuth_Select_CustomerID]
	@CustomerID int
AS
	SELECT *
	FROM SocialMediaAuth sma with(nolock)
	WHERE sma.CustomerID = @CustomerID and sma.IsDeleted = 0
