CREATE PROCEDURE [dbo].[e_Filter_Select_PublicationID]
@PublicationID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM Filter With(NoLock)
	WHERE ProductId = @PublicationID 
	
END