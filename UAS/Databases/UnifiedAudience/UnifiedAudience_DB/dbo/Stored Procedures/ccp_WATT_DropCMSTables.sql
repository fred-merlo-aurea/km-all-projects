CREATE PROCEDURE [dbo].[ccp_WATT_DropCMSTables]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMicTable]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMicTable]
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMicTableFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMicTableFinal]
	
	------------------------------------------------------------------------------------------
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMacTable]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMacTable]
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempWATTMacTableFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempWATTMacTableFinal]

END
GO