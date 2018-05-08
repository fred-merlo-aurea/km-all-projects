CREATE  PROC [dbo].[e_Code_Exists_LinkTypeID] 
(
	@LinkTypeID int
)
AS 
BEGIN
	if exists (select top 1 AliasID from LinkAlias WITH (NOLOCK) where LinkTypeID=@LinkTypeID and IsDeleted = 0) 
		SELECT 1
	else 
		SELECT 0
END