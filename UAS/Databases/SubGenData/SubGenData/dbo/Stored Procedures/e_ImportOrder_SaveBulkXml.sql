create procedure e_ImportOrder_SaveBulkXml
@xml xml
as
BEGIN

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		OrderID int NOT NULL,
		OrderItemID int NULL,
		SubscriberID int NULL,
		SubscriberFirstName varchar(500) NULL,
		SubscriberLastName varchar(500) NULL,
		ShippingFirstName varchar(500) NULL,
		ShippingLastName varchar(500) NULL,
		ShippingAddressLine1 varchar(500) NULL,
		ShippingCity varchar(500) NULL,
		ShippingState varchar(500) NULL,
		ShippingPostalCode varchar(500) NULL,
		ShippingCountry varchar(500) NULL,
		BillingFirstName varchar(500) NULL,
		BillingLastName varchar(500) NULL,
		BillingAddressLine1 varchar(500) NULL,
		BillingCity varchar(500) NULL,
		BillingState varchar(500) NULL,
		BillingPostalCode varchar(500) NULL,
		BillingCountry varchar(500) NULL,
		CreatedbyRep varchar(500) NULL,
		OrderDate date NULL,
		ProductName varchar(500) NULL,
		PublicationName varchar(500) NULL,
		PublicationProductCode int NULL,
		PublicationRevenueCode int NULL,
		SubscriptionOfferName varchar(500) NULL,
		StarterIssues bit NULL,
		TotalIssues int NULL,
		Copies int NULL,
		Quantity int NULL,
		ParentOrderItemID int NULL,
		FulfilledDate date NULL,
		RefundedDate date NULL,
		SubTotal money NULL,
		TaxTotal money NULL,
		GrandTotal money NULL,
		ProductID int NULL,
		DateCreated datetime NOT NULL,
		DateUpdated datetime NULL,
		IsMergedToUAD bit NOT NULL,
		DateMergedToUAD datetime NULL
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		OrderID,OrderItemID,SubscriberID,SubscriberFirstName,SubscriberLastName,ShippingFirstName,ShippingLastName,ShippingAddressLine1,
		ShippingCity,ShippingState,ShippingPostalCode,ShippingCountry,BillingFirstName,BillingLastName,BillingAddressLine1,BillingCity,
		BillingState,BillingPostalCode,BillingCountry,CreatedbyRep,OrderDate,ProductName,PublicationName,PublicationProductCode,
		PublicationRevenueCode,SubscriptionOfferName,StarterIssues,TotalIssues,Copies,Quantity,ParentOrderItemID,FulfilledDate,
		RefundedDate,SubTotal,TaxTotal,GrandTotal,ProductID,DateCreated,DateUpdated,IsMergedToUAD,DateMergedToUAD
	)  
	
	SELECT 
			OrderID,OrderItemID,SubscriberID,SubscriberFirstName,SubscriberLastName,ShippingFirstName,ShippingLastName,ShippingAddressLine1,
			ShippingCity,ShippingState,ShippingPostalCode,ShippingCountry,BillingFirstName,BillingLastName,BillingAddressLine1,BillingCity,
			BillingState,BillingPostalCode,BillingCountry,CreatedbyRep,OrderDate,ProductName,PublicationName,PublicationProductCode,
			PublicationRevenueCode,SubscriptionOfferName,StarterIssues,TotalIssues,Copies,Quantity,ParentOrderItemID,FulfilledDate,
			RefundedDate,SubTotal,TaxTotal,GrandTotal,ProductID,DateCreated,DateUpdated,IsMergedToUAD,DateMergedToUAD
	FROM OPENXML(@docHandle, N'/XML/ImportOrder') --SubscriberOriginal  
	WITH   
	(  
		OrderID int 'OrderID',
		OrderItemID int 'OrderItemID',
		SubscriberID int 'SubscriberID',
		SubscriberFirstName varchar(500) 'SubscriberFirstName',
		SubscriberLastName varchar(500) 'SubscriberLastName',
		ShippingFirstName varchar(500) 'ShippingFirstName',
		ShippingLastName varchar(500) 'ShippingLastName',
		ShippingAddressLine1 varchar(500) 'ShippingAddressLine1',
		ShippingCity varchar(500) 'ShippingCity',
		ShippingState varchar(500) 'ShippingState',
		ShippingPostalCode varchar(500) 'ShippingPostalCode',
		ShippingCountry varchar(500) 'ShippingCountry',
		BillingFirstName varchar(500) 'BillingFirstName',
		BillingLastName varchar(500) 'BillingLastName',
		BillingAddressLine1 varchar(500) 'BillingAddressLine1',
		BillingCity varchar(500) 'BillingCity',
		BillingState varchar(500) 'BillingState',
		BillingPostalCode varchar(500) 'BillingPostalCode',
		BillingCountry varchar(500) 'BillingCountry',
		CreatedbyRep varchar(500) 'CreatedbyRep',
		OrderDate date 'OrderDate',
		ProductName varchar(500) 'ProductName',
		PublicationName varchar(500) 'PublicationName',
		PublicationProductCode int 'PublicationProductCode',
		PublicationRevenueCode int 'PublicationRevenueCode',
		SubscriptionOfferName varchar(500) 'SubscriptionOfferName',
		StarterIssues bit 'StarterIssues',
		TotalIssues int 'TotalIssues',
		Copies int 'Copies',
		Quantity int 'Quantity',
		ParentOrderItemID int 'ParentOrderItemID',
		FulfilledDate date 'FulfilledDate',
		RefundedDate date 'RefundedDate',
		SubTotal money 'SubTotal',
		TaxTotal money 'TaxTotal',
		GrandTotal money 'GrandTotal',
		ProductID int 'ProductID',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		IsMergedToUAD bit 'IsMergedToUAD',
		DateMergedToUAD datetime 'DateMergedToUAD'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle  
	
	--Insert or Update to ImportOrder - check if OrderID exists
	update o
	set o.OrderItemID = i.OrderItemID,
		o.SubscriberID = i.SubscriberID,
		o.SubscriberFirstName = i.SubscriberFirstName,
		o.SubscriberLastName = i.SubscriberLastName,
		o.ShippingFirstName = i.ShippingFirstName,
		o.ShippingLastName = i.ShippingLastName,
		o.ShippingAddressLine1 = i.ShippingAddressLine1,
		o.ShippingCity = i.ShippingCity,
		o.ShippingState = i.ShippingState,
		o.ShippingPostalCode = i.ShippingPostalCode,
		o.ShippingCountry = i.ShippingCountry,
		o.BillingFirstName = i.BillingFirstName,
		o.BillingLastName = i.BillingLastName,
		o.BillingAddressLine1 = i.BillingAddressLine1,
		o.BillingCity = i.BillingCity,
		o.BillingState = i.BillingState,
		o.BillingPostalCode = i.BillingPostalCode,
		o.BillingCountry = i.BillingCountry,
		o.CreatedbyRep = i.CreatedbyRep,
		o.OrderDate = i.OrderDate,
		o.ProductName = i.ProductName,
		o.PublicationName = i.PublicationName,
		o.PublicationProductCode = i.PublicationProductCode,
		o.PublicationRevenueCode = i.PublicationRevenueCode,
		o.SubscriptionOfferName = i.SubscriptionOfferName,
		o.StarterIssues = i.StarterIssues,
		o.TotalIssues = i.TotalIssues,
		o.Copies = i.Copies,
		o.Quantity = i.Quantity,
		o.ParentOrderItemID = i.ParentOrderItemID,
		o.FulfilledDate = i.FulfilledDate,
		o.RefundedDate = i.RefundedDate,
		o.SubTotal = i.SubTotal,
		o.TaxTotal = i.TaxTotal,
		o.GrandTotal = i.GrandTotal,
		o.ProductID = i.ProductID,
		o.DateUpdated = GETDATE(),
		o.IsMergedToUAD = 'false',
		o.DateMergedToUAD = null
	from ImportOrder o
	join @import i on o.OrderID = i.OrderID 


	insert into ImportOrder (OrderID,OrderItemID,SubscriberID,SubscriberFirstName,SubscriberLastName,ShippingFirstName,ShippingLastName,ShippingAddressLine1,
		ShippingCity,ShippingState,ShippingPostalCode,ShippingCountry,BillingFirstName,BillingLastName,BillingAddressLine1,BillingCity,
		BillingState,BillingPostalCode,BillingCountry,CreatedbyRep,OrderDate,ProductName,PublicationName,PublicationProductCode,
		PublicationRevenueCode,SubscriptionOfferName,StarterIssues,TotalIssues,Copies,Quantity,ParentOrderItemID,FulfilledDate,
		RefundedDate,SubTotal,TaxTotal,GrandTotal,ProductID)
	select i.OrderID,i.OrderItemID,i.SubscriberID,i.SubscriberFirstName,i.SubscriberLastName,i.ShippingFirstName,i.ShippingLastName,i.ShippingAddressLine1,
		i.ShippingCity,i.ShippingState,i.ShippingPostalCode,i.ShippingCountry,i.BillingFirstName,i.BillingLastName,i.BillingAddressLine1,i.BillingCity,
		i.BillingState,i.BillingPostalCode,i.BillingCountry,i.CreatedbyRep,i.OrderDate,i.ProductName,i.PublicationName,i.PublicationProductCode,
		i.PublicationRevenueCode,i.SubscriptionOfferName,i.StarterIssues,i.TotalIssues,i.Copies,i.Quantity,i.ParentOrderItemID,i.FulfilledDate,
		i.RefundedDate,i.SubTotal,i.TaxTotal,i.GrandTotal,i.ProductID
	from @import i
	left outer join ImportOrder o on i.OrderID = o.OrderID
	where o.OrderID is null

END