CREATE PROCEDURE [dbo].[e_Layout_Exists_ByMessageTypeID] 
	@MessageTypeID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 LayoutID FROM Layout WITH (NOLOCK) WHERE CustomerID = @CustomerID AND MessageTypeID = @MessageTypeID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END