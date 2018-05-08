CREATE  PROC [dbo].[e_LinkAlias_Save] 
(
	@AliasID int = NULL,
    @ContentID int = NULL,
    @Link varchar(2048) = NULL,
    @Alias varchar(2048) = NULL,
    @LinkTypeID int = NULL,
    @LinkOwnerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @AliasID is NULL or @AliasID <= 0
	BEGIN
		INSERT INTO LinkAlias
		(
			ContentID,Link, Alias, LinkTypeID,CreatedUserID,CreatedDate,IsDeleted, LinkOwnerID
		)
		VALUES
		(
			@ContentID,@Link,@Alias,@LinkTypeID,@UserID,GetDate(),0, @LinkOwnerID
		)
		SET @AliasID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE LinkAlias
			SET ContentID=@ContentID, Link=@Link, Alias=@Alias, LinkTypeID=@LinkTypeID, UpdatedUserID=@UserID, UpdatedDate=GETDATE(),LinkOwnerID=@LinkOwnerID
		WHERE
			AliasID = @AliasID
	END

	SELECT @AliasID
END