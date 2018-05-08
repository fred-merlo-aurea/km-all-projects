CREATE PROCEDURE [dbo].[e_UserDataMask_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * FROM UserDataMask With(NoLock)

END
