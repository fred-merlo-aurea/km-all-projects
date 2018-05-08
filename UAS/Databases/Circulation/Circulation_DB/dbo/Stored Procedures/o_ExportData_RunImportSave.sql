﻿CREATE PROCEDURE [dbo].[o_ExportData_RunImportSave]
@UserID int,
@xml xml
AS

DECLARE @docHandle int
DECLARE @soIDs TABLE (SubscriberID int, UniqueID int)
DECLARE @subIDs TABLE (SubscriberID int, SubscriptionID int)
DECLARE @ThisUserID int = @UserID

--DECLARE @Import TABLE
--CREATE TYPE DEImportTable as Table
--(
--[UniqueID] int not null,
--[SubscriberID] int null, [PublicationID] int null,[SubscriptionID] int null, [Batch] int null, [Hisbatch] varchar(100) null, 
--[Hisbatch1] varchar(100) null, [Hisbatch2] varchar(100) null, [Hisbatch3] varchar(100) null, [Pubcode] varchar(100) null, 
--[SequenceID] int null, [Cat] int null, [Xact] int null, [XactDate] DateTime null,[Fname] varchar(50) null, [Lname] varchar(50) null,
--[Title] varchar(255) null, [Company] varchar(100) null, [Address] varchar(100) null, [Mailstop] varchar(100) null, [City] varchar(50) null, 
--[State] varchar(50) null, [Zip] varchar(50) null, [Plus4] varchar(50) null, [County] varchar(50) null, [Country] varchar(50) null, 
--[CRTY] int null, [Phone] varchar(50) null, [Fax] varchar(50) null, [Mobile] varchar(50) null,[Email] varchar(255) null, 
--[Website] varchar(255) null, [AcctNum] varchar(50) null,[ORIGSSRC] int null, [SUBSRC] int null, [Copies] int null, [NANQ] int null, 
--[Qsource] int null, [Qdate] DateTime null,[Cdate] DateTime null,[Par3C] int null, [EmailID] int null, [Verify] varchar(100) null,
--[Interview] varchar(100) null, [Mail] varchar(100) null, [Old_Date] DateTime null,[Old_QSRC] varchar(100) null, [MBR_ID] int null, 
--[MBR_Flag] varchar(100) null, [MBR_Reject] varchar(100) null, [SPECIFY] varchar(200) null, [SIC] varchar(200) null, [EMPLOY] varchar(200) null, 
--[SALES] varchar(200) null, [IMB_SERIAL1] int null, [IMB_SERIAL2] int null, [IMB_SERIAL3] int null, [Business] varchar(200) null, 
--[BUSNTEXT] varchar(200) null, [Function] varchar(200) null, [FUNCTEXT] varchar(200) null,[DEMO1] varchar(200) null, [DEMO1TEXT] varchar(200) null, 
--[DEMO2] varchar(200) null, [DEMO3] varchar(200) null, [DEMO4] varchar(200) null, [DEMO5] varchar(200) null, [DEMO6] varchar(200) null, 
--[DEMO6TEXT] varchar(200) null, [DEMO7] varchar(200) null, [DEMO8] varchar(200) null, [DEMO9] varchar(200) null, [DEMO10] varchar(200) null, 
--[DEMO10TEXT] varchar(200) null, [DEMO11] varchar(200) null, [DEMO12] varchar(200) null, [DEMO14] varchar(200) null, [DEMO15] varchar(200) null, 
--[DEMO16] varchar(200) null, [DEMO18] varchar(200) null, [DEMO19] varchar(200) null, [DEMO20] varchar(200) null, [DEMO21] varchar(200) null, 
--[DEMO22] varchar(200) null, [DEMO23] varchar(200) null, [DEMO24] varchar(200) null, [DEMO25] varchar(200) null, [DEMO26] varchar(200) null, 
--[DEMO27] varchar(200) null, [DEMO28] varchar(200) null, [DEMO29] varchar(200) null, [DEMO40] varchar(200) null, [DEMO41] varchar(200) null, 
--[DEMO42] varchar(200) null, [DEMO43] varchar(200) null, [DEMO44] varchar(200) null, [DEMO45] varchar(200) null, [DEMO46] varchar(200) null, 
--[DEMO31] varchar(200) null, [DEMO32] varchar(200) null, [DEMO33] varchar(200) null, [DEMO34] varchar(200) null, [DEMO35] varchar(200) null, 
--[DEMO36] varchar(200) null, [DEMO37] varchar(200) null,[DEMO38] varchar(200) null, [SECBUS] varchar(200) null, [SECFUNC] varchar(200) null, 
--[Business1] varchar(200) null, [Function1] varchar(200) null, [Income1] varchar(200) null, [Age1] int null, [Home_Value] varchar(200) null, 
--[JOBT1] varchar(200) null, [JOBT1TEXT] varchar(200) null, [JOBT2] varchar(200) null, [JOBT3] varchar(200) null, [TOE1] varchar(200) null, 
--[TOE2] varchar(200) null, [AOI1] varchar(200) null, [AOI2] varchar(200) null, [AOI3] varchar(200) null, [PROD1] varchar(200) null, 
--[PROD1TEXT] varchar(200) null, [BUYAUTH] varchar(200) null, [IND1] varchar(200) null, [IND1TEXT] varchar(200) null, [STATUS] bit null, 
--[PRICECODE] int null, [NUMISSUES] int null, [CPRATE] float null,[TERM] int null, [ISSTOGO] int null, [CARDTYPE] int null, [CARDTYPECC] int null, 
--[CCNUM] varchar(16) null, [CCEXPIRE] varchar(50) null, [CCNAME] varchar(100) null, [AMOUNTPD] float null, [AMOUNT] float null, 
--[BALDUE] float null, [AMTEARNED] float null, [AMTDEFER] float null, [PAYDATE] DateTime null,[STARTISS] DateTime null,[EXPIRE] DateTime null,
--[NWEXPIRE] DateTime null, [DELIVERCODE] int null
--)
DECLARE @Import DEImportTable
EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

