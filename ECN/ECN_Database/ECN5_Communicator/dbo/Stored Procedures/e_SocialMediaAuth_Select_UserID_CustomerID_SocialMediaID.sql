CREATE PROCEDURE [dbo].[e_SocialMediaAuth_Select_UserID_CustomerID_SocialMediaID]
@UserID varchar(500),
@CustomerID int,
@SocialMediaID int
AS
	SELECT * 
	FROM SocialMediaAuth sma with(nolock)
	WHERE sma.SocialMediaID = @SocialMediaID and sma.UserID = @UserID and sma.IsDeleted = 0 and 
	sma.CustomerID in (SELECT CustomerID From ECN5_Accounts..Customer c with(nolock) where c.IsDeleted = 0 and c.BaseChannelID in (SELECT BaseChannelID FROM ECN5_Accounts..Customer with(nolock) where CustomerID = @CustomerID))
