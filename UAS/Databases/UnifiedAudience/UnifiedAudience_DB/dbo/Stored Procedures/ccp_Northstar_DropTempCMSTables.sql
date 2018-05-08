CREATE Procedure ccp_Northstar_DropTempCMSTables
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempNorthstarWebPersonGroup]') AND type in (N'U'))
	DROP TABLE [dbo].[tempNorthstarWebPersonGroup]

END
GO