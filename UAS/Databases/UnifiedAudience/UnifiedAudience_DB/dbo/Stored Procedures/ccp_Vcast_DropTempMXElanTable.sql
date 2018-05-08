CREATE PROCEDURE [dbo].[ccp_Vcast_DropTempMXElanTable]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXElan]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXElan]	
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXElanFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXElanFinal]
	
END
GO