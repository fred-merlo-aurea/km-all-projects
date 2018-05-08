CREATE PROCEDURE [dbo].[ccp_Advanstar_CreateTempTables]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarPersonID]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarPersonID]
		END
	
	CREATE TABLE tempAdvanstarPersonID
	(
		Person_ID varchar(75) null,
		SourceCode varchar(max) null	
	)

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRegCode]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRegCode]
		END

	CREATE TABLE tempAdvanstarRegCode
	(
		Person_ID varchar(75) null,
		Ticket_Type varchar(75) null,
		Ticket_Stub varchar(75) null
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRegCodeCompare]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRegCodeCompare]
		END

	CREATE TABLE tempAdvanstarRegCodeCompare
	(
		TICKETTYPE varchar(75) null,
		TICKETSUBT varchar(75) null,
		REGCODE varchar(75) null
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarPersonIDFinal]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarPersonIDFinal]
		END
	
	CREATE TABLE tempAdvanstarPersonIDFinal
	(
		Person_ID varchar(75) null,
		IndyCode varchar(75) null,
		CatCode varchar(75) null
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRegCodeFinal]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRegCodeFinal]
		END

	CREATE TABLE tempAdvanstarRegCodeFinal
	(
		Pubcode varchar(75) null,
		Person_ID varchar(75) null,
		RegCode varchar(75)
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarInterest]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarInterest]
		END
	
	CREATE TABLE tempAdvanstarInterest
	(
		PubCode varchar(75) null,
		Person_ID varchar(75) null,
		SourceCode varchar(75) null,
		IGroupNo varchar(75) null,
		MasterValue varchar(250) null,
		MasterDesc varchar(250) null,
		PriCode varchar(250) null
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarSourceCode]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarSourceCode]
		END
	
	CREATE TABLE tempAdvanstarSourceCode
	(
		Person_ID varchar(75) null,
		SourceCode varchar(75) null
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarPriCode]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarPriCode]
		END
	
	CREATE TABLE tempAdvanstarPriCode
	(
		SourceCode varchar(75) null,
		PriCode varchar(75) null
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempAdvanstarRefreshDupes]') AND type in (N'U'))
		BEGIN
			DROP TABLE [dbo].[tempAdvanstarRefreshDupes]
		END
	
	CREATE TABLE tempAdvanstarRefreshDupes
	(
		Sequence varchar(75) null,
		IGroupNo varchar(75) null
	)

END
GO