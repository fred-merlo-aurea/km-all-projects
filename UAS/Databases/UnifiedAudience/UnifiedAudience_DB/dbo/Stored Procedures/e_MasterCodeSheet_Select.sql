CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM MasterCodeSheet With(NoLock)

END