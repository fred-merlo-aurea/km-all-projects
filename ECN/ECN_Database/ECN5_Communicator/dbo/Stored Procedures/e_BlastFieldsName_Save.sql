CREATE  PROC [dbo].[e_BlastFieldsName_Save] 
(
	@BlastFieldID int,
	@CustomerID int,
	@Name varchar(200),
    @UserID int
)
AS 
BEGIN
	declare @BlastFieldsNameID int
	set @BlastFieldsNameID =0
	
	select @BlastFieldsNameID= BlastFieldsNameID from BlastFieldsName
	where BlastFieldID=@BlastFieldID and CustomerID=@CustomerID and IsDeleted=0
	
	if @BlastFieldsNameID>0
	BEGIN
		update BlastFieldsName set Name=@Name, UpdatedDate=GETDATE(), UpdatedUserID=@UserID 
		where BlastFieldsNameID=@BlastFieldsNameID
	END
	ELSE
	BEGIN
		insert into BlastFieldsName(BlastFieldID, CustomerID, Name, CreatedUserID, CreatedDate, IsDeleted) 
		values(@BlastFieldID, @CustomerID, @Name, @UserID, GETDATE(), 0)
		SET @BlastFieldID = @@IDENTITY	
	END
	SELECT @BlastFieldID
END