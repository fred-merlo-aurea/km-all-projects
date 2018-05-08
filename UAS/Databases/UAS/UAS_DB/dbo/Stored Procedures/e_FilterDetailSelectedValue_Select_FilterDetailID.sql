CREATE PROCEDURE [dbo].[e_FilterDetailSelectedValue_Select_FilterDetailID]
@FilterDetailID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FilterDetailSelectedValue With(NoLock)
	WHERE FilterDetailId = @FilterDetailID 

END