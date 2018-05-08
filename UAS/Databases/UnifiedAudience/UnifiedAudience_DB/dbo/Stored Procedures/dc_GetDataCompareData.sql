CREATE PROCEDURE [dbo].[dc_GetDataCompareData]
@processCode varchar(50),
@dcTargetCodeId int,
@id int, --ProductId/PubId or BrandId or 0 for consensus
@MatchType varchar(50)
AS
	begin
		set nocount on
		--36	Data Compare Target
		declare @prodCodeId int = (select CodeId from UAD_LOOKUP..Code where CodeTypeId in (select CodeTypeId from UAD_LOOKUP..CodeType where CodeTypeName='Data Compare Target' and IsActive=1) and IsActive=1 and CodeName='Product')
		declare @brandCodeId int = (select CodeId from UAD_LOOKUP..Code where CodeTypeId in (select CodeTypeId from UAD_LOOKUP..CodeType where CodeTypeName='Data Compare Target' and IsActive=1) and IsActive=1 and CodeName='Brand')
		declare @conCodeId int = (select CodeId from UAD_LOOKUP..Code where CodeTypeId in (select CodeTypeId from UAD_LOOKUP..CodeType where CodeTypeName='Data Compare Target' and IsActive=1) and IsActive=1 and CodeName='Consensus')
		
		----------this needs to run on client UAD
		if(@dcTargetCodeId = @conCodeId)--Consensus
			begin
				if (@MatchType = 'Matched')
				Begin
					select distinct s.SubscriptionID
					from DataCompareProfile d with(nolock) 
					join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					where d.ProcessCode = @processCode
				End
				else
				Begin
					select *
					from DataCompareProfile d with(nolock) 
					left outer join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					where d.ProcessCode = @processCode
					and s.SubscriptionID is null				
				End
			end		
		else if(@dcTargetCodeId = @prodCodeId)--Product
			begin
				if (@MatchType = 'Matched')
				Begin
					select distinct s.SubscriptionID
					from DataCompareProfile d with(nolock) 
					join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
					where d.ProcessCode = @processCode
					and ps.PubID = @id
				End
				else if (@MatchType = 'NonMatched')
				Begin
					select *
					from DataCompareProfile d with(nolock) 
					left outer join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					where d.ProcessCode = @processCode
					and s.SubscriptionID is null					
				End
				else if (@MatchType = 'MatchedNotInProduct')
				Begin
					select distinct s.SubscriptionID
					from DataCompareProfile d with(nolock) 
					join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					left outer join
						(
								select distinct s1.SubscriptionID
								from DataCompareProfile d1 with(nolock) 
								join Subscriptions s1 with (Nolock) on d1.IGRP_NO = s1.IGrp_No
								join PubSubscriptions ps with (nolock) on s1.SubscriptionID = ps.SubscriptionID
								where d1.ProcessCode = @processCode
								and ps.PubID = @id			
							) x on s.SubscriptionID = x.SubscriptionID
					where d.ProcessCode = @processCode and
						x.SubscriptionID is null
				End						
			end
		else if(@dcTargetCodeId = @brandCodeId)--Brand
			begin
				if (@MatchType = 'Matched')
				Begin
					select distinct s.SubscriptionID
					from DataCompareProfile d with(nolock) 
					join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
					join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
					where d.ProcessCode = @processCode
					and bd.BrandID = @id
				end
				else if (@MatchType = 'NonMatched')
				Begin
					select *
					from DataCompareProfile d with(nolock) 
					left outer join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					where d.ProcessCode = @processCode
					and s.SubscriptionID is null				
				End
				else if (@MatchType = 'MatchedNotInBrand')
				Begin
					select distinct s.SubscriptionID
					from DataCompareProfile d with(nolock) 
					join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
					left outer join
						(
								select distinct s1.SubscriptionID
								from DataCompareProfile d1 with(nolock) 
								join Subscriptions s1 with (Nolock) on d1.IGRP_NO = s1.IGrp_No
								join PubSubscriptions ps with (nolock) on s1.SubscriptionID = ps.SubscriptionID
								join branddetails bd with (nolock)  on bd.pubID = ps.pubID  
								where d1.ProcessCode = @processCode
								and bd.BrandID = @id			
							) x on s.SubscriptionID = x.SubscriptionID
					where d.ProcessCode = @processCode and
						x.SubscriptionID is null
				End				
			end
	end
