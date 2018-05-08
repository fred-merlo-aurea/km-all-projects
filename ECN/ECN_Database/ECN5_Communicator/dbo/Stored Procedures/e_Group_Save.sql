CREATE  PROC [dbo].[e_Group_Save] 
(
	@GroupID int = NULL,
	@CustomerID int = NULL,
	@FolderID int = NULL,
	@GroupName varchar(50) = NULL,
	@GroupDescription varchar(500) = NULL,
	@OwnerTypeCode varchar(50) = NULL,
	@MasterSupression int = null,
	@PublicFolder int = null,
	@OptinHTML varchar(5000) = NULL,
	@OptinFields varchar(5000) = NULL,
	@AllowUDFHistory varchar(1) = NULL,
	@IsSeedList bit = null,
	@UserID int = null,
	@Archived bit = null
)
AS 
BEGIN
	IF @GroupID is NULL or @GroupID <= 0
	BEGIN
		INSERT INTO [Groups]
		(
			CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,MasterSupression,PublicFolder,OptinHTML,OptinFields,AllowUDFHistory,IsSeedList, CreatedUserID,CreatedDate,Archived
		)
		VALUES
		(
			@CustomerID,@FolderID,@GroupName,@GroupDescription,@OwnerTypeCode,@MasterSupression,@PublicFolder,@OptinHTML,@OptinFields,@AllowUDFHistory,@IsSeedList, @UserID,GetDate(),@Archived
		)
		SET @GroupID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [Groups]
			SET CustomerID=@CustomerID,FolderID=@FolderID,GroupName=@GroupName,GroupDescription=@GroupDescription,OwnerTypeCode=@OwnerTypeCode,MasterSupression=@MasterSupression,
				PublicFolder=@PublicFolder,OptinHTML=@OptinHTML,OptinFields=@OptinFields,AllowUDFHistory=@AllowUDFHistory,IsSeedList=@IsSeedList,UpdatedUserID=@UserID,UpdatedDate=GETDATE(),Archived = @Archived
		WHERE
			GroupID = @GroupID
	END

	SELECT @GroupID
END