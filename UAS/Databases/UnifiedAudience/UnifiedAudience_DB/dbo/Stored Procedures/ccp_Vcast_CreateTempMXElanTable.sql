CREATE PROCEDURE [dbo].[ccp_Vcast_CreateTempMXElanTable]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXElan]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXElan]
	
	CREATE TABLE tempVcastMXElan
	(
		SubNum varchar(500) null,
		SO2 varchar(max) null,
		SO3 varchar(max) null	
	)
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXElanFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXElanFinal]
	
	CREATE TABLE [tempVcastMXElanFinal]
	(
		SubNum varchar(500) null,
		SO2 varchar(max) null,
		SO3 varchar(max) null	
	)
	
END
GO