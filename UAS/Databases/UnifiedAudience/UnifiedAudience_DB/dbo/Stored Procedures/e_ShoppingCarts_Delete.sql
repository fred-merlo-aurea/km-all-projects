CREATE PROCEDURE [dbo].[e_ShoppingCarts_Delete]
	@ShoppingCartID int
AS
BEGIN

	SET NOCOUNT ON

	DELETE 
	FROM [ShoppingCarts] 
	WHERE ShoppingCartID = @ShoppingCartID

END