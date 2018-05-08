CREATE PROCEDURE [dbo].[sp_SaveSubscriptions]
@SubscriptionID	int,
@Fname	varchar(100),
@Lname	varchar(100),
@Company	varchar(100),
@Address	varchar(255),
@Address3 varchar(255),
@City	varchar(50),
@State	varchar(50),
@Zip	varchar(10),
@CountryID	int,
@Title	varchar(100),
@ForZip	varchar(50),
@Plus4	varchar(4),
@Phone	varchar(100),
@Fax	varchar(100),
@Mobile	varchar(30),
@Email	varchar(100),
@MailStop	varchar(255),
@MailPermission	bit,
@FaxPermission	bit,
@PhonePermission	bit,
@OtherProductsPermission	bit,
@ThirdPartyPermission	bit,
@EmailRenewPermission	bit,
@TextPermission	bit,
@Notes varchar(2000),
@UpdatedByUserID int
AS
BEGIN
	set nocount on
	
	declare @Country varchar(100)
	select @Country=ShortName  from UAD_Lookup..Country where CountryID = @CountryID
			
	declare @ResetGeoCodesinSubscriptions bit = 0
	
	if exists(	select top 1 1
			from	Subscriptions  WITH (nolock) 
			where 
					SubscriptionID = @SubscriptionID and
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


	UPDATE 
		Subscriptions 
	SET 
		 Fname = @Fname, 
		 Lname = @Lname ,
		 Company = @Company, 
		 Address = @Address, 
		 MailStop = @MailStop, 
		 Address3 = @Address3,
		 City = @City, 
		 State = @State, 
		 Zip = @Zip,
		 CountryID = @CountryID, 
		 COUNTRY = @COUNTRY, 
		 Title = @Title, 
		 ForZip = @ForZip, 
		 Plus4 = @Plus4, 
		 Phone = @Phone, 
		 Fax = @Fax, 
		 Mobile = @Mobile,
		 Email = @Email, 
		 MailPermission = @MailPermission, 
		 FaxPermission = @FaxPermission, 
		 PhonePermission = @PhonePermission, 
		 OtherProductsPermission = @OtherProductsPermission, 
		 ThirdPartyPermission = @ThirdPartyPermission, 
		 EmailRenewPermission = @EmailRenewPermission,
		 TextPermission = @TextPermission,
		 Notes = @Notes,
		 EmailExists = Case When Len(@Email) > 0 then 1 else 0 end,
		 PhoneExists = Case When Len(@Phone) > 0 then 1 else 0 end,
		 FaxExists = Case When Len(@Fax) > 0 then 1 else 0 end,
		 Latitude	= (Case When @ResetGeoCodesinSubscriptions = 1 then NULL else Latitude end),
		 Longitude	= (Case When @ResetGeoCodesinSubscriptions = 1 then NULL else Longitude end),
		 IsLatLonValid = (Case When @ResetGeoCodesinSubscriptions = 1 then 0 else IsLatLonValid end),
		 LatLonMsg	= (Case When @ResetGeoCodesinSubscriptions = 1 then '' else LatLonMsg end),
		 DateUpdated = GETDATE(),
		 UpdatedByUserID = @UpdatedByUserID

	where 
		SubscriptionID = @SubscriptionID
END