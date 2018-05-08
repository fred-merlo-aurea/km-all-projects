CREATE PROCEDURE [e_Product_Select]
AS
BEGIN

	set nocount on

	SELECT *
	FROM Pubs With(NoLock)

END