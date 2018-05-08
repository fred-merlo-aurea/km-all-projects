CREATE  PROC [dbo].[e_BlastFieldsName_Select] 
(
	@BlastFieldID int,
	@CustomerID int
)
AS 
BEGIN
	select * from BlastFieldsName where BlastFieldID=@BlastFieldID
	and CustomerID=@CustomerID and IsDeleted=0
	
END