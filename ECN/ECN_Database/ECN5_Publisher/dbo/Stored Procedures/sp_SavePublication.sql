CREATE proc [dbo].[sp_SavePublication] 
(
	@PublicationID 		int,
	@PublicationName 	varchar(100),
	@PublicationType	varchar(50),
	@CategoryID			int,
	@Circulation		int,
	@FrequencyID		int,
	@PublicationCode 	varchar(50),
	@CustomerID			int,
	@GroupID			int,
	@Active				bit,
	@ContactAddress1 	varchar(100),
	@ContactAddress2 	varchar(100),
	@ContactEmail 		varchar(100),
	@ContactPhone 		varchar(25),
	@ContactFormLink	varchar(255),
	@EnableSubscription	Bit,
	@SubscriptionOption int,
	@SubscriptionFormLink varchar(255),
	@LogoURL			varchar(255),
	@LogoLink			Varchar(255),
	@UserID				int
)
as
Begin
	if exists(select PublicationID from [PUBLICATION] where PublicationName = @PublicationName and PublicationID <> @PublicationID and customerID = @CustomerID)
	Begin
		RAISERROR('Publication already exists!!!',16,1)
	end
	else
	Begin
		if exists(select PublicationID from [PUBLICATION] where PublicationCode = @PublicationCode and PublicationID <> @PublicationID and customerID = @CustomerID and len(@PublicationCode) > 0)
		Begin
			RAISERROR('Publication Alias already exists!!!',16,1)
		end
		else
		begin
			if @PublicationID = 0 
			Begin
				if (@GroupID = 0)
				Begin
					select @GroupID = GroupID from ecn5_communicator..groups where CustomerID = @customerID and GroupName = @PublicationName +' Subscribers'
					if isnull(@GroupID,0) = 0 
					Begin
						Insert into ecn5_communicator..groups 
							(CustomerID,FolderID,GroupName,GroupDescription,
							OwnerTypeCode,MasterSupression,PublicFolder,OptinHTML,
							OptinFields,AllowUDFHistory)
						values
						(@customerID, 0, @PublicationName +' Subscribers',@PublicationName + ' Subscribers', 
						'customer', null, 0, '', 
						'EmailAddress|SubscribeTypeCode|FormatTypeCode|GroupID|CustomerID|','N')

						set @GroupID = @@IDENTITY
					end
				End

				INSERT INTO [PUBLICATION]
				( 
					[PublicationName],[PublicationType],[CategoryID],[Circulation],[FrequencyID],
					[PublicationCode],[CustomerID],[GroupID],[Active],
					[ContactAddress1],[ContactAddress2],[ContactEmail],[ContactPhone],ContactFormLink,
					[EnableSubscription],[SubscriptionOption],[SubscriptionFormLink],
					[LogoURL],[LogoLink],
					[CreatedUserID],[CreatedDate],[UpdatedDate]
				)
				VALUES
				(
					
					@PublicationName,@PublicationType,
					case when @CategoryID=0 then NULL else @categoryID end,@Circulation,
					case when @FrequencyID=0 then NULL else @FrequencyID end,
					@PublicationCode,@CustomerID,@GroupID,@Active,
					@ContactAddress1,@ContactAddress2,@ContactEmail,@ContactPhone,ltrim(rtrim(@ContactFormLink)),
					@EnableSubscription,@SubscriptionOption,ltrim(rtrim(@SubscriptionFormLink)),
					@LogoURL,@LogoLink,
					@UserID,getdate(),getdate()
				)
				set @PublicationID = @@IDENTITY
			End
			Else
			Begin
				select @GroupID = GroupID from [PUBLICATION] where PublicationID = @PublicationID

				if exists (select groupID from ecn5_communicator..Groups where groupID = @groupID)
				Begin
					update ecn5_communicator..Groups 
					set GroupName = @PublicationName +' Subscribers',
						GroupDescription= @PublicationName +' Subscribers'
					where 
						GroupID = @groupID
				end
				else
				Begin
					Insert into ecn5_communicator..Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,MasterSupression,PublicFolder,OptinHTML,OptinFields,AllowUDFHistory)
					values (@CustomerID, 0, @PublicationName +' Subscribers', @PublicationName +' Subscribers', 'customer', 0, 1, '', '', 'N')
					
					set @groupID = @@IDENTITY    

				end

				Update [PUBLICATION]
				Set PublicationName = @PublicationName,
					[PublicationType] = @PublicationType,
					[CategoryID] = case when @CategoryID=0 then NULL else @categoryID end,
					[Circulation] = @Circulation,
					[FrequencyID] = case when @FrequencyID=0 then NULL else @FrequencyID end,
					[PublicationCode] = @PublicationCode,
					GroupID = @GroupID,
					Active = @Active,
					ContactAddress1 = @ContactAddress1, 
					ContactAddress2 = @ContactAddress2,
					ContactEmail = @ContactEmail, 
					ContactPhone = @ContactPhone, 					
					ContactFormLink =  ltrim(rtrim(@ContactFormLink)),
					EnableSubscription = @EnableSubscription,
					[SubscriptionOption]=	@SubscriptionOption,
					[SubscriptionFormLink] = ltrim(rtrim(@SubscriptionFormLink)),
					[LogoURL] = @LogoURL,
					[LogoLink] = @LogoLink,
					[CreatedUserID] = @UserID,
					UpdatedDate = getdate()
				where
					PublicationID = @PublicationID
			End
			select @PublicationID as ID
		end
	end
End
