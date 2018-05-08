
CREATE PROCEDURE [e_CategoryCode_Select]
AS
BEGIN

	set nocount on

	SELECT * FROM CategoryCode With(NoLock)

END