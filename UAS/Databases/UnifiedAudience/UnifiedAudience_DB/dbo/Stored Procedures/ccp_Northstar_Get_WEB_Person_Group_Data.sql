CREATE Procedure [dbo].[ccp_Northstar_Get_WEB_Person_Group_Data]
AS
BEGIN

	set nocount on

	Select * from tempNorthstarWebPersonGroup With(NoLock)

END
GO