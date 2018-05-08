CREATE  PROC [dbo].[e_BlastFieldsValue_Select] 
(
	@BlastFieldID int,
	@CustomerID int
)
AS 
BEGIN
	select CONVERT(varchar(200),bfv.BlastFieldsValueID) as  BlastFieldsValueID,bfv.Value, 
	bfv.BlastFieldID, bfv.CreatedDate, bfv.CreatedUserID, bfv.UpdatedDate, bfv.UpdatedUserID, bfv.IsDeleted 
	from BlastFieldsValue bfv where CustomerID=@CustomerID and BlastFieldID=@BlastFieldID and IsDeleted=0
END