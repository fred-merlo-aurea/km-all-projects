CREATE  PROC [dbo].[e_UploadLog_Save] 
(
	@UploadID int = NULL,
    @UserID int = NULL,
    @CustomerID int = NULL,
    @Path varchar(500) = NULL,
    @FileName varchar(250) = NULL,
    @PageSource varchar(500) = NULL
)
AS 
BEGIN
	IF @UploadID is NULL or @UploadID <= 0
	BEGIN
		INSERT INTO UploadLog
		(
			UserID, CustomerID, [Path], [FileName], uploaddate, PageSource
		)
		VALUES
		(
			@UserID, @CustomerID, @Path, @FileName, GETDATE(), @PageSource
		)
		SET @UploadID = @@IDENTITY
	END
	--ELSE
	--BEGIN
	--	UPDATE UploadLog
	--		SET UserID=@UserID,CustomerID=@CustomerID,[Path]=@Path,[FileName]=@FileName,uploaddate=GETDATE(),PageSource=@PageSource
	--	WHERE
	--		UploadID = @UploadID
	--END

	SELECT @UploadID
END