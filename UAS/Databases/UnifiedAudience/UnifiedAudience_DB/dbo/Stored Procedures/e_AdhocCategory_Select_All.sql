CREATE PROCEDURE [dbo].[e_AdhocCategory_Select_All]
AS
BEGIN

	set nocount on

	SELECT * FROM AdhocCategory With(NoLock)

END