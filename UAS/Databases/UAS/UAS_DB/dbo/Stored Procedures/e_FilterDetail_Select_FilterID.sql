CREATE PROCEDURE [dbo].[e_FilterDetail_Select_FilterID]
@FilterID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FilterDetail With(NoLock)
	WHERE FilterId = @FilterID 

END