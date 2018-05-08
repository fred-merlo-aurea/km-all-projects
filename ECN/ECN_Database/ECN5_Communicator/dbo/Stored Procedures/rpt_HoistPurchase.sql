--sp_helptext rpt_HoistPurchase

CREATE PROCEDURE [dbo].[rpt_HoistPurchase] 
(  
	@vendorname varchar(100),  
	@Month int,  
	@year int  
)  
as  
Begin  
   
	  
	select  convert(int, user_Store_Location_ID) as storeno,   
			user_Store_Location_Name as storename,   
			user_Sales_Rep_Name as salesperson,  
			count(case when month(user_Sale_date) = @Month then historyID end) as lineitemsMTD,  
			count(historyID) as lineitemsYTD,  
			isnull(sum(case when month(user_Sale_date) = @Month then user_item_extend_Amt else 0 end),0) as DollarMTD,  
			isnull(sum(user_item_extend_Amt),0) as DollarYTD,  
			count(distinct case when month(user_Sale_date) = @Month then user_Invoice_ID else 0 end) as TotalInvoiceMTD,   
			Isnull(count(distinct user_Invoice_ID),0) as TotalInvoiceYTD,   
			sum(case when month(user_Sale_date) = @Month then user_Item_Total_Cost else 0 end) as CostMTD,   
			isnull(sum(user_Item_Total_Cost),0) as CostYTD
			   
   	from customersaleshistory  
   	where   
    	(user_Item_Description like '%' + @vendorname + '%' OR user_Vendor_Name like '%' + @vendorname + '%') and    
    	Year(user_Sale_date) = @year   
   	Group by user_Store_Location_ID, user_Store_Location_Name, user_Sales_Rep_Name  
 	order by StoreNo, Salesperson  
end
