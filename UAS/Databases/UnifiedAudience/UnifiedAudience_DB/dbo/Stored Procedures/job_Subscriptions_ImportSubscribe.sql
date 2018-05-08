CREATE PROCEDURE job_Subscriptions_ImportSubscribe
@Xml xml
AS
BEGIN

	SET NOCOUNT ON

		DECLARE @docHandle int,
				@ActiveEmailStatusID int,
				@UnSubscribeEmailStatusID int,
				@UnverifiedEmailStatusID int
				
		Declare @subID table (SubscriptionID int)			
				
		select @ActiveEmailStatusID = EmailStatusID from EmailStatus where Status = 'Active'
		select @UnSubscribeEmailStatusID = EmailStatusID from EmailStatus where Status = 'UnSubscribe'
		select @UnverifiedEmailStatusID  = EmailStatusID from EmailStatus where Status = 'Unverified'
		
		CREATE TABLE #import 
			(  
				EmailAddress varchar(100), PubID int, FirstName varchar(100), LastName varchar(100), Reason varchar(200), RequestDate datetime
			) 
		CREATE INDEX EA_1 on #import (EmailAddress)

		EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml  

		Insert Into #import (EmailAddress,PubID,FirstName,LastName,Reason,RequestDate)
			select x.EmailAddress, p.pubID, x.FirstName,x.LastName,x.Reason, x.RequestDate
			from
			(
				SELECT 
					EmailAddress,PubCode,FirstName,LastName,Reason,RequestDate
				FROM OPENXML(@docHandle, N'/XML/Emails')   
				WITH   
				(  
					EmailAddress varchar(100) 'EmailAddress', PubCode varchar(50) 'PubCode', FirstName varchar(100) 'FirstName', LastName varchar(100) 'LastName',Reason varchar(200) 'Reason', RequestDate datetime 'RequestDate'
					
				)  
			) x join Pubs p on x.PubCode = p.pubcode
		where rtrim(ltrim(isnull(x.Emailaddress,''))) <> '' 

		EXEC sp_xml_removedocument @docHandle   

		
	DECLARE @EmailAddress varchar(100) 
	DECLARE @PubID int  
	DECLARE @FirstName varchar(100)
	DECLARE @LastName varchar(100)
	DECLARE @Reason varchar(200)
	DECLARE @RequestDate datetime,
			@newSubscriptionID int

	DECLARE c cursor
	FOR 
		Select EmailAddress, PubID, FirstName, LastName, Reason, RequestDate
		From #import 
	
	OPEN c

	FETCH NEXT FROM c INTO @EmailAddress, @PubID, @FirstName, @LastName, @Reason, @RequestDate

	WHILE @@FETCH_STATUS = 0
	BEGIN

			delete from @subID
			
			insert into @subID
			select distinct s.SubscriptionID from Subscriptions s with (NOLOCK) join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID where ps.EMAIL = @EmailAddress and FNAME = @FirstName and LNAME = @LastName
			
			IF NOT EXISTS(Select top 1 subscriptionID from @subID)
				Begin
					insert into @subID
					select distinct s.SubscriptionID from Subscriptions s with (NOLOCK) join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID where ps.EMAIL = @EmailAddress  and (FNAME = @FirstName or LNAME = @LastName)
				End
			
			IF NOT EXISTS(Select top 1 subscriptionID from @subID)
				Begin
					insert into @subID
					select distinct s.SubscriptionID from Subscriptions s with (NOLOCK) join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID where ps.EMAIL = @EmailAddress
				End
			
			--Make sure Email exists in Subscriptions
			--Get Subscription by email
			IF NOT EXISTS(Select top 1 subscriptionID from @subID)
				BEGIN
					DECLARE @MaxSeq int = (Select isnull(MAX(SEQUENCE),0) +1 From Subscriptions)
				
					INSERT INTO Subscriptions (SEQUENCE,EMAIL,FNAME,LNAME,EmailExists,PhoneExists,FaxExists,IsExcluded,Latitude,Longitude,IsLatLonValid,LatLonMsg 
						,Igrp_No,categoryID,TransactionID,Qdate,Transactiondate)
					VALUES(@MaxSeq,@EmailAddress,@FirstName,@LastName,'true','false','false','false',0,0,'false','No Address' 
						,NEWID(),10,10,GETDATE(),GETDATE())
				
					set @newSubscriptionID  = @@IDENTITY
				
					insert into @subID
					SELECT @newSubscriptionID 
				END

			MERGE PubSubscriptions AS target
			USING (SELECT s.SubscriptionID, @pubID as pubID FROM @subID s) AS source (SubscriptionID, pubID)
			ON (target.subscriptionID = source.SubscriptionID and target.pubID = source.pubID)
			WHEN NOT MATCHED BY TARGET THEN
				INSERT  (SubscriptionID,PubID,Email, EmailStatusID,StatusUpdatedDate,StatusUpdatedReason, Qualificationdate )
					VALUES(subscriptionID,@pubID,@EmailAddress, @ActiveEmailStatusID,@RequestDate,@Reason, GETDATE())
			WHEN MATCHED AND target.EmailStatusID = @UnverifiedEmailStatusID
				THEN UPDATE SET StatusUpdatedDate = @RequestDate, StatusUpdatedReason = @Reason, EmailStatusID = @ActiveEmailStatusID;
		
			insert into PubSubscriptionDetail (PubSubscriptionID,SubscriptionID,CodesheetID)
			select x.pubsubscriptionID, x.SubscriptionID, x.codesheetID
			from
			(
				Select ps.pubsubscriptionID, s.SubscriptionID, c.codesheetID
				from @subID s 
					join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID 
					join ResponseGroups rg on rg.PubID = ps.PubID
					join CodeSheet c on c.ResponseGroupID  = rg.ResponseGroupID
				Where ps.PubID = @PubID and rg.ResponseGroupName = 'PubCode'
			) x 
				left outer join PubSubscriptionDetail psd on x.PubSubscriptionID = psd.PubSubscriptionID and x.CodeSheetID = psd.CodesheetID
			where psd.PubSubscriptionDetailID is null
			
			insert into SubscriptionDetails (SubscriptionID,MasterID)
			select x.SubscriptionID, x.MasterID
			from
			(
				Select distinct s.SubscriptionID, cmb.MasterID
				from @subID s 
					join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID 
					join ResponseGroups rg on rg.PubID = ps.PubID
					join CodeSheet c on c.ResponseGroupID  = rg.ResponseGroupID
					join CodeSheet_Mastercodesheet_Bridge cmb on cmb.codesheetID = c.codesheetID
				Where ps.PubID = @PubID and rg.ResponseGroupName = 'PubCode'
			) x 
				left outer join SubscriptionDetails sd on sd.SubscriptionID = x.SubscriptionID and sd.MasterID = x.MasterID
			where sd.SubscriptionID is null
					
		FETCH NEXT FROM c INTO @EmailAddress, @PubID, @FirstName, @LastName, @Reason, @RequestDate
	END
	CLOSE c
	DEALLOCATE c
	
	DROP TABLE #import 

end	