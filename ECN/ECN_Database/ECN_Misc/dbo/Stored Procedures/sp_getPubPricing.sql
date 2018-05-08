CREATE proc [dbo].[sp_getPubPricing] 
(
	@CustomerID int,
	@totalnewsletter int,
	@NewsletterIDs varchar(2000),
	@YearSubscribed int
)
as
Begin
	declare @PriceID int,
			@ActualRate decimal(10,2),
			@RegRate  decimal(10,2),
			@withNewsletterID int,
			@withNewsLetterPrice decimal(10,2),
			@allocationpercent decimal(10,2)


	set @priceID = 0
	set @ActualRate = 0
	set @RegRate = 0
	set @withNewsletterID= 0
	set @allocationpercent = 1

	if @totalnewsletter > 10
		set @totalnewsletter = 10

	declare @Newsletters TABLE (GroupID int, regularprice decimal(10,2), actualprice decimal(10,2))

	insert into @Newsletters (GroupID)
	select * from fn_split(@NewsletterIDs, ',')

	select @priceID = PriceID, @withNewsletterID= WithNewsletter 
	from canon_paidpub_pricing 
	where	CustomerID = @customerID and 
			combofor = @totalnewsletter and 
			WithNewsletter in (select GroupID from @Newsletters)

	if @PriceID = 0
		select @priceID = PriceID from canon_paidpub_pricing where CustomerID = @customerID and  combofor = @totalnewsletter and WithOutNewsletter not in (select GroupID from @Newsletters)
	else
		select @withNewsLetterPrice = (case when @YearSubscribed = 1 then RegularRate1yr when @YearSubscribed = 2 then RegularRate2yr else RegularRate3yr end)
		from canon_paidpub_pricing where CustomerID = @customerID and  WithNewsletter = @withNewsletterID and combofor = 1

	select	@ActualRate = (case when @YearSubscribed = 1 then ActualRate1yr when @YearSubscribed = 2 then ActualRate2yr else ActualRate3yr end),
			@RegRate	= (case when @YearSubscribed = 1 then RegularRate1yr when @YearSubscribed = 2 then RegularRate2yr else RegularRate3yr end)
	from	canon_paidpub_pricing 
	where	CustomerID = @customerID and  PriceID = @PriceID

	if @withNewsletterID > 0
	Begin
		--select @withNewsLetterPrice, @RegRate
		set @allocationpercent =(@withNewsLetterPrice*100)/@RegRate 
	end

	--select @priceID, @allocationpercent , (@ActualRate*@allocationpercent)/100,  (@ActualRate-(@ActualRate*@allocationpercent)/100)/(@totalnewsletter-1), @totalnewsletter totalnewletter, @withNewsLetterPrice as newletterprice, @withNewsletterID as WithNewsletterID, @ActualRate as actualrate, @RegRate as regularrate

	--select * from canon_paidpub_pricing where	PriceID = 4


	if @withNewsletterID > 0
	Begin
		update @Newsletters set regularprice=(@RegRate*@allocationpercent)/100, actualprice=(@ActualRate*@allocationpercent)/100 where GroupID = @withNewsletterID
		update @Newsletters set regularprice=(@RegRate-(@RegRate*@allocationpercent)/100)/(case when (@totalnewsletter-1) = 0 then 1 else @totalnewsletter-1 end), actualprice=(@ActualRate-(@ActualRate*@allocationpercent)/100)/(case when (@totalnewsletter-1) = 0

 then 1 else @totalnewsletter-1 end) where GroupID <> @withNewsletterID
	end
	else
		update @Newsletters set regularprice=@RegRate/@totalnewsletter, actualprice=@ActualRate/@totalnewsletter where GroupID <> @withNewsletterID

	--select g.groupname, n.GroupID , Round(regularprice,0) as regularprice, Round(actualprice,0) as Actualprice from @newsletters n join ecn5_communicator..groups g on n.groupID = g.groupID
	select g.groupname, n.GroupID , regularprice as regularprice, actualprice as Actualprice, regularprice-actualprice as savings from @newsletters n join ecn5_communicator..groups g on n.groupID = g.groupID
end
