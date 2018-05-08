CREATE proc [dbo].[e_Publication_Save] 
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
			if @PublicationID <= 0 
			Begin
				INSERT INTO Publication
				( 
					[PublicationName],[PublicationType],[CategoryID],[Circulation],[FrequencyID],
					[PublicationCode],[CustomerID],[GroupID],[Active],
					[ContactAddress1],[ContactAddress2],[ContactEmail],[ContactPhone],ContactFormLink,
					[EnableSubscription],[SubscriptionOption],[SubscriptionFormLink],
					[LogoURL],[LogoLink],
					[CreatedUserID],[CreatedDate],[IsDeleted]
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
					@UserID,getdate(),0
				)
				set @PublicationID = @@IDENTITY
			End
			Else
			Begin
				Update Publication
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
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					PublicationID = @PublicationID
			End
			select @PublicationID as ID
End