CREATE  PROC [dbo].[e_BlastFieldsValue_Save] 
(
	@BlastFieldID int,
	@CustomerID int,
	@Value varchar(200),
    @UserID int
)
AS 
BEGIN
	insert into BlastFieldsValue(BlastFieldID, CustomerID, Value, CreatedUserID, CreatedDate, IsDeleted)
	values(@BlastFieldID, @CustomerID, @Value, @UserID, GETDATE(), 0)
	
	select @@IDENTITY
END