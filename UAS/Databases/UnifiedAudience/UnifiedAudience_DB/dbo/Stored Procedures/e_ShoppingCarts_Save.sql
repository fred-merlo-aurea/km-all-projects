CREATE PROCEDURE [dbo].[e_ShoppingCarts_Save]
@UserID uniqueidentifier,
@SearchtypeID int,
@SubscriptionID int,
@Price decimal(10,2),
@IsFreeDownload bit
AS
BEGIN

	SET NOCOUNT ON

	INSERT INTO [ShoppingCarts]
           ([UserID],
		    [SearchtypeID],
		    [SubscriptionID],
		    [Price],
		    [DateAdded],
		    [IsFreeDownload])
     VALUES (@UserID,@SearchtypeID,@SubscriptionID, @Price, GETDATE(), @IsFreeDownload);SELECT @@IDENTITY;
    
END 