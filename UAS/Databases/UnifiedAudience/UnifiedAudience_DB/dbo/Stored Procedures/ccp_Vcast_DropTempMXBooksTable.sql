CREATE PROCEDURE [dbo].[ccp_Vcast_DropTempMXBooksTable]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXBooks]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXBooks]
	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempVcastMXBooksFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempVcastMXBooksFinal]
	
END
GO