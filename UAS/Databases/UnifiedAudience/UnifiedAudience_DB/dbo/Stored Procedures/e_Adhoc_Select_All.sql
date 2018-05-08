CREATE PROCEDURE [dbo].[e_Adhoc_Select_All]
AS
BEGIN

	set nocount on

	SELECT * FROM Adhoc With(NoLock)

END