CREATE PROCEDURE ccp_GLM_CreateCMSTables
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempGLMRelational]') AND type in (N'U'))
		DROP TABLE [dbo].[tempGLMRelational]
	
		CREATE TABLE tempGLMRelational
		(
			[EMAIL] varchar(150) null, 
			[LEADSSENT] int null,
			[LIKES] int null,
			[BOARDFOLLOWS] int null,
			[EXHIBITORFOLLOWS] int null	
		)	
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempGLMRelationalFinal]') AND type in (N'U'))
		DROP TABLE [dbo].[tempGLMRelationalFinal]
	
		CREATE TABLE tempGLMRelationalFinal
		(
			[EMAIL] varchar(150) null, 
			[LEADSSENT] int null,
			[LIKES] int null,
			[BOARDFOLLOWS] int null,
			[EXHIBITORFOLLOWS] int null	
		)	

END
GO