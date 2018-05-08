CREATE  PROC [dbo].[e_Layout_Exists_MessageTypeID] 
(
	@MessageTypeID int = NULL
)
AS 
BEGIN
	if exists (select top 1 LayoutID from Layout WITH (NOLOCK) where MessageTypeID=@MessageTypeID and IsDeleted = 0) 
		return 1
	else 
		return 0
END