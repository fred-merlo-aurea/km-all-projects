CREATE PROCEDURE [dbo].[e_SocialMediaAuth_Save]
	@SocialMediaAuthID int,
	@SocialMediaID int,
	@CustomerID int,
	@AccessToken varchar(500),
	@UserID varchar(500),
	@IsDeleted bit,
	@UpdatedUserID int = null,
	@CreatedUserID int = null,
	@AccessSecret varchar(500),
	@ProfileName varchar(500)
AS
	if(@SocialMediaAuthID > 0)
	BEGIN
		UPDATE SocialMediaAuth
		SET Access_Token = @AccessToken, UserID = @UserID, IsDeleted = @IsDeleted, UpdatedUserID = @UpdatedUserID, UpdatedDate = GETDATE(), Access_Secret = @AccessSecret, ProfileName = @ProfileName
		WHERE SocialMediaAuthId = @SocialMediaAuthID
		SELECT @SocialMediaAuthID
	END
	ELSE
	BEGIN
		INSERT INTO SocialMediaAuth(SocialMediaID, CustomerID, Access_Token, UserID, IsDeleted, CreatedUserID, CreatedDate, Access_Secret, ProfileName)
		VALUES(@SocialMediaID, @CustomerID, @AccessToken, @UserID, @IsDeleted, @CreatedUserID, GETDATE(), @AccessSecret, @ProfileName)
		SELECT @@IDENTITY;
	END
