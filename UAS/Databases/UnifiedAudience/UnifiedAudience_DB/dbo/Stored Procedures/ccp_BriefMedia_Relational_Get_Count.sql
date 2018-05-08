CREATE PROCEDURE [dbo].[ccp_BriefMedia_Relational_Get_Count]
AS
BEGIN

	set nocount on

	SELECT DISTINCT	COUNT(*)
	FROM tempBriefMediaBMWUFinal WITH (NOLOCK)

END
GO