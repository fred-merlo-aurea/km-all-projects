CREATE PROCEDURE [dbo].[e_GlobalMasterSuppressionList_Select_All]   

AS
	SELECT * FROM GlobalMasterSuppressionList WITH (NOLOCK) WHERE IsDeleted = 0