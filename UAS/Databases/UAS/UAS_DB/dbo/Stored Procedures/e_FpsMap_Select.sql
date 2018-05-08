CREATE PROCEDURE [dbo].[e_FpsMap_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FpsMap With(NoLock)

END