-- IMPORT FROM XML TO TEMP TABLE
insert into @Import 
(
	UniqueID,
	SubscriberID, PublicationID,SubscriptionID,Batch, Hisbatch, Hisbatch1, Hisbatch2, Hisbatch3, Pubcode,SequenceID, Cat, Xact,XactDate,Fname, Lname,Title,Company, [Address],Mailstop,City,
	State,Zip,Plus4,County,Country,CRTY,Phone,Fax, Mobile,Email, Website,AcctNum,ORIGSSRC, SUBSRC, Copies, NANQ, Qsource,Qdate,Cdate,Par3C,
	EmailID, Verify,Interview,Mail,Old_Date,Old_QSRC, MBR_ID,MBR_Flag, MBR_Reject,SPECIFY, SIC, EMPLOY,SALES, IMB_SERIAL1,IMB_SERIAL2,IMB_SERIAL3,
	Business, BUSNTEXT, [Function], FUNCTEXT,DEMO1, DEMO1TEXT,DEMO2,DEMO3,DEMO4,DEMO5,DEMO6,DEMO6TEXT,DEMO7,DEMO8,DEMO9,DEMO10,DEMO10TEXT, DEMO11, DEMO12, 
	DEMO14, DEMO15, DEMO16, DEMO18, DEMO19, DEMO20, DEMO21, DEMO22, DEMO23, DEMO24, DEMO25, DEMO26, DEMO27, DEMO28, DEMO29, DEMO40, DEMO41, DEMO42, 
	DEMO43, DEMO44, DEMO45, DEMO46, DEMO31, DEMO32, DEMO33, DEMO34, DEMO35, DEMO36, DEMO37,DEMO38, SECBUS, SECFUNC,Business1, Function1, Income1, Age1,
	Home_Value,JOBT1,JOBT1TEXT, JOBT2, JOBT3, TOE1, TOE2, AOI1, AOI2, AOI3, PROD1,PROD1TEXT,BUYAUTH,IND1, IND1TEXT, [STATUS],PRICECODE, NUMISSUES,CPRATE,
	TERM,ISSTOGO, CARDTYPE,CARDTYPECC, CCNUM, CCEXPIRE,CCNAME, AMOUNTPD, AMOUNT, BALDUE, AMTEARNED, AMTDEFER, PAYDATE,STARTISS,EXPIRE,NWEXPIRE, DELIVERCODE
)  
SELECT 
	UniqueID,
	SubscriberID, PublicationID,SubscriptionID,Batch, Hisbatch, Hisbatch1, Hisbatch2, Hisbatch3, Pubcode,SequenceID, Cat, Xact,XactDate,Fname, Lname,Title,Company, [Address],Mailstop,City,
	State,Zip,Plus4,County,Country,CRTY,Phone,Fax, Mobile,Email, Website,AcctNum,ORIGSSRC, SUBSRC, Copies, NANQ, Qsource,Qdate,Cdate,Par3C,
	EmailID, Verify,Interview,Mail,Old_Date,Old_QSRC, MBR_ID,MBR_Flag, MBR_Reject,SPECIFY, SIC, EMPLOY,SALES, IMB_SERIAL1,IMB_SERIAL2,IMB_SERIAL3,
	Business, BUSNTEXT, [Function], FUNCTEXT,DEMO1, DEMO1TEXT,DEMO2,DEMO3,DEMO4,DEMO5,DEMO6,DEMO6TEXT,DEMO7,DEMO8,DEMO9,DEMO10,DEMO10TEXT, DEMO11, DEMO12, 
	DEMO14, DEMO15, DEMO16, DEMO18, DEMO19, DEMO20, DEMO21, DEMO22, DEMO23, DEMO24, DEMO25, DEMO26, DEMO27, DEMO28, DEMO29, DEMO40, DEMO41, DEMO42, 
	DEMO43, DEMO44, DEMO45, DEMO46, DEMO31, DEMO32, DEMO33, DEMO34, DEMO35, DEMO36, DEMO37,DEMO38, SECBUS, SECFUNC,Business1, Function1, Income1, Age1,
	Home_Value,JOBT1,JOBT1TEXT, JOBT2, JOBT3, TOE1, TOE2, AOI1, AOI2, AOI3, PROD1,PROD1TEXT,BUYAUTH,IND1, IND1TEXT, [STATUS],PRICECODE, NUMISSUES,CPRATE,
	TERM,ISSTOGO, CARDTYPE,CARDTYPECC, CCNUM, CCEXPIRE,CCNAME, AMOUNTPD, AMOUNT, BALDUE, AMTEARNED, AMTDEFER, PAYDATE,STARTISS,EXPIRE,NWEXPIRE,DELIVERCODE
