CREATE PROCEDURE [dbo].[ccp_WATT_Get_Mac_Data]
AS
BEGIN

	set nocount on

	Select * from tempWATTMacTableFinal With(NoLock)

END
GO