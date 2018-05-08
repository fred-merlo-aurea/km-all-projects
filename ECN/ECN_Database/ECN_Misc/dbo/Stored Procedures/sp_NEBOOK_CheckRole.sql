CREATE proc [dbo].[sp_NEBOOK_CheckRole]
(	@surveyID int,
	@userID int
)
as
Begin
	declare @customerID int,
			@userCustomerID int

	select @customerID = customerid from ecn5_collector..survey where surveyid = @surveyID
	select @userCustomerID = customerID from ecn5_accounts..users where userID = @userID

	if @customerID = @userCustomerID
		select 1
	else
	Begin
		if exists (select * from NEBook_SuperRegions nsr join NEBOOK_Region_SuperRegion rs on nsr.superregionID = rs.superregionID join nebook_customers c on rs.regionID = c.regionID where nsr.userID = @userID and customerID = @customerID)
			select 1
		else
		Begin
			if exists (select * from nebook_regions r join nebook_customers c on r.regionID = c.regionID where userID = @userID and customerID = @customerID)
				select 1
			else
				select 0
		end
	end  
End
