CREATE PROCEDURE [dbo].[e_ProductGroups_Select]
AS

BEGIN

	set nocount on

	SELECT * FROM PubGroups With(NoLock)

END