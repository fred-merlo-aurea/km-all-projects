CREATE PROCEDURE [dbo].[e_ReportGroups_Exists_ByIDNameResponseGroupID]
@ReportGroupID int, 
@ResponseGroupID int,
@Name varchar(255)
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS (
		SELECT TOP 1 ReportGroupID
		FROM ReportGroups WITH (NOLOCK)
		WHERE DisplayName = @Name and ReportGroupID != @ReportGroupID and ResponseGroupID = @ResponseGroupID
	) SELECT 1 ELSE SELECT 0

END
