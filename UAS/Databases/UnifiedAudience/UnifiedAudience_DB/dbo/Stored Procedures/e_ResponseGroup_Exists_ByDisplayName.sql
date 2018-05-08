CREATE PROCEDURE [dbo].[e_ResponseGroup_Exists_ByDisplayName]
@DisplayName varchar(100)
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 ResponseGroupID
		FROM ResponseGroups WITH (NOLOCK)
		WHERE DisplayName = @DisplayName
	) SELECT 1 ELSE SELECT 0

END