FROM OPENXML(@docHandle, N'/XML/Subscriber')   
WITH   
(  
	UniqueID int 'UniqueID',
	SubscriberID int 'SubscriberID',
	PublicationID int 'PublicationID',
	SubscriptionID int 'SubscriptionID',
	Batch int 'Batch',
	Hisbatch varchar(100) 'Hisbatch',
	Hisbatch1 varchar(100) 'Hisbatch1',
	Hisbatch2 varchar(100) 'Hisbatch2',
	Hisbatch3 varchar(100) 'Hisbatch3',
	Pubcode varchar(100) 'Pubcode',
	SequenceID int 'SequenceID',
	Cat int 'Cat',
	Xact int 'Xact',
	XactDate DateTime 'XactDate',
	Fname varchar(50) 'Fname',
	Lname varchar(50) 'Lname',
	Title varchar(255) 'Title',
	Company varchar(100) 'Company',
	[Address] varchar(100) 'Address',
	Mailstop varchar(100) 'Mailstop',
	City varchar(50) 'City',
	[State] varchar(50) 'State',
	Zip varchar(50) 'Zip',
	Plus4 varchar(50) 'Plus4',
	County varchar(50) 'County',
	Country varchar(50) 'Country',
	CRTY int 'CRTY',
	Phone varchar(50) 'Phone',
	Fax varchar(50) 'Fax',
	Mobile varchar(50) 'Mobile',
	Email varchar(255) 'Email',
	Website varchar(255) 'Website',
	AcctNum varchar(50) 'AcctNum',
	ORIGSSRC int 'ORIGSSRC',
	SUBSRC int 'SUBSRC',
	Copies int 'Copies',
	NANQ int 'NANQ',
	Qsource int 'Qsource',
	Qdate DateTime 'Qdate',
	Cdate DateTime 'Cdate',
	Par3C int 'Par3C',
	EmailID int 'EmailID',
	Verify varchar(100) 'Verify',
	Interview varchar(100) 'Interview',
	Mail varchar(100) 'Mail',
	Old_Date DateTime 'Old_Date',
	Old_QSRC varchar(100) 'Old_QSRC',
	MBR_ID int 'MBR_ID',
	MBR_Flag varchar(100) 'MBR_Flag',
	MBR_Reject varchar(100) 'MBR_Reject',
	SPECIFY varchar(200) 'SPECIFY',
	SIC varchar(200) 'SIC',
	EMPLOY varchar(200) 'EMPLOY',
	SALES varchar(200) 'SALES',
	IMB_SERIAL1 int 'IMB_SERIAL1',
	IMB_SERIAL2 int 'IMB_SERIAL2',
	IMB_SERIAL3 int 'IMB_SERIAL3',
	Business varchar(200) 'Business',
	BUSNTEXT varchar(200) 'BUSNTEXT',
	[Function] varchar(200) 'Function',
	FUNCTEXT varchar(200) 'FUNCTEXT',
	DEMO1 varchar(200) 'DEMO1',
	DEMO1TEXT varchar(200) 'DEMO1TEXT',
	DEMO2 varchar(200) 'DEMO2',
	DEMO3 varchar(200) 'DEMO3',
	DEMO4 varchar(200) 'DEMO4',
	DEMO5 varchar(200) 'DEMO5',
	DEMO6 varchar(200) 'DEMO6',
	DEMO6TEXT varchar(200) 'DEMO6TEXT',
	DEMO7 varchar(200) 'DEMO7',
	DEMO8 varchar(200) 'DEMO8',
	DEMO9 varchar(200) 'DEMO9',
	DEMO10 varchar(200) 'DEMO10',
	DEMO10TEXT varchar(200) 'DEMO10TEXT',
	DEMO11 varchar(200) 'DEMO11',
	DEMO12 varchar(200) 'DEMO12',
	DEMO14 varchar(200) 'DEMO14',
	DEMO15 varchar(200) 'DEMO15',
	DEMO16 varchar(200) 'DEMO16',
	DEMO18 varchar(200) 'DEMO18',
	DEMO19 varchar(200) 'DEMO19',
	DEMO20 varchar(200) 'DEMO20',
	DEMO21 varchar(200) 'DEMO21',
	DEMO22 varchar(200) 'DEMO22',
	DEMO23 varchar(200) 'DEMO23',
	DEMO24 varchar(200) 'DEMO24',
	DEMO25 varchar(200) 'DEMO25',
	DEMO26 varchar(200) 'DEMO26',
	DEMO27 varchar(200) 'DEMO27',
	DEMO28 varchar(200) 'DEMO28',
	DEMO29 varchar(200) 'DEMO29',
	DEMO40 varchar(200) 'DEMO40',
	DEMO41 varchar(200) 'DEMO41',
	DEMO42 varchar(200) 'DEMO42',
	DEMO43 varchar(200) 'DEMO43',
	DEMO44 varchar(200) 'DEMO44',
	DEMO45 varchar(200) 'DEMO45',
	DEMO46 varchar(200) 'DEMO46',
	DEMO31 varchar(200) 'DEMO31',
	DEMO32 varchar(200) 'DEMO32',
	DEMO33 varchar(200) 'DEMO33',
	DEMO34 varchar(200) 'DEMO34',
	DEMO35 varchar(200) 'DEMO35',
	DEMO36 varchar(200) 'DEMO36',
	DEMO37 varchar(200) 'DEMO37',
	DEMO38 varchar(200) 'DEMO38',
	SECBUS varchar(200) 'SECBUS',
	SECFUNC varchar(200) 'SECFUNC',
	Business1 varchar(200) 'Business1',
	Function1 varchar(200) 'Function1',
	Income1 varchar(200) 'Income1',
	Age1 int 'Age1',
	Home_Value varchar(200) 'Home_Value',
	JOBT1 varchar(200) 'JOBT1',
	JOBT1TEXT varchar(200) 'JOBT1TEXT',
	JOBT2 varchar(200) 'JOBT2',
	JOBT3 varchar(200) 'JOBT3',
	TOE1 varchar(200) 'TOE1',
	TOE2 varchar(200) 'TOE2',
	AOI1 varchar(200) 'AOI1',
	AOI2 varchar(200) 'AOI2',
	AOI3 varchar(200) 'AOI3',
	PROD1 varchar(200) 'PROD1',
	PROD1TEXT varchar(200) 'PROD1TEXT',
	BUYAUTH varchar(200) 'BUYAUTH',
	IND1 varchar(200) 'IND1',
	IND1TEXT varchar(200) 'IND1TEXT',
	[STATUS] bit 'STATUS',
	PRICECODE int 'PRICECODE',
	NUMISSUES int 'NUMISSUES',
	CPRATE float 'CPRATE',
	TERM int 'TERM',
	ISSTOGO int 'ISSTOGO',
	CARDTYPE int 'CARDTYPE',
	CARDTYPECC int 'CARDTYPECC',
	CCNUM varchar(16) 'CCNUM',
	CCEXPIRE varchar(50) 'CCEXPIRE',
	CCNAME varchar(100) 'CCNAME',
	AMOUNTPD float 'AMOUNTPD',
	AMOUNT float 'AMOUNT',
	BALDUE float 'BALDUE',
	AMTEARNED float 'AMTEARNED',
	AMTDEFER float 'AMTDEFER',
	PAYDATE DateTime 'PAYDATE',
	STARTISS DateTime 'STARTISS',
	EXPIRE DateTime 'EXPIRE',
	NWEXPIRE DateTime 'NWEXPIRE',
	DELIVERCODE int 'DELIVERCODE'	 
)  

