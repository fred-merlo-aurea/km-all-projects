CREATE PROCEDURE [dbo].[ccp_WATT_CreateCMSTables]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMicTable]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMicTable]
	
	CREATE TABLE tempWATTMicTable
	(
		[Pubcode] varchar(500) null, 
		[FoxColumnName] varchar(500) null,
		[CodeSheetValue] varchar(8000) null
	)	
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMicTableFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMicTableFinal]
	
	CREATE TABLE tempWATTMicTableFinal
	(
		[Pubcode] varchar(500) null, 
		[FoxColumnName] varchar(500) null,
		[CodeSheetValue] varchar(8000) null
	)
	
	------------------------------------------------------------------------------------------
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMacTable]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMacTable]
	
	CREATE TABLE tempWATTMacTable
	(
		[Pubcode] varchar(500) null, 
		[FoxColumnName] varchar(500) null,
		[CodeSheetValue] varchar(8000) null
	)	
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMacTableFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMacTableFinal]
	
	CREATE TABLE tempWATTMacTableFinal
	(
		[Pubcode] varchar(500) null, 
		[FoxColumnName] varchar(500) null,
		[CodeSheetValue] varchar(8000) null
	)

END
GO