CREATE PROCEDURE dbo.sp_ecnLogin (  
	 @username VARCHAR(255),   
	 @password VARCHAR(255)  
)  
AS    
    
BEGIN     
	 DECLARE @userID INT, @customerID INT, @baseChannelID INT, @status VARCHAR(1)  
	   
	 -- Login Details [TABLE 0]
	 SELECT @userID = u.UserID, @customerID = c.CustomerID, @baseChannelID = c.BaseChannelID, @status = u.ActiveFlag  
	  FROM [Users] u   
	   JOIN [Customer] c ON u.CustomerID = c.CustomerID and c.IsDeleted = 0
	   JOIN [BaseChannel] bc ON c.BaseChannelID = bc.BaseChannelID and bc.IsDeleted = 0 
	  WHERE u.UserName = @userName AND u.Password = @password and u.IsDeleted = 0
	   
	 SELECT ISNULL(@userID,'0') AS 'UserID', 
		ISNULL(@customerID,'0') AS 'CustomerID', 
		ISNULL(@baseChannelID,'0') AS 'BaseChannelID', 
		ISNULL(@status,'N') AS 'ACTIVE'  
	 /* 
	 IF @userID > 0 AND @status = 'Y'  
	 BEGIN  
	  --Base User Details [TABLE 1]
	  SELECT UserName, Password, FullName, CommunicatorOptions, CollectorOptions, CreatorOptions, AccountsOptions, RoleID 
	  FROM Users 
	  WHERE UserID = @userID

	  --UserRole Details [TABLE 2]
	  SELECT a.DisplayName, ua.Active 
	  FROM Users u 
	    JOIN UserActions ua ON u.UserID = ua.UserID 
	    JOIN Actions a ON a.ActionID = ua.ActionID 
	  WHERE u.UserID = @userID

	  --Base Customer Details [TABLE 3]
	  SELECT CustomerName, ActiveFlag, CommunicatorLevel, CollectorLevel, CreatorLevel, PublisherLevel, CharityLevel, AccountsLevel,
	    Address, City, State, Country, Zip, WebAddress, Salutation, ContactName, c.FirstName, c.LastName, ContactTitle, Phone, Fax, c.Email, 
 	    SubscriptionsEmail, CustomerType, DemoFlag, IsStrategic, 
	    'IronPortMailingIP'=(SELECT ISNULL(ConfigValue,'') FROM CustomerConfig WHERE CustomerID = @customerID AND ConfigName = 'MailingIP'),
	    ISNULL(AccountExecutiveID,0)AS 'AccountExecutiveID', (ISNULL(s1.FirstName,'')+' '+ISNULL(s1.LastName,'')) AS 'AccountExecutiveName', ISNULL(s1.Email,'') AS 'AccountExecutiveEmail',
	    ISNULL(AccountManagerID,0) AS 'AccountManagerID', (ISNULL(s1.FirstName,'')+' '+ISNULL(s1.LastName,'')) AS 'AccountManagerName', ISNULL(s1.Email,'') AS 'AccountManagerEmail'
	  FROM [Customer] c 
	    LEFT OUTER JOIN Staff s1 on c.AccountExecutiveID = s1.StaffID 
	    LEFT OUTER JOIN Staff s2 on c.AccountManagerID = s2.StaffID
	  WHERE CustomerID = @customerID  

	  --Customer Email Credits [TABLE 4]
	  SELECT SUM(cl.Quantity) AS 'TotalCredits', SUM(cl.Used) AS 'UsedCredits',SUM(Quantity-Used) AS 'AvailableCredits', 
	    CONVERT(VARCHAR,MAX(cl.ExpirationDate),101) AS 'CreditsExpireOn'
	  FROM [Customer] c JOIN [CustomerLicense] cl ON c.customerID = cl.customerID 
	  WHERE cl.IsActive = 1 AND cl.Quantity > 0  AND ExpirationDate > getDate() AND LicenseTypeCode = 'emailblock10k' 
	    AND c.CustomerID = @customerID
	    GROUP BY cl.LicenseTypeCode 

	  --Customer Templates [TABLE 5]
	  SELECT TemplateTypeCode, HeaderSource 
	  FROM [CustomerTemplate] 
	  WHERE customerID = @customerID

	  --Customer Features [TABLE 6]
	  SELECT pd.ProductDetailName AS 'ProductFeatureName', cp.Active 
	  FROM [CustomerProduct] cp 
	    JOIN [Productdetail] pd ON cp.ProductDetailID = pd.ProductDetailID
	    JOIN [Product] p ON p.ProductID = pd.ProductID 
	  WHERE cp.CustomerID = @customerID  

	  --Base Channel Details Features [TABLE 7]
	  SELECT BaseChannelName, ChannelURL, BounceDomain, WebAddress, MasterCustomerID, IsBranding, ChannelPartnerImage
	  FROM [BaseChannel]
	  WHERE BaseChannelID = @baseChannelID 
	 END  
	*/
END