CREATE PROCEDURE [dbo].[e_Campaign_Select]	
AS
BEGIN

	set nocount on

	Select * from Campaigns With(NoLock)

END