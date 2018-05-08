CREATE Procedure [dbo].[ccp_Northstar_Get_WEB_Person_Group_Get_Count]
AS
BEGIN

	set nocount on

	Select count(*) from tempNorthstarWebPersonGroup With(NoLock)

END
GO