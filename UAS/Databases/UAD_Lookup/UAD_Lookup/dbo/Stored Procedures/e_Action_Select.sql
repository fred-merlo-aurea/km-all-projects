CREATE PROCEDURE [e_Action_Select]
AS
BEGIN

	set nocount on

	SELECT * FROM Action With(NoLock)

END