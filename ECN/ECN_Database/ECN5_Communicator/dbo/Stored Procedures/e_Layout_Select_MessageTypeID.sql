CREATE PROCEDURE [dbo].[e_Layout_Select_MessageTypeID] 
(
@MessageTypeID int = NULL,
@CustomerID int = NULL
)
AS
BEGIN

SELECT * FROM Layout WITH(NOLOCK) WHERE CustomerID = @CustomerID AND IsDeleted = 0 and MessageTypeID=@MessageTypeID

END
