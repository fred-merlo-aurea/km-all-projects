CREATE  PROC [dbo].[e_Content_Save] 
(
	@ContentID int = NULL,
    @ContentTitle varchar(255) = NULL,
    @ContentTypeCode varchar(50) = NULL,
    @LockedFlag varchar(1) = NULL,
    @FolderID int = NULL,
    @ContentSource text = NULL,
    @ContentMobile text = NULL,
    @ContentText text = NULL,
    @ContentURL varchar(255) = NULL,
    @ContentFilePointer varchar(255) = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL,
    @Sharing varchar(1) = NULL,
    @MasterContentID int = null,
    @ContentSMS text = null,
    @UseWYSIWYGeditor bit =NULL,
	@Archived bit = null,
	@Validated  bit =null
)
AS 
BEGIN
	IF @ContentID is NULL or @ContentID <= 0
	BEGIN
		IF @UseWYSIWYGeditor is null
		BEGIN
			set @UseWYSIWYGeditor=1
		END
		INSERT INTO Content
		(
			ContentTitle,ContentTypeCode,LockedFlag,FolderID,ContentSource,ContentMobile,ContentText,ContentURL,ContentFilePointer,CustomerID,Sharing, UpdatedUserID,UpdatedDate, CreatedUserID,CreatedDate,IsDeleted,MasterContentID,ContentSMS,UseWYSIWYGeditor,Archived,IsValidated
		)
		VALUES
		(
			@ContentTitle,@ContentTypeCode,@LockedFlag,@FolderID,@ContentSource,@ContentMobile,@ContentText,@ContentURL,@ContentFilePointer,@CustomerID,@Sharing, @UserID,GetDate(), @UserID,GetDate(),0,@MasterContentID,@ContentSMS,@UseWYSIWYGeditor,@Archived,@Validated 
		)
		SET @ContentID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE Content
			SET ContentTitle=@ContentTitle,ContentTypeCode=@ContentTypeCode,LockedFlag=@LockedFlag,FolderID=@FolderID,ContentSource=@ContentSource,
				ContentMobile=@ContentMobile,ContentText=@ContentText,ContentURL=@ContentURL,ContentFilePointer=@ContentFilePointer,CustomerID=@CustomerID,
				Sharing=@Sharing,UpdatedUserID=@UserID,UpdatedDate=GETDATE(),ContentSMS=@ContentSMS,MasterContentID=@MasterContentID,Archived = @Archived,IsValidated=@Validated
		WHERE
			ContentID = @ContentID
	END

	SELECT @ContentID
END