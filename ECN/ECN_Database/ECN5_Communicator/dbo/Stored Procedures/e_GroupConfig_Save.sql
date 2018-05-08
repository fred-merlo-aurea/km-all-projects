CREATE PROCEDURE [dbo].[e_GroupConfig_Save]
@GroupConfigID  int = null,
@CustomerID  int,
@ShortName varchar(200),
@IsPublic char(1),
@UserID int
AS

IF @GroupConfigID is NULL or @GroupConfigID <= 0
	BEGIN
		INSERT INTO GroupConfig
		(
			CustomerID, ShortName,  IsPublic, CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@CustomerID,@ShortName, @IsPublic, @UserID, GETDATE(), 0
		)
		SET @GroupConfigID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE GroupConfig
			SET 
			ShortName=@ShortName,
			IsPublic=@IsPublic,
			UpdatedUserID=@UserID,
			UpdatedDate=GETDATE()
			WHERE
			GroupConfigID = @GroupConfigID
			and 
			IsDeleted=0
			and 
			CustomerID=@CustomerID
	END

	SELECT @GroupConfigID