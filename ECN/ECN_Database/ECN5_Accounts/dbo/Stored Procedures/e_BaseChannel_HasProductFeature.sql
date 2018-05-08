create PROCEDURE [dbo].[e_BaseChannel_HasProductFeature]      
(  
 @BaseChannelID int,
 @Product varchar(50),
 @Feature varchar(50)
)  
AS    
  
BEGIN     
   
   If 
		exists (
		select
			top 1 *
		from
			Customer c with(nolock)
			join CustomerProduct cp WITH (NOLOCK) on c.CustomerID = cp.CustomerID
			join ProductDetail pd on cp.ProductDetailID = pd.ProductDetailID
			join Product p with(nolock) on pd.ProductID = p.ProductID
		Where  
			c.BaseChannelID = @BaseChannelID
			and p.ProductName = @Product
			and pd.ProductDetailName = @Feature
			and cp.IsDeleted = 0
			and c.IsDeleted = 0 )
	select 1
	else
		select 0
END
GO