EXEC sp_xml_removedocument @docHandle    
	
/**** UPDATES ****/
	/** Subscriber **/	
	BEGIN
		UPDATE Subscriber
		SET					
			FirstName = i.Fname,
			LastName = i.Lname,
			Title = i.Title,
			Company = i.Company,
			Address1 = i.Address,
			Address2 = i.Mailstop,
			City = i.City,
			RegionCode = i.State,
			RegionID = CASE WHEN (i.State != '' OR i.State is not null) THEN (SELECT RegionID FROM Region With(NoLock) Where RegionCode = i.State) ELSE NULL END,
			ZipCode = i.Zip,
			Plus4 = i.Plus4,
			County = i.County,
			Country = i.Country,
			CountryID = CASE WHEN (i.CRTY > 0) THEN i.CRTY								 
							 ELSE 0 END,
			Phone = i.Phone,
			Fax = i.Fax,
			Mobile = i.Mobile,
			Email = i.Email,
			Website = i.Website,
			Age = i.Age1,
			DateUpdated = GETDATE(),
			UpdatedByUserID = @ThisUserID	
		FROM @Import i
		WHERE Subscriber.SubscriberID = i.SubscriberID
		and EXISTS (SELECT SubscriberID FROM Subscriber With(NoLock) WHERE SubscriberID = i.SubscriberID)
	END
	
	/** Subscription **/	
	BEGIN
		UPDATE Subscription
		SET					
			SequenceID = i.SequenceID,
			ActionID_Current = CASE WHEN (i.Cat > 0 AND i.Xact > 0) 
									THEN (SELECT ActionID FROM Action With(NoLock)
									where CategoryCodeID = (SELECT CategoryCodeID FROM CategoryCode With(NoLock) where CategoryCodeValue = i.Cat)  
									and TransactionCodeID = (SELECT TransactionCodeID FROM TransactionCode With(NoLock) where TransactionCodeValue = i.Xact) 
									and ActionTypeID = 2)
									ELSE 0 END,
			DateUpdated = i.XactDate,
			AccountNumber = i.AcctNum,
			OriginalSubscriberSourceCode = i.ORIGSSRC,
			SubscriberSourceCode = i.SubSrc,
			Copies = i.Copies,
			QSourceID = i.Qsource, 
			QSourceDate = i.Qdate,
			DateCreated = i.Cdate,
			Par3cID = i.Par3C,			
			UpdatedByUserID = @ThisUserID	
			--SubscriptionStatusID = @STATUS	
		FROM @Import i
		WHERE Subscription.SubscriberID = i.SubscriberID and Subscription.PublicationID = i.PublicationID
		and EXISTS (SELECT SubscriberID FROM Subscription With(NoLock) WHERE SubscriberID = i.SubscriberID and PublicationID = i.PublicationID)
	END
	
	/** SubscriptionPaid **/	
	BEGIN
		UPDATE SubscriptionPaid
		SET					
			PriceCodeID = i.PRICECODE,
			TotalIssues = i.NUMISSUES,       	
			CPRate = i.CPRATE,
			--@TERM            					
			PaymentTypeID = i.CARDTYPE,     	
			CreditCardTypeID = i.CARDTYPECC,      	
			CCNumber = i.CCNUM,           	
			CCExpirationMonth = CASE WHEN (i.CCEXPIRE != '' OR i.CCEXPIRE IS NOT NULL) 
									THEN (Select SUBSTRING(i.CCEXPIRE,CHARINDEX('/',i.CCEXPIRE)-2,2)) --@CCExpMonth,
								ELSE NULL END,
			CCExpirationYear = CASE WHEN (i.CCEXPIRE != '' OR i.CCEXPIRE IS NOT NULL) 
									THEN (Select SUBSTRING(i.CCEXPIRE,CHARINDEX('/',i.CCEXPIRE)+1,LEN(i.CCEXPIRE))) --@CCExpYear,
								ELSE NULL END,        	
			CCHolderName = i.CCNAME,          	
			AmountPaid = i.AMOUNTPD,
			Amount = i.AMOUNT,          	
			BalanceDue = i.BALDUE,          					        	
			PaidDate = i.PAYDATE,         	
			StartIssueDate = i.STARTISS,        	
			ExpireIssueDate = i.EXPIRE,
			DeliverID = CASE WHEN (i.DELIVERCODE > 0)
							THEN (Select DeliverID from DeliverSubscriptionPaid Where DeliverCode = i.DELIVERCODE)	
						ELSE NULL END,
			DateUpdated = GETDATE(),
			UpdatedByUserID = @ThisUserID	
		FROM @Import i			
		WHERE SubscriptionPaid.SubscriptionID = i.SubscriptionID
		and EXISTS (SELECT SubscriptionID FROM SubscriptionPaid WHERE SubscriptionID = i.SubscriptionID)
	END
	
	/** DataImportExport **/	
	BEGIN
		UPDATE DataImportExport
		SET					
			Hisbatch1 = i.Hisbatch1,
			Hisbatch2 = i.Hisbatch2,
			Hisbatch3 = i.Hisbatch3,
			NANQ = i.NANQ,
			EmailID = i.EmailID,
			Verify = i.Verify,
			Interview = i.Interview,
			Mail = i.Mail,
			PrevQDate = i.Old_Date,
			PrevQSource = i.Old_QSRC,
			MemberID = i.MBR_ID,
			MemberFlag = i.MBR_Flag,
			MemberReject = i.MBR_Reject,
			IMBSerial1 = i.IMB_SERIAL1,
			IMBSerial2 = i.IMB_SERIAL2,
			IMBSerial3 = i.IMB_SERIAL3,
			HomeValue = i.Home_Value,
			IssuesToGo = i.ISSTOGO,
		 	AmountEarned = i.AMTEARNED,       	
			AmountDeferred = i.AMTDEFER, 	
			NewExpire = i.NWEXPIRE	     		    
		FROM @Import i
		WHERE DataImportExport.SubscriberID = i.SubscriberID
		and EXISTS (SELECT SubscriberID FROM Subscriber WHERE SubscriberID = i.SubscriberID)
	END
	
	
