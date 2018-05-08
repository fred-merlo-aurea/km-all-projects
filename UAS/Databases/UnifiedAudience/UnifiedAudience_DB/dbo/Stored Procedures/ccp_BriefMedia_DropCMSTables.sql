CREATE PROCEDURE [dbo].[ccp_BriefMedia_DropCMSTables]
WITH EXECUTE AS 'dbo'
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempBriefMediaBMWU]') AND type in (N'U'))
	DROP TABLE [dbo].[tempBriefMediaBMWU]

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempBriefMediaBMWU]') AND type in (N'U'))
	DROP TABLE [dbo].[tempBriefMediaBMWUFinal]

END
GO