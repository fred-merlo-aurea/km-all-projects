CREATE PROCEDURE [dbo].[ccp_WATT_Get_Mic_Data]
AS
BEGIN

	set nocount on

	Select * from tempWATTMicTableFinal With(NoLock)

END
GO