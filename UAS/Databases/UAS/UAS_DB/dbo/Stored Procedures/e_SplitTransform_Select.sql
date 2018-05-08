CREATE PROCEDURE [dbo].[e_SplitTransform_Select]
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformSplitTrans With(NoLock)

END
GO