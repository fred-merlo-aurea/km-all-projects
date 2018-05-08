CREATE PROCEDURE [dbo].[sp_UpdateOptInOptOut]
	(
	@xmldata xml,
	@status varchar(20),
	@keyword varchar(50)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @dt datetime
	
	set @dt = GETDATE();

	--declare @SMSOptIn TABLE (EmailID int);	
	--	--SELECT MobileValues.SMS.value('./@Mobile','VARCHAR(10)') as mobile
	--	--		FROM @xmldata.nodes('/Blast') as MobileValues(SMS) 
		

	--	insert into @SMSOptIn
	--	select distinct e.emailID
	--	from
	--		[Emails] e with (NOLOCK) join
	--		[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c with (NOLOCK) on e.CustomerID = c.CustomerID join
	--			(
	--			SELECT MobileValues.SMS.value('./@Mobile','VARCHAR(10)') as mobile
	--			FROM @xmldata.nodes('/Blast') as MobileValues(SMS) 
	--			) inn on e.mobile = inn.mobile
	--	where
	--			c.TextPowerKWD = @keyword

	--	UPDATE [Emails]
	--	SET [Emails].SMSOptIn = @status, DateUpdated= @dt
	--	FROM [Emails] e join @SMSOptIn s on e.EmailID = s.EmailID 
		
	

		--SELECT MobileValues.SMS.value('./@Mobile','VARCHAR(10)') as mobile
		--		FROM @xmldata.nodes('/Blast') as MobileValues(SMS) 
		
		WITH SMS_CTE (EmailID)
		AS
		(
			select distinct e.emailID
			from
				[Emails] e with (NOLOCK) join
				[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c with (NOLOCK) on e.CustomerID = c.CustomerID join
					(
					SELECT MobileValues.SMS.value('./@Mobile','VARCHAR(10)') as mobile
					FROM @xmldata.nodes('/Blast') as MobileValues(SMS) 
					) inn on e.mobile = inn.mobile
			where
					c.TextPowerKWD = @keyword
		)
		UPDATE [Emails]
		SET [Emails].SMSOptIn = @status, DateUpdated= @dt
		FROM [Emails] e join SMS_CTE s on e.EmailID = s.EmailID 
		
		if(@status='optout')
		BEGIN
			update [EmailGroups]
			set [EmailGroups].SMSEnabled='False' ,LastChanged= @dt			
			From [EmailGroups] e join  SMS_CTE s on e.EmailID = s.EmailID 
		END
		
END
