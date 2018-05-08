CREATE PROCEDURE [dbo].[e_LinkOwnerIndex_Select_Single]   
@LinkOwnerIndexID int = NULL
AS
	SELECT * FROM LinkOwnerIndex WITH (NOLOCK) WHERE LinkOwnerIndexID = @LinkOwnerIndexID and IsDeleted = 0