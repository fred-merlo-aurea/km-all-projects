CREATE PROCEDURE [dbo].[e_ResponseGroup_Exists_ByIDDisplayNamePubID]
@ResponseGroupID int, 
@DisplayName varchar(100),
@PubID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 ResponseGroupID
		FROM ResponseGroups WITH (NOLOCK)
		WHERE DisplayName = @DisplayName and ResponseGroupID != @ResponseGroupID and PubID = @PubID
	) SELECT 1 ELSE SELECT 0

END
