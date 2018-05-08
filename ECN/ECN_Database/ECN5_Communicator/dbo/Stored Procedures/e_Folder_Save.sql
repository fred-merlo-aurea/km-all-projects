CREATE  PROC [dbo].[e_Folder_Save] 
(
	@FolderID int = NULL,
	@ParentID int = NULL,
	@IsSystem bit = NULL,
	@FolderType varchar(50) = NULL,
	@FolderDescription varchar(255) = NULL,
	@FolderName varchar(50) = NULL,
	@CustomerID int = NULL,
	@UserID int = null
)
AS 
BEGIN
	IF @FolderID is NULL or @FolderID <= 0
	BEGIN
		INSERT INTO Folder
		(
			CustomerID,FolderName,FolderDescription,FolderType,IsSystem,ParentID,CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@CustomerID,@FolderName,@FolderDescription,@FolderType,@IsSystem,@ParentID,@UserID,GetDate(),0
		)
		SET @FolderID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE Folder
			SET CustomerID=@CustomerID,FolderName=@FolderName,FolderDescription=@FolderDescription,FolderType=@FolderType,IsSystem=@IsSystem,ParentID=@ParentID,UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			FolderID = @FolderID
	END

	SELECT @FolderID
END