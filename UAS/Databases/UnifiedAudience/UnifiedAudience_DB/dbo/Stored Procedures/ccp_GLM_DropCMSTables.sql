CREATE PROCEDURE [dbo].[ccp_GLM_DropCMSTables]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempGLMRelational]') AND type in (N'U'))
	DROP TABLE [dbo].[tempGLMRelational]
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempGLMRelationalFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempGLMRelationalFinal]

END
GO