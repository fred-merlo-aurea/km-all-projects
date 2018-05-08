CREATE PROCEDURE [dbo].[ccp_Vcast_Get_Distinct_MX_Books]
AS
BEGIN

	set nocount on

	Select * from tempVcastMXBooksFinal With(NoLock)

END
GO