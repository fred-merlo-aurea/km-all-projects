CREATE PROCEDURE [dbo].[ccp_Vcast_CreateTempMXBooksTable]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXBooks]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXBooks]
	
	CREATE TABLE tempVcastMXBooks
	(
		SubNum varchar(500) null,
		SO2 varchar(max) null,
		SO3 varchar(max) null	
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXBooksFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXBooksFinal]
	
	CREATE TABLE [tempVcastMXBooksFinal]
	(
		SubNum varchar(500) null,
		SO2 varchar(max) null,
		SO3 varchar(max) null	
	)
	
END
GO