CREATE PROCEDURE [dbo].[e_ECNCampaign_Select]
AS
BEGIN

	set nocount on

	Select * from ECNCampaign With(NoLock)

END