/**** INSERTS ****/
	/** Subscriber **/
	INSERT INTO Subscriber (ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,City,
						RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
						AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,
						Age,Income,Gender,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,tmpSubscriptionID,IsLocked)
			OUTPUT Inserted.SubscriberID, Inserted.tmpSubscriptionID 
			INTO @soIDs
	SELECT NULL,i.Fname,i.Lname,i.Company,i.Title,NULL,NULL,i.Address,i.Mailstop,i.City,
						i.State,
						CASE WHEN (i.State != '' OR i.State is not null) THEN (SELECT RegionID FROM Region With(NoLock) Where RegionCode = i.State) ELSE NULL END,
						i.Zip,i.Plus4,NULL,i.County,i.Country,CASE WHEN (i.CRTY > 0) THEN i.CRTY ELSE 0 END,
						NULL,NULL,0,
						NULL,NULL,NULL,i.Email,i.Phone,i.Fax,i.Mobile,i.Website,NULL,
						i.Age1,NULL,NULL,GETDATE(),NULL,@ThisUserID,NULL,i.UniqueID,0
	FROM @Import i
	WHERE i.SubscriberID = 0 --OR i.SubscriberID NOT IN (SELECT SubscriberID FROM Subscriber With(NoLock))
	
	/** Subscription **/
	INSERT INTO Subscription (SequenceID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,
						SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,
						OriginalSubscriberSourceCode,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,Par3cID,SubsrcTypeID,AccountNumber)
			OUTPUT Inserted.SubscriberID, Inserted.SubscriptionID 
			INTO @subIDs
	SELECT i.SequenceID,CASE WHEN (i.PublicationID > 0) THEN (SELECT PublisherID FROM Publication WHERE PublicationID = i.PublicationID) ELSE 0 END,s.SubscriberID,i.PublicationID,
				CASE WHEN (i.Cat > 0 AND i.Xact > 0) THEN (SELECT ActionID FROM Action With(NoLock) where CategoryCodeID = (SELECT CategoryCodeID FROM CategoryCode With(NoLock) where CategoryCodeValue = i.Cat)  
										and TransactionCodeID = (SELECT TransactionCodeID FROM TransactionCode With(NoLock) where TransactionCodeValue = i.Xact) and ActionTypeID = 2)
										ELSE 0 END,
						0,0,0,i.Qsource,i.Qdate,0,0,i.SubSrc,i.Copies,
						i.ORIGSSRC,i.Cdate,i.XactDate,@ThisUserID,NULL,i.Par3C,0,i.AcctNum
	FROM @Import i
	FULL JOIN @soIDs s on i.UniqueID = s.UniqueID
	WHERE i.UniqueID in (Select UniqueID FROM @soIDs)
	
	/** SubscriptionPaid **/
	INSERT INTO SubscriptionPaid (SubscriptionID,PriceCodeID,StartIssueDate,ExpireIssueDate,CPRate,Amount,AmountPaid,
						BalanceDue,PaidDate,TotalIssues,CheckNumber,CCNumber,CCExpirationMonth,CCExpirationYear,CCHolderName,
						CreditCardTypeID,PaymentTypeID,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,DeliverID)
	SELECT i.SubscriptionID,CASE WHEN (i.PRICECODE > 0) THEN i.PRICECODE ELSE 0 END,i.STARTISS,i.EXPIRE,i.CPRATE,i.AMOUNT,i.AMOUNTPD,
						i.BALDUE,i.PAYDATE,i.NUMISSUES,NULL,i.CCNUM,
						CASE WHEN (i.CCEXPIRE != '' OR i.CCEXPIRE IS NOT NULL) 
										THEN (Select SUBSTRING(i.CCEXPIRE,CHARINDEX('/',i.CCEXPIRE)-2,2)) --@CCExpMonth,
									ELSE NULL END,
						CASE WHEN (i.CCEXPIRE != '' OR i.CCEXPIRE IS NOT NULL) 
										THEN (Select SUBSTRING(i.CCEXPIRE,CHARINDEX('/',i.CCEXPIRE)+1,LEN(i.CCEXPIRE))) --@CCExpYear,
									ELSE NULL END,
									i.CCNAME,i.CARDTYPECC,i.CARDTYPE,GETDATE(),NULL,@ThisUserID,NULL,
						CASE WHEN (i.DELIVERCODE > 0)
							THEN (Select DeliverID from DeliverSubscriptionPaid Where DeliverCode = i.DELIVERCODE)
						ELSE NULL END
	FROM @Import i
	WHERE 
	(i.SubscriptionID > 0 OR i.SubscriptionID is not NULL) 
	AND
	(i.SubscriptionID not in (SELECT SubscriptionID FROM SubscriptionPaid))	
	
	INSERT INTO SubscriptionPaid (SubscriptionID,PriceCodeID,StartIssueDate,ExpireIssueDate,CPRate,Amount,AmountPaid,
						BalanceDue,PaidDate,TotalIssues,CheckNumber,CCNumber,CCExpirationMonth,CCExpirationYear,CCHolderName,
						CreditCardTypeID,PaymentTypeID,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,DeliverID)
	SELECT su.SubscriptionID,CASE WHEN (i.PRICECODE > 0) THEN i.PRICECODE ELSE 0 END,i.STARTISS,i.EXPIRE,i.CPRATE,i.AMOUNT,i.AMOUNTPD,
						i.BALDUE,i.PAYDATE,i.NUMISSUES,NULL,i.CCNUM,
						CASE WHEN (i.CCEXPIRE != '' OR i.CCEXPIRE IS NOT NULL) 
										THEN (Select SUBSTRING(i.CCEXPIRE,CHARINDEX('/',i.CCEXPIRE)-2,2)) --@CCExpMonth,
									ELSE NULL END,
						CASE WHEN (i.CCEXPIRE != '' OR i.CCEXPIRE IS NOT NULL) 
										THEN (Select SUBSTRING(i.CCEXPIRE,CHARINDEX('/',i.CCEXPIRE)+1,LEN(i.CCEXPIRE))) --@CCExpYear,
									ELSE NULL END,
									i.CCNAME,i.CARDTYPECC,i.CARDTYPE,GETDATE(),NULL,@ThisUserID,NULL,
						CASE WHEN (i.DELIVERCODE > 0)
							THEN (Select DeliverID from DeliverSubscriptionPaid Where DeliverCode = i.DELIVERCODE)
						ELSE NULL END
	FROM @Import i
	FULL JOIN @soIDs so on i.UniqueID = so.UniqueID
	FULL JOIN @subIDs su on so.SubscriberID = su.SubscriberID
	WHERE 	
	(su.SubscriptionID > 0 OR su.SubscriptionID is not NULL) 
	AND
	(su.SubscriptionID not in (SELECT SubscriptionID FROM SubscriptionPaid))	
	
	/** DataImportExport **/      	           					
    INSERT INTO DataImportExport (SubscriberID,Hisbatch1,Hisbatch2,Hisbatch3,NANQ,EmailID,Verify,Interview,Mail,PrevQDate,PrevQSource,
								MemberID,MemberFlag,MemberReject,IMBSerial1,IMBSerial2,IMBSerial3,HomeValue,IssuesToGo,AmountEarned,
								AmountDeferred,NewExpire)
	SELECT so.SubscriberID,i.Hisbatch1,i.Hisbatch2,i.Hisbatch3,i.NANQ,i.EmailID,i.Verify,i.Interview,i.Mail,i.Old_Date,i.Old_QSRC,
								i.MBR_ID,i.MBR_Flag,i.MBR_Reject,i.IMB_SERIAL1,i.IMB_SERIAL2,i.IMB_SERIAL3,i.Home_Value,i.ISSTOGO,
								i.AMTEARNED,i.AMTDEFER,i.NWEXPIRE
	FROM @import i
	FULL JOIN @soIDs so on i.UniqueID = so.UniqueID
	WHERE ((i.UniqueID in (Select UniqueID FROM @soIDs)) 
			OR (i.SubscriberID NOT IN (Select SubscriberID FROM Subscriber) AND i.SubscriberID > 0) 
			OR (so.SubscriberID NOT IN (Select SubscriberID FROM Subscriber)))	        	         	        	
			

