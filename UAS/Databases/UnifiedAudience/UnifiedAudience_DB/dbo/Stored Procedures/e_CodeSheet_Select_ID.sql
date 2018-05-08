CREATE PROCEDURE [dbo].[e_CodeSheet_Select_ID]
@CodeSheetID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM CodeSheet With(NoLock)
	WHERE CodeSheetID = @CodeSheetID

END
