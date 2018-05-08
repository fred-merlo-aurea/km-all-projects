
CREATE PROCEDURE e_SubscriberFinal_AddressSearch
	@Address varchar(255),
	@Mailstop varchar(255),
	@City varchar(255),
	@State varchar(50),
	@Zip varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select top 1
		   address
		  ,mailstop
		  ,city
		  ,State
		  ,zip
	from SubscriberFinal
	where Address = @address and MailStop = @mailstop and City = @city and State = @state and Zip = @zip
	
END
GO
