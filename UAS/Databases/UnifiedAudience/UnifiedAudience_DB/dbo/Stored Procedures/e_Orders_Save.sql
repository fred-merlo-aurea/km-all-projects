CREATE PROCEDURE [dbo].[e_Orders_Save]
	@UserID  uniqueidentifier,
	@OrderSubTotal decimal(10,2),
	@PromotionCode varchar(10),
	@OrderTotal decimal(10,2),
	@CardHolderName varchar(50),
	@CardHolderAddress1 varchar(100),
	@CardHolderAddress2 varchar(100),
	@CardHolderCity varchar(50),
	@CardHolderState varchar(50),
	@CardHolderZip varchar(10),
	@CardHolderCountryID int,
	@CardHolderPhone varchar(25),
	@CardNo varchar(4),
	@CardExpirationMonth int,
	@CardExpirationYear int,
	@CardType varchar(25),
	@CardCVV varchar(10),
	@IsProcessed bit,
	@PaymentTransactionID varchar(50),
	@CartType int,
	@DownloadLockDays int = null,
	@CartIDs varchar(4000)
AS
BEGIN

	set nocount on

	declare @OrderID int
	
	declare @shoppingcart Table (subscriptionID int)
	
	insert into @shoppingcart
	select  distinct s.SubscriptionID 
	from	ShoppingCarts s with(nolock) left outer join			
			OrderDetails od with(nolock) on s.SubscriptionID = od.SubscriptionID left outer join 
			Orders o with(nolock) on od.OrderID = o.OrderID 
	where  (o.OrderDate < (GETDATE() - 30) or o.OrderDate is null) and 
			s.UserID = @UserID  and (s.SearchTypeID = @CartType or @CartType=0)
			--and (ShoppingCartID in (select ITEMS from dbo.fn_Split(@CartIDs,','))) 
			
	if exists (select top 1 subscriptionID from @shoppingcart)
		Begin

			--OPEN SYMMETRIC KEY CDMKey DECRYPTION BY CERTIFICATE CDMCert
		
			INSERT INTO [Orders]
				  ([UserID]
				  ,[OrderDate]
				  ,[OrderSubTotal]
				  ,[PromotionCode]
				  ,[OrderTotal]
				  ,[CardHolderName]
				  ,[CardHolderAddress1]
				  ,[CardHolderAddress2]
				  ,[CardHolderCity]
				  ,[CardHolderState]
				  ,[CardHolderZip]
				  ,[CardHolderCountryID]
				  ,[CardHolderPhone]
				  ,[CardNo]
				  ,[CardExpirationMonth]
				  ,[CardExpirationYear]
				  ,[CardType]
				  ,[CardCVV]
				  ,[IsProcessed]
				  ,[PaymentTransactionID])
		      
			VALUES
				(@UserID, GETDATE(), @OrderSubTotal, @PromotionCode,@OrderTotal,@CardHolderName, @CardHolderAddress1, @CardHolderAddress2,
				 @CardHolderCity, @CardHolderState, @CardHolderZip, (case when @CardHolderCountryID = 0 then null else @CardHolderCountryID end), @CardHolderPhone, @CardNo, @CardExpirationMonth,
				 @CardExpirationYear, @CardType, @CardCVV, @IsProcessed,	@PaymentTransactionID)   

			if @CartType = 0
				begin
					if exists(select * from ShoppingCarts with(nolock) where UserID = @UserID)
						begin
							set @OrderID = @@IDENTITY

							insert into  OrderDetails (OrderID, UserID, SearchTypeID, SubscriptionID, Price, IsFreeDownload) 
							select @OrderID, UserID, SearchTypeID, s.SubscriptionID, case when IsFreeDownload = 1 then 0 else ISNULL(Price,0) end , IsFreeDownload  
							from ShoppingCarts s with(nolock) join @shoppingcart ts on s.SubscriptionID = ts.subscriptionID
							where UserID = @UserID
				
							--select  @OrderID, od.userID, o.OrderDate, s.UserID, s.SearchTypeID, s.SubscriptionID, ISNULL(s.Price,0), s.IsFreeDownload  
							--from ShoppingCarts s left outer join			
							--OrderDetails od on s.SubscriptionID = od.SubscriptionID left outer join 
							--Orders o on od.OrderID = o.OrderID 
							--where  (o.OrderDate < (GETDATE() - 30) or o.OrderDate is null) and 
							--s.UserID = @UserID 
				
							--select @OrderID, UserID, SearchTypeID, SubscriptionID, ISNULL(Price,0), IsFreeDownload  
							--from ShoppingCarts  
							--where UserID = @UserID 
				
							delete from ShoppingCarts where UserID = @UserID and SubscriptionID in (select SubscriptionID from @shoppingcart)
				
							SELECT @OrderID
						end
					else
						Begin
							RAISERROR('There is no items added to the cart',16,1)                   
							RETURN 
						End			
				end	
			else 
				begin
					if exists(select * from ShoppingCarts with(nolock) where UserID = @UserID and SearchTypeID = @CartType)
						begin
							set @OrderID = @@IDENTITY
				 
							insert into  OrderDetails (OrderID, UserID, SearchTypeID, SubscriptionID, Price, IsFreeDownload) 
							select @OrderID, UserID, SearchTypeID, s.SubscriptionID, case when IsFreeDownload = 1 then 0 else ISNULL(Price,0) end , IsFreeDownload  
							from ShoppingCarts s with(nolock) join @shoppingcart ts on s.SubscriptionID = ts.subscriptionID
							where UserID = @UserID and SearchTypeID = @CartType 
				
							delete from ShoppingCarts where UserID = @UserID and SubscriptionID in (select SubscriptionID from @shoppingcart)
				 
							--insert into  OrderDetails (OrderID, UserID, SearchTypeID, SubscriptionID, Price, IsFreeDownload) 
							--select @OrderID, UserID, SearchTypeID, SubscriptionID, ISNULL(Price,0), IsFreeDownload  
							--from ShoppingCarts  
							--where UserID = @UserID and SearchTypeID = @CartType
				
							--delete from ShoppingCarts where UserID = @UserID and SearchTypeID = @CartType
				
							SELECT @OrderID
						end	
					else
						Begin
							RAISERROR('There is no items added to the cart',16,1)                   
							RETURN 
						End				
				end
		end

END