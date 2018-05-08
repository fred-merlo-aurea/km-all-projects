CREATE PROCEDURE [dbo].[ccp_BriefMedia_Select_Data]
AS
BEGIN

	set nocount on

	SELECT DISTINCT	* 
	FROM tempBriefMediaBMWUFinal WITH (NOLOCK)
	ORDER BY DrupalID

END
GO