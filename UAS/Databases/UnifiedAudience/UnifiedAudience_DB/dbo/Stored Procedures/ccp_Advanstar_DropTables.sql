CREATE PROCEDURE [dbo].[ccp_Advanstar_DropTables]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarPersonID]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarPersonID]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRegCode]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRegCode]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRegCodeCompare]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRegCodeCompare]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarPersonIDFinal]') AND type in (N'U'))
		BEGIN
		DROP TABLE [dbo].[tempAdvanstarPersonIDFinal]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRegCodeFinal]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRegCodeFinal]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarInterest]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarInterest]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarSourceCode]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarSourceCode]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarPriCode]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarPriCode]
		END
		
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRefreshDupes]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRefreshDupes]
		END	

END
GO