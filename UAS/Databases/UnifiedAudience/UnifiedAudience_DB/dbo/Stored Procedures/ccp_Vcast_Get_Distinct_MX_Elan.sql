CREATE PROCEDURE [dbo].[ccp_Vcast_Get_Distinct_MX_Elan]
AS
BEGIN

	set nocount on

	Select * from tempVcastMXElanFinal With(NoLock)

END
GO