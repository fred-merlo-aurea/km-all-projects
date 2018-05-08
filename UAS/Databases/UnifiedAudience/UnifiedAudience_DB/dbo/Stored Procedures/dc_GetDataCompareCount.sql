create procedure dc_GetDataCompareCount
@processCode varchar(50),
@dcTargetCodeId int,
@id int --ProductId/PubId or BrandId or 0 for consensus
as
	begin
		set nocount on
		declare @returnCount int = 0
		--Count comes from Subscriptions (consensus)
		--	or PubSubscriptions (product)
		--	or vw_BrandConsensus (brand)
		--need count of Profiles - get DcTargetCodeId from DataCompareView
		--36	Data Compare Target
		declare @prodCodeId int = (select CodeId from UAD_LOOKUP..Code where CodeTypeId in (select CodeTypeId from UAD_LOOKUP..CodeType where CodeTypeName='Data Compare Target' and IsActive=1) and IsActive=1 and CodeName='Product')
		declare @brandCodeId int = (select CodeId from UAD_LOOKUP..Code where CodeTypeId in (select CodeTypeId from UAD_LOOKUP..CodeType where CodeTypeName='Data Compare Target' and IsActive=1) and IsActive=1 and CodeName='Brand')
		declare @conCodeId int = (select CodeId from UAD_LOOKUP..Code where CodeTypeId in (select CodeTypeId from UAD_LOOKUP..CodeType where CodeTypeName='Data Compare Target' and IsActive=1) and IsActive=1 and CodeName='Consensus')
		
		----------this needs to run on client UAD
		if(@dcTargetCodeId = @conCodeId)--Consensus
			begin
				set @returnCount = (select count(distinct s.SubscriptionID)
									from DataCompareProfile d with(nolock) 
									join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
									join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
									where d.ProcessCode = @processCode)
			end		
		else if(@dcTargetCodeId = @prodCodeId)--Product
			begin
				set @returnCount = (select count(distinct s.SubscriptionID)
									from DataCompareProfile d with(nolock) 
									join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
									join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
									where d.ProcessCode = @processCode
									and ps.PubID = @id)
			end
		else if(@dcTargetCodeId = @brandCodeId)--Brand
			begin
				set @returnCount = (select count(distinct s.SubscriptionID)
									from DataCompareProfile d with(nolock) 
									join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
									join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
									join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
									where d.ProcessCode = @processCode
									and bd.BrandID = @id)
			end
		----------this needs to run on client UAD

		select @returnCount
	end

