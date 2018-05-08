CREATE PROCEDURE [dbo].[e_Batch_Select]
AS
BEGIN

	set nocount on

	SELECT *
	FROM Batch With(NoLock)	

END