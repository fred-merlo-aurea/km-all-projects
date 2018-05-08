CREATE PROCEDURE [dbo].[e_LinkAlias_Exist_LinkOwnerID]
	@LinkOwnerID int
AS
	if exists(select top 1 la.AliasID from LinkAlias la with(nolock) where la.LinkOwnerID = @LinkOwnerID and la.IsDeleted = 0)
		select 1
	else 
		select 0
