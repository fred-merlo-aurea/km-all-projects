create procedure job_NCOA_AddressUpdate
@xml xml
as
	SET NOCOUNT ON           
	DECLARE @docHandle int
    declare @insertcount int

	CREATE TABLE #NCOAimport 
	(
					SequenceID int NOT NULL,
					Address1 varchar(100) null,
					Address2 varchar(100) null,
					City varchar(50) null,
					RegionCode varchar(50) null,
					ZipCode varchar(50) null,
					Plus4 varchar(10) null,
					PublisherID int not null,
					PublicationID int not null,
					ProcessCode varchar(50) not null
	)
	CREATE NONCLUSTERED INDEX [IDX_SequenceID] ON #NCOAimport (SequenceID ASC)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into #NCOAimport 
	(
		SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,PublisherID,PublicationID,ProcessCode 
	)  

	SELECT SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,PublisherID,PublicationID,ProcessCode 
	FROM OPENXML(@docHandle, N'/XML/NCOA') 
	WITH   
	(  
		SequenceID int 'SequenceID',
		Address1 varchar(100) 'Address1',
		Address2 varchar(100) 'Address2',
		City varchar(50) 'City',
		RegionCode varchar(50) 'RegionCode',
		ZipCode varchar(50) 'ZipCode',
		Plus4 varchar(10) 'Plus4',
		PublisherID int 'PublisherID',
		PublicationID int 'PublicationID',
		ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle

	declare @publisherID int = (select distinct PublisherID from #NCOAimport)
    declare @publicationID int = (select distinct PublicationID from #NCOAimport)
    
    delete #NCOAimport where SequenceID not in (Select SequenceID from Subscription with(nolock))
    
	declare @batchCount int = (select count(i.SequenceID)
								from #NCOAimport i)
								--join Subscription s with(nolock) on i.SequenceID = s.SequenceID)

	declare @userID int = (select UserID from UAS..[User] where EmailAddress = 'NcoaImport@TeamKM.com')
	declare @batchID int
	if(@batchCount > 0)
			begin
				insert into Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
				values(@publicationID,@userID,@batchCount,'false',getdate(),getdate());
				set @batchID = (select @@IDENTITY);
			end
		--select * from Batch where PublicationID=7
		--select * from UserLog where FromObjectValues='job_NCOA_AddressUpdate'
		
	--foreach address in import where oldAddress = KmAddress update - log before/after in userlog table
	declare @SequenceID int
	declare @Address1 varchar(100)
	declare @Address2 varchar(100)
	declare @City varchar(50)
	declare @RegionCode varchar(50)
	declare @ZipCode varchar(50)
	declare @Plus4 varchar(10)
	declare @ProcessCode varchar(50)
	declare @SubscriptionID int
	declare @SubscriberID int
	
	
	DECLARE c CURSOR
	FOR 
					select i.SequenceID,i.Address1,i.Address2,i.City,i.RegionCode,i.ZipCode,i.Plus4,i.ProcessCode,s.SubscriptionID,s.SubscriberID
					from #NCOAimport i
					join Subscription s with(nolock) on i.SequenceID = s.SequenceID
					join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
					where i.PublisherID = @publisherID
	OPEN c
	FETCH NEXT FROM c INTO @SequenceID,@Address1,@Address2,@City,@RegionCode,@ZipCode,@Plus4,@ProcessCode,@SubscriptionID,@SubscriberID
	WHILE @@FETCH_STATUS = 0
	BEGIN
	                
			declare @historySubscriptionID int
			--create HistorySubscription record
			insert into HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
											IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
											SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
											OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
											Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
											AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
											SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
											LockDate,LockDateRelease,LockedByUserID,PhoneExt)
            
			 select 
					SubscriptionID,PublisherID,s.SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
					IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,s.DateCreated,
					s.DateUpdated,s.CreatedByUserID,s.UpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
					OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
					Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
					AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
					sub.DateCreated,sub.DateUpdated,sub.CreatedByUserID,sub.UpdatedByUserID,sub.DateCreated,sub.CreatedByUserID,IsLocked,
					LockDate,LockDateRelease,LockedByUserID,PhoneExt
			from Subscription s with(nolock)
			join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
			where s.SubscriptionID = @subscriptionID;
			set @historySubscriptionID = (select @@IDENTITY);

			declare @historyId int
			Insert Into History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,HistoryPaidID,
																											HistoryPaidBillToID,DateCreated,CreatedByUserID)
			Values(@batchID,@batchCount,@publisherID,@publicationID,@subscriberID,@subscriptionID,@historySubscriptionID,0,0,GETDATE(),@userID);
			set @historyId = (select @@IDENTITY);
			
			declare @userLogTypeId int = (select CodeId from uas..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
			declare @appId int = (select ApplicationID from uas..Application with(nolock) where ApplicationName='Circulation')
			declare @userLogId int
			
			Insert Into UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
			Values(@appId,@userLogTypeId,@userID,'Subscriber','job_NCOA_AddressUpdate','',GETDATE());
			set @userLogId = (select @@IDENTITY);
			
			Insert Into HistoryToUserLog (HistoryID,UserLogID)
			Values(@historyId,@userLogId);

			--Now actually update the address
			Update Subscriber
			set Address1 = @Address1,
				Address2 = @Address2,
				City = @City,
				RegionCode = @RegionCode,
				RegionID = (select RegionID from UAS..Region with(nolock) where RegionCode = @RegionCode),
				ZipCode = @ZipCode,
				Plus4 = @Plus4,
				IsAddressValidated = 'false',
				Latitude = 0,
				Longitude = 0,
				AddressValidationDate = null,
				AddressValidationMessage = '',
				DateUpdated = GETDATE(),
				UpdatedByUserID = @userID
			where Subscriber.SubscriberID = @SubscriberID
			
			--Now update the ActionID_Current and Previous
			declare @isPaid bit = (select IsPaid from Subscription with(nolock) where SubscriptionID = @SubscriptionID)
			declare @currentActionID int = (select ActionID_Current from Subscription with(nolock) where SubscriptionID = @SubscriptionID)
			declare @catCodeID int = (select CategoryCodeID from Action where ActionID = @currentActionID)
			declare @newActionID int = 0
			IF(@isPaid = 'true')
				begin
					set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 2 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

					Update Subscription
					set ActionID_Previous = @currentActionID,
						ActionID_Current = @newActionID --TransactionCodeID=2 TransactionCodeValue = 21
					where SubscriptionID = @subscriptionID 
				end
			else
				begin
					set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 17 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

					Update Subscription
					set ActionID_Previous = @currentActionID,
						ActionID_Current = @newActionID --TransactionCodeID=17 TransactionCodeValue = 21
					where SubscriptionID = @subscriptionID 
				end
						
		FETCH NEXT FROM c INTO @SequenceID,@Address1,@Address2,@City,@RegionCode,@ZipCode,@Plus4,@ProcessCode,@SubscriptionID,@SubscriberID
	END
	CLOSE c
	DEALLOCATE c
	
	DROP TABLE #NCOAimport
GO



