CREATE PROCEDURE [dbo].[e_ResponseGroup_Exists_ByIDResponseGroupNamePubID]
@ResponseGroupID int, 
@ResponseGroupName varchar(100),
@PubID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 ResponseGroupID
		FROM ResponseGroups WITH (NOLOCK)
		WHERE ResponseGroupName = @ResponseGroupName and ResponseGroupID != @ResponseGroupID and PubID = @PubID
	) SELECT 1 ELSE SELECT 0

END
