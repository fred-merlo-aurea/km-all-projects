CREATE PROCEDURE [dbo].[e_Subscriptions_Update_Address]
@SubscriptionID	int,
@Address varchar(100) = '',
@MailStop varchar(50) = '',
@Address3 varchar(100) = '',
@City varchar(50) = '',
@State varchar(50) = '',
@Zip varchar(10) = '',
@CountryID int = 0,
@Phone varchar(50) = '',
@Fax varchar(50) = '',
@Email varchar(100),
@UpdatedByUserID int,
@OverWrite bit
AS
BEGIN
	
	SET NOCOUNT ON
	
	declare @Country varchar(100)
	select @Country=ShortName  
	from UAD_Lookup..Country 
	where CountryID = @CountryID
	
	declare @ResetGeoCodesinSubscriptions bit = 0

	if @OverWrite = 0
		Begin			
			declare @UpdateAddressinSubscriptions bit = 0
	
			--select top 1 
			--		@UpdateAddressinSubscriptions = (Case When ISNULL(Address,'')!='' AND ISNULL(City,'')!='' AND ISNULL(State,'')!='' AND ISNULL(Zip,'')!='' then 1 else 0 end)
			--from
			--		Subscriptions  with (NOLOCK)
			--where
			--		SubscriptionID = @SubscriptionID

	
			if (ISNULL(@Address,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@State,'')!='' AND ISNULL(@Zip,'')!='')
				Begin
					set @UpdateAddressinSubscriptions = 1
				End
			if @UpdateAddressinSubscriptions = 1
				Begin
					if exists (select top 1 1
								from Subscriptions s WITH (nolock) 
								where SubscriptionID = @SubscriptionID and
									(
									ISNULL(@Address,'') <> ISNULL(Address,'') or 
									ISNULL(@City,'') <> ISNULL(City,'') or 
									ISNULL(@State,'') <> ISNULL(State,'') or 
									ISNULL(@Zip,'') <>  ISNULL(Zip,'') or 
									ISNULL(@CountryID,0) <> ISNULL(CountryID,0)
									)
							)
							Begin
								set @ResetGeoCodesinSubscriptions = 1 
							end
				end
			Update s
				Set Address		= (Case When @UpdateAddressinSubscriptions = 1 then REPLACE(REPLACE(REPLACE(@Address, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else ADDRESS end),
					MailStop		= (Case When @UpdateAddressinSubscriptions = 1 then REPLACE(REPLACE(REPLACE(@MailStop, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else MAILSTOP end),
					Address3		= (Case When @UpdateAddressinSubscriptions = 1 then REPLACE(REPLACE(REPLACE(@Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else ADDRESS3 end),               
					City			= (Case When @UpdateAddressinSubscriptions = 1 then REPLACE(REPLACE(REPLACE(@City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else CITY end),
					State		= (Case When @UpdateAddressinSubscriptions = 1 then REPLACE(REPLACE(REPLACE(@State, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else STATE end),
					Zip			= (Case When @UpdateAddressinSubscriptions = 1 then REPLACE(REPLACE(REPLACE(@Zip, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else ZIP end),
					Country		= (Case When @UpdateAddressinSubscriptions = 1 then REPLACE(REPLACE(REPLACE(@Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else COUNTRY end),
					CountryID	= (Case When @UpdateAddressinSubscriptions = 1 then @CountryID else CountryID end),
					Latitude		= (Case When @ResetGeoCodesinSubscriptions = 1 then NULL	else Latitude end),
					Longitude	= (Case When @ResetGeoCodesinSubscriptions = 1 then NULL	else Longitude end),
					IsLatLonValid = (Case When @ResetGeoCodesinSubscriptions = 1 then 0		else IsLatLonValid end),
					LatLonMsg	= (Case When @ResetGeoCodesinSubscriptions = 1 then ''	else LatLonMsg end),
					Phone		= (Case When ISNULL(@Phone,'')!='' then REPLACE(REPLACE(REPLACE(@Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else PHONE end),
					Fax			= (Case When ISNULL(@Fax,'')!='' then REPLACE(REPLACE(REPLACE(@Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else FAX end),
					Email		= (Case When ISNULL(@Email,'')!='' then @Email else EMAIL end),
					emailexists	= (Case When ltrim(rtrim(isnull((Case When ISNULL(@Email,'')!='' then @Email else EMAIL end),''))) <> '' then 1 else 0 end), 
					Faxexists	= (Case When ltrim(rtrim(isnull((Case When ISNULL(@Fax,'')!='' then REPLACE(REPLACE(REPLACE(@Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else FAX end),''))) <> '' then 1 else 0 end), 
					PhoneExists	= (Case When ltrim(rtrim(isnull((Case When ISNULL(@Phone,'')!='' then REPLACE(REPLACE(REPLACE(@Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') else PHONE end),''))) <> '' then 1 else 0 end),
					IsMailable	= 
					(Case 
						When (@UpdateAddressinSubscriptions = 0 or (@UpdateAddressinSubscriptions = 1 and @ResetGeoCodesinSubscriptions = 0)) then IsMailable
						When @ResetGeoCodesinSubscriptions = 1 and @CountryID in(1 ,2) then 0
						When @CountryID  >=3 and (LEN(@Address) = 0 or LEN(@State) = 0 or LEN(@Country) = 0)  then 0 else 1
					end	),
					DateUpdated	= GETDATE(),
					UpdatedByUserID = @UpdatedByUserID
			   From Subscriptions s
			   where SubscriptionID = @SubscriptionID
		end
	else
	Begin
			if exists (select top 1 1
						from Subscriptions  WITH (nolock) 
						where SubscriptionID = @SubscriptionID and
							(
							ISNULL(@ADDRESS,'') <> ISNULL(address,'') or 
							ISNULL(@CITY,'') <> ISNULL(CITY,'') or 
							ISNULL(@STATE,'') <> ISNULL(STATE,'') or 
							ISNULL(@ZIP,'') <>  ISNULL(ZIP,'') or 
							ISNULL(@CountryID,0) <> ISNULL(CountryID,0)
							))
				Begin
					set @ResetGeoCodesinSubscriptions = 1 
				end

			Update Subscriptions 
			Set Address = @Address, 
				MailStop = @MailStop, 
				City = @City, 
				Address3 = @Address3, 
				State = @State, 
				Zip = @Zip,
				CountryID = @CountryID, 
				Country = @Country, 
				Phone = @Phone, 
				Fax = @Fax, 
				Email = @Email, 
				EmailExists = Case When Len(@Email) > 0 then 1 else 0 end,
				PhoneExists = Case When Len(@Phone) > 0 then 1 else 0 end,
				FaxExists = Case When Len(@Fax) > 0 then 1 else 0 end,
				Latitude	= (Case When @ResetGeoCodesinSubscriptions = 1 then NULL else Latitude end),
				Longitude	= (Case When @ResetGeoCodesinSubscriptions = 1 then NULL else Longitude end),
				IsLatLonValid = (Case When @ResetGeoCodesinSubscriptions = 1 then 0 else IsLatLonValid end),
				LatLonMsg	= (Case When @ResetGeoCodesinSubscriptions = 1 then '' else LatLonMsg end),
				DateUpdated = GETDATE(),
				UpdatedByUserID = @UpdatedByUserID,
				IsMailable	= (Case When @ResetGeoCodesinSubscriptions = 1 and @CountryID in(1 ,2) then 0
					When @CountryID  >=3 and (LEN(@Address) = 0 or LEN(@State) = 0 or LEN(@Country) = 0)  then 0 else 1 end	)			 
			where SubscriptionID = @SubscriptionID	
		end

end