/**** SubscriptionResponseMap ****/		
	/** UPDATE **/	
	----Set all current to not active
	Update SubscriptionResponseMap
	set IsActive = 0, ResponseOther = '', DateUpdated = GETDATE(), UpdatedByUserID = @ThisUserID
	FROM @Import i
	where SubscriptionResponseMap.SubscriptionID = i.SubscriptionID
							
	/** INSERT **/
	INSERT INTO SubscriptionResponseMap (SubscriptionID, ResponseID, IsActive, DateCreated, DateUpdated, 
										CreatedByUserID, UpdatedByUserID, ResponseOther)	
	Select i.SubscriptionID, r.ResponseID, 0, GETDATE(), NULL, 
			@ThisUserID, NULL, ''
	FROM @Import i
	FULL OUTER JOIN Response r on i.PublicationID = r.PublicationID
	WHERE i.SubscriptionID > 0
	AND
	r.ResponseID not in 
	(Select ResponseID from SubscriptionResponseMap Where (SubscriptionID > 0 AND SubscriptionID is not NULL) AND SubscriptionID = i.SubscriptionID)
	
	
	INSERT INTO SubscriptionResponseMap (SubscriptionID, ResponseID, IsActive, DateCreated, DateUpdated, 
										CreatedByUserID, UpdatedByUserID, ResponseOther)	
	Select su.SubscriptionID, r.ResponseID, 0, GETDATE(), NULL, 
			@ThisUserID, NULL, ''
	FROM @Import i
	FULL JOIN @soIDs so on i.UniqueID = so.UniqueID
	FULL JOIN @subIDs su on so.SubscriberID = su.SubscriberID
	FULL OUTER JOIN Response r on i.PublicationID = r.PublicationID
	WHERE su.SubscriptionID > 0
	AND
	r.ResponseID not in 
	(Select ResponseID from SubscriptionResponseMap Where (SubscriptionID > 0 AND SubscriptionID is not NULL) AND SubscriptionID = su.SubscriptionID)
	

	----FINAL UPDATE FOR ALL
	DECLARE @RowID int

	DECLARE c CURSOR
	FOR
	SELECT UniqueID FROM @Import

	OPEN c

	FETCH NEXT FROM c INTO @RowID

	WHILE @@FETCH_STATUS = 0
	BEGIN

	DECLARE @SubscriptionID int = (SELECT TOP 1 CASE WHEN (i.SubscriptionID > 0) THEN i.SubscriptionID ELSE su.SubscriptionID END FROM @Import i FULL JOIN @soIDs so on i.UniqueID = so.UniqueID FULL JOIN @subIDs su on so.SubscriberID = su.SubscriberID WHERE i.UniqueID = @RowID)
	DECLARE @PublicationID int = (SELECT TOP 1 PublicationID FROM @Import WHERE UniqueID = @RowID)
	DECLARE @Other varchar(2000)
	DECLARE @Codes varchar(max)
	
	SET @Codes = (SELECT TOP 1 Business FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'Business', @Codes;	
	--
	SET @Other = (SELECT TOP 1 BUSNTEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'Business', @Codes,@Other;
	SET @Codes = (SELECT TOP 1 [Function] FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'Function',@Codes
	--
	SET @Other = (SELECT TOP 1 FUNCTEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'Function', @Codes,@Other;
	SET @Codes = (SELECT TOP 1 DEMO1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO1',@Codes
	--
	SET @Other = (SELECT TOP 1 DEMO1TEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO1', @Codes,@Other;
	SET @Codes = (SELECT TOP 1 DEMO2 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO2',@Codes
	SET @Codes = (SELECT TOP 1 DEMO3 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO3',@Codes
	SET @Codes = (SELECT TOP 1 DEMO4 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO4',@Codes
	SET @Codes = (SELECT TOP 1 DEMO5 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO5',@Codes
	SET @Codes = (SELECT TOP 1 DEMO6 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO6',@Codes
	--
	SET @Other = (SELECT TOP 1 DEMO6TEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO6', @Codes,@Other;
	SET @Codes = (SELECT TOP 1 DEMO7 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO7',@Codes
	SET @Codes = (SELECT TOP 1 DEMO8 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO8',@Codes
	SET @Codes = (SELECT TOP 1 DEMO9 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO9',@Codes
	SET @Codes = (SELECT TOP 1 DEMO10 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO10',@Codes
	--
	SET @Other = (SELECT TOP 1 DEMO10TEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO10', @Codes,@Other;
	SET @Codes = (SELECT TOP 1 DEMO11 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO11',@Codes
	SET @Codes = (SELECT TOP 1 DEMO12 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO12',@Codes
	SET @Codes = (SELECT TOP 1 DEMO14 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO14',@Codes
	SET @Codes = (SELECT TOP 1 DEMO15 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO15',@Codes
	SET @Codes = (SELECT TOP 1 DEMO16 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO16',@Codes
	SET @Codes = (SELECT TOP 1 DEMO18 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO18',@Codes
	SET @Codes = (SELECT TOP 1 DEMO19 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO19',@Codes
	SET @Codes = (SELECT TOP 1 DEMO20 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO20',@Codes
	SET @Codes = (SELECT TOP 1 DEMO21 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO21',@Codes
	SET @Codes = (SELECT TOP 1 DEMO22 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO22',@Codes
	SET @Codes = (SELECT TOP 1 DEMO23 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO23',@Codes
	SET @Codes = (SELECT TOP 1 DEMO24 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO24',@Codes
	SET @Codes = (SELECT TOP 1 DEMO25 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO25',@Codes
	SET @Codes = (SELECT TOP 1 DEMO26 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO26',@Codes
	SET @Codes = (SELECT TOP 1 DEMO27 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO27',@Codes
	SET @Codes = (SELECT TOP 1 DEMO28 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO28',@Codes
	SET @Codes = (SELECT TOP 1 DEMO29 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO29',@Codes
	SET @Codes = (SELECT TOP 1 DEMO40 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO40',@Codes
	SET @Codes = (SELECT TOP 1 DEMO41 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO41',@Codes
	SET @Codes = (SELECT TOP 1 DEMO42 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO42',@Codes
	SET @Codes = (SELECT TOP 1 DEMO43 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO43',@Codes
	SET @Codes = (SELECT TOP 1 DEMO44 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO44',@Codes
	SET @Codes = (SELECT TOP 1 DEMO45 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO45',@Codes
	SET @Codes = (SELECT TOP 1 DEMO46 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO46',@Codes
	SET @Codes = (SELECT TOP 1 DEMO31 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO31',@Codes
	SET @Codes = (SELECT TOP 1 DEMO32 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO32',@Codes
	SET @Codes = (SELECT TOP 1 DEMO33 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO33',@Codes
	SET @Codes = (SELECT TOP 1 DEMO34 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO34',@Codes
	SET @Codes = (SELECT TOP 1 DEMO35 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO35',@Codes
	SET @Codes = (SELECT TOP 1 DEMO36 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO36',@Codes
	SET @Codes = (SELECT TOP 1 DEMO37 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO37',@Codes
	SET @Codes = (SELECT TOP 1 DEMO38 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'DEMO38',@Codes
	SET @Codes = (SELECT TOP 1 SECBUS FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'SECBUS',@Codes
	SET @Codes = (SELECT TOP 1 SECFUNC FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'SECFUNC',@Codes
	SET @Codes = (SELECT TOP 1 Business1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'Business1',@Codes
	SET @Codes = (SELECT TOP 1 Function1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'Function1',@Codes
	SET @Codes = (SELECT TOP 1 JOBT1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'JOBT1',@Codes
	--
	SET @Other = (SELECT TOP 1 JOBT1TEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'JOBT1', @Codes,@Other;
	SET @Codes = (SELECT TOP 1 JOBT2 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'JOBT2',@Codes
	SET @Codes = (SELECT TOP 1 JOBT3 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'JOBT3',@Codes
	SET @Codes = (SELECT TOP 1 TOE1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'TOE1',@Codes
	SET @Codes = (SELECT TOP 1 TOE2 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'TOE2',@Codes
	SET @Codes = (SELECT TOP 1 AOI1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'AOI1',@Codes
	SET @Codes = (SELECT TOP 1 AOI2 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'AOI2',@Codes
	SET @Codes = (SELECT TOP 1 AOI3 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'AOI3',@Codes
	SET @Codes = (SELECT TOP 1 PROD1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'PROD1',@Codes
	--
	SET @Other = (SELECT TOP 1 PROD1TEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'PROD1', @Codes,@Other;
	SET @Codes = (SELECT TOP 1 BUYAUTH FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'BUYAUTH',@Codes
	SET @Codes = (SELECT TOP 1 IND1 FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponse] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'IND1',@Codes 
	--
	SET @Other = (SELECT TOP 1 IND1TEXT FROM @Import WHERE UniqueID = @RowID)
	EXECUTE [o_ExportData_UpdateResponseOther] @ThisUserID,@Import,@SubscriptionID,@PublicationID,'IND1', @Codes,@Other;

	FETCH NEXT FROM c INTO @RowID
	END
	CLOSE c
	DEALLOCATE c
