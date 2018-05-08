CREATE PROCEDURE [dbo].[dc_GetDataCompareSummary]
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
	declare @totalCount int
	
	IF OBJECT_ID('tempdb..#TempSummary') IS NOT NULL 
	BEGIN 
		DROP TABLE #TempSummary;
	END 
	
	CREATE TABLE #TempSummary (Field varchar(100), Same int, SamePercentage int, Different int, DifferentPercentage int, UADOnly int, UADOnlyPercentage int, FileOnly int, FileOnlyPercentage int)
	
	----------this needs to run on client UAD
	if(@dcTargetCodeId = @conCodeId)--Consensus
		begin
			if (@MatchType = 'Matched')
			Begin
				select @totalCount = count(distinct s.igrp_no) from DataCompareProfile d with(nolock) 
						join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
						where d.ProcessCode = @processCode
						
				insert into #TempSummary
				select
				'Email',
				count(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode
				
				insert into #TempSummary
				select
				'First Name & Last Name',
				count(case when isnull(s.Fname ,'') = isnull(d.Fname,'')  and  isnull(s.Lname,'') = isnull(d.Lname,'')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Fname,'') = isnull(d.Fname,'')   and  isnull(s.Lname,'') = isnull(d.Lname,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode
				
				insert into #TempSummary
				select
				'Complete Address',
				count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'')) and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,				
				count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode		
								
				insert into #TempSummary
				select
				'Address',
				count(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode
				
				insert into #TempSummary
				select
				'Address2',
				count(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode										
				
				insert into #TempSummary
				select
				'Address3',
				count(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode
			
				insert into #TempSummary
				select
				'City',
				count(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode
			
				insert into #TempSummary
				select
				'State',
				count(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode				

				insert into #TempSummary
				select
				'Zip',
				count(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode	
				
				insert into #TempSummary
				select
				'Country',
				count(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode	
				
				insert into #TempSummary
				select
				'Phone',
				count(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode	
				
				insert into #TempSummary
				select
				'Job Title',
				count(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode		
				
				insert into #TempSummary
				select
				'Company Name',
				count(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				where d.ProcessCode = @processCode																																											
			End
		end		
	else if(@dcTargetCodeId = @prodCodeId)--Product
		begin
			if (@MatchType = 'Matched')
			Begin
				select @totalCount = count(distinct s.igrp_no) 
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
					
				insert into #TempSummary
				select
				'Email',
				count(case when isnull(ps.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(ps.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(ps.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
				
				insert into #TempSummary
				select
				'First Name & Last Name',
				count(case when isnull(ps.FirstName ,'') = isnull(d.Fname,'')  and  isnull(ps.LastName,'') = isnull(d.Lname,'')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.FirstName,'') = isnull(d.Fname,'')   and  isnull(ps.LastName,'') = isnull(d.Lname,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(ps.FirstName,'') <> '' and isnull(d.Fname,'') <> '' and isnull(ps.FirstName,'') != isnull(d.Fname,'')) and (isnull(ps.LastName,'') <> '' and isnull(d.Lname,'') <> '' and isnull(ps.LastName,'') != isnull(d.Lname,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(ps.FirstName,'') <> '' and isnull(d.Fname,'') <> '' and isnull(ps.FirstName,'') != isnull(d.Fname,'')) and (isnull(ps.LastName,'') <> '' and isnull(d.Lname,'') <> '' and isnull(ps.LastName,'') != isnull(d.Lname,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(ps.FirstName,'') <> '' and isnull(d.Fname,'') = '') and (isnull(ps.LastName,'') <> '' and isnull(d.Lname,'') = '')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(ps.FirstName,'') <> '' and isnull(d.Fname,'') = '') and (isnull(ps.LastName,'') <> '' and isnull(d.Lname,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when (isnull(ps.FirstName,'') = '' and isnull(d.Fname,'') <> '') and (isnull(ps.LastName,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(ps.FirstName,'') = '' and isnull(d.Fname,'') <> '') and (isnull(ps.LastName,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
				
				insert into #TempSummary
				select
				'Complete Address',
				count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(ps.[Address1], '') = isnull(d.[ADDRESS],'') and
						isnull(ps.City,'') = isnull(d.City,'') and
						isnull(ps.RegionCode,'') = isnull(d.[State],'') and
						isnull(ps.ZipCode,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(ps.[Address1], '') = isnull(d.[ADDRESS],'') and
						isnull(ps.City,'') = isnull(d.City,'') and
						isnull(ps.RegionCode,'') = isnull(d.[State],'') and
						isnull(ps.ZipCode,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'')) and
						(isnull(ps.[Address1],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(ps.[Address1],'') != isnull(d.[ADDRESS],'')) and
						(isnull(ps.City,'') <> '' and isnull(d.City,'') <> '' and isnull(ps.City,'') != isnull(d.City,'')) and
						(isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') <> '' and isnull(ps.RegionCode,'') != isnull(d.[State],'')) and
						(isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') <> '' and isnull(ps.ZipCode,'') != isnull(d.Zip,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') and
						(isnull(ps.[Address1],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(ps.[Address1],'') != isnull(d.[ADDRESS],'')) and
						(isnull(ps.City,'') <> '' and isnull(d.City,'') <> '' and isnull(ps.City,'') != isnull(d.City,'')) and
						(isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') <> '' and isnull(ps.RegionCode,'') != isnull(d.[State],'')) and
						(isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') <> '' and isnull(ps.ZipCode,'') != isnull(d.Zip,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(ps.[Address1],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(ps.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') = '') and
						(isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(ps.[Address1],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(ps.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') = '') and
						(isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,				
				count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(ps.[Address1],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(ps.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(ps.RegionCode,'') = '' and isnull(d.[State],'') <> '') and
						(isnull(ps.ZipCode,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(ps.[Address1],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(ps.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(ps.RegionCode,'') = '' and isnull(d.[State],'') <> '') and
						(isnull(ps.ZipCode,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id	
								
				insert into #TempSummary
				select
				'Address',
				count(case when isnull(ps.[Address1],'') = isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.[Address1],'') = isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.[Address1],'') <> '' and isnull(d.[Address],'') <> '' and isnull(ps.[Address1],'') != isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.[Address1],'') <> '' and isnull(d.[Address],'') <> '' and isnull(ps.[Address1],'') != isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.[Address1],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.[Address1],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.[Address1],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.[Address1],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
				
				insert into #TempSummary
				select
				'Address2',
				count(case when isnull(ps.Address2,'') = isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address2,'') = isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Address2,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(ps.Address2,'') != isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address2,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(ps.Address2,'') != isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Address2,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address2,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.Address2,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address2,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id								
				
				insert into #TempSummary
				select
				'Address3',
				count(case when isnull(ps.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(ps.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(ps.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
			
				insert into #TempSummary
				select
				'City',
				count(case when isnull(ps.City,'') = isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.City,'') = isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.City,'') <> '' and isnull(d.City,'') <> '' and isnull(ps.City,'') != isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.City,'') <> '' and isnull(d.City,'') <> '' and isnull(ps.City,'') != isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
			
				insert into #TempSummary
				select
				'State',
				count(case when isnull(ps.RegionCode,'') = isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.RegionCode,'') = isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') <> '' and isnull(ps.RegionCode,'') != isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') <> '' and isnull(ps.RegionCode,'') != isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.RegionCode,'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.RegionCode,'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.RegionCode,'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id			

				insert into #TempSummary
				select
				'Zip',
				count(case when isnull(ps.ZipCode,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.ZipCode,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') <> '' and isnull(ps.ZipCode,'') != isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') <> '' and isnull(ps.ZipCode,'') != isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.ZipCode,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.ZipCode,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.ZipCode,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
				
				insert into #TempSummary
				select
				'Country',
				count(case when isnull(ps.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(ps.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(ps.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
				
				insert into #TempSummary
				select
				'Phone',
				count(case when isnull(ps.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(ps.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(ps.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id
				
				insert into #TempSummary
				select
				'Job Title',
				count(case when isnull(ps.Title,'') = isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Title,'') = isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(ps.Title,'') != isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(ps.Title,'') != isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id	
				
				insert into #TempSummary
				select
				'Company Name',
				count(case when isnull(ps.Company,'') = isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Company,'') = isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(ps.Company,'') != isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(ps.Company,'') != isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(ps.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(ps.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(ps.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(ps.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID 
				where d.ProcessCode = @processCode and ps.PubID = @id	
			End
			else if (@MatchType = 'MatchedNotInProduct')
			Begin
				select @totalCount = count(distinct s.igrp_no)
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
					
				insert into #TempSummary
				select
				'Email',
				count(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'First Name & Last Name',
				count(case when isnull(s.Fname ,'') = isnull(d.Fname,'')  and  isnull(s.Lname,'') = isnull(d.Lname,'')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Fname,'') = isnull(d.Fname,'')   and  isnull(s.Lname,'') = isnull(d.Lname,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Complete Address',
				count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'')) and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,				
				count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
								
				insert into #TempSummary
				select
				'Address',
				count(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Address2',
				count(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Address3',
				count(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
			
				insert into #TempSummary
				select
				'City',
				count(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
			
				insert into #TempSummary
				select
				'State',
				count(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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

				insert into #TempSummary
				select
				'Zip',
				count(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Country',
				count(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Phone',
				count(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Job Title',
				count(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Company Name',
				count(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				select @totalCount = count(distinct s.igrp_no) 
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
			
				insert into #TempSummary
				select 
				'Email',
				count(distinct(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
				
				insert into #TempSummary
				select
				'First Name & Last Name',
				count(distinct(case when isnull(s.Fname ,'') = isnull(d.Fname,'')  and  isnull(s.Lname,'') = isnull(d.Lname,'')  then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Fname,'') = isnull(d.Fname,'')   and  isnull(s.Lname,'') = isnull(d.Lname,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '')  then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '') then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
				
				insert into #TempSummary
				select
				'Complete Address',
				count(distinct(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'')) and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end))* 100)/ @totalCount else 0 end,				
				count(distinct(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id	
								
				insert into #TempSummary
				select
				'Address',
				count(distinct(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
				
				insert into #TempSummary
				select
				'Address2',
				count(distinct(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id									
				
				insert into #TempSummary
				select
				'Address3',
				count(distinct(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
			
				insert into #TempSummary
				select
				'City',
				count(distinct(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
			
				insert into #TempSummary
				select
				'State',
				count(distinct(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
				
				insert into #TempSummary
				select
				'Zip',
				count(distinct(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
				
				insert into #TempSummary
				select
				'Country',
				count(distinct(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
				
				insert into #TempSummary
				select
				'Phone',
				count(distinct(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
				
				insert into #TempSummary
				select
				'Job Title',
				count(distinct(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id		
				
				insert into #TempSummary
				select
				'Company Name',
				count(distinct(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end)) * 100) / @totalCount else 0 end,
				count(distinct(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end))* 100)/ @totalCount else 0 end,
				count(distinct(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end)),
				case when @totalCount > 0 then (count(distinct(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end)) * 100) / @totalCount else 0 end
				from DataCompareProfile d with(nolock) 
				join Subscriptions s with (Nolock) on d.IGRP_NO = s.IGrp_No
				join PubSubscriptions ps with (nolock) on s.SubscriptionID = ps.SubscriptionID
				join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  
				where d.ProcessCode = @processCode
				and bd.BrandID = @id
			end
			else if (@MatchType = 'MatchedNotInBrand')
			Begin
				select @totalCount = count(distinct s.igrp_no) 
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
					
				insert into #TempSummary
				select
				'Email',
				count(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') = isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') <> '' and isnull(s.EMAIL,'') != isnull(d.EMAIL,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') <> '' and isnull(d.EMAIL,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.EMAIL,'') = '' and isnull(d.EMAIL,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'First Name & Last Name',
				count(case when isnull(s.Fname ,'') = isnull(d.Fname,'')  and  isnull(s.Lname,'') = isnull(d.Lname,'')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Fname,'') = isnull(d.Fname,'')   and  isnull(s.Lname,'') = isnull(d.Lname,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') <> '' and isnull(s.Fname,'') != isnull(d.Fname,'')) and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') <> '' and isnull(s.Lname,'') != isnull(d.Lname,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '')  then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') <> '' and isnull(d.Fname,'') = '') and (isnull(s.Lname,'') <> '' and isnull(d.Lname,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(s.Fname,'') = '' and isnull(d.Fname,'') <> '') and (isnull(s.Lname,'') = '' and isnull(d.Lname,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Complete Address',
				count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = isnull(d.Company,'') and
						isnull(s.[ADDRESS], '') = isnull(d.[ADDRESS],'') and
						isnull(s.City,'') = isnull(d.City,'') and
						isnull(s.[State],'') = isnull(d.[State],'') and
						isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'')) and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') <> '' and isnull(s.[ADDRESS],'') != isnull(d.[ADDRESS],'')) and
						(isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'')) and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'')) and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'')) then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') <> '' and isnull(d.Company,'') = '') and
						(isnull(s.[ADDRESS],'') <> '' and isnull(d.[ADDRESS],'') = '') and
						(isnull(s.City,'') <> '' and isnull(d.City,'') = '') and
						(isnull(s.[State],'') <> '' and isnull(d.[State],'') = '') and
						(isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '') then s.SubscriptionID end)* 100)/ @totalCount else 0 end,				
				count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when (isnull(s.Company,'') = '' and isnull(d.Company,'') <> '') and
						(isnull(s.[ADDRESS],'') = '' and isnull(d.[ADDRESS],'') <> '') and
						(isnull(s.City,'') = '' and isnull(d.City,'') <> '') and
						(isnull(s.[State],'') = '' and isnull(d.[State],'') <> '') and
						(isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '') then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
								
				insert into #TempSummary
				select
				'Address',
				count(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') = isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') <> '' and isnull(s.[Address],'') != isnull(d.[Address],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') <> '' and isnull(d.[Address],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.[Address],'') = '' and isnull(d.[Address],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Address2',
				count(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') = isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') <> '' and isnull(s.MailStop,'') != isnull(d.MailStop,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') <> '' and isnull(d.MailStop,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.MailStop,'') = '' and isnull(d.MailStop,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Address3',
				count(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') = isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') <> '' and isnull(s.Address3,'') != isnull(d.Address3,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') <> '' and isnull(d.Address3,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Address3,'') = '' and isnull(d.Address3,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
			
				insert into #TempSummary
				select
				'City',
				count(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') = isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') <> '' and isnull(d.City,'') <> '' and isnull(s.City,'') != isnull(d.City,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') <> '' and isnull(d.City,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.City,'') = '' and isnull(d.City,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
			
				insert into #TempSummary
				select
				'State',
				count(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') = isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') <> '' and isnull(s.[State],'') != isnull(d.[State],'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') <> '' and isnull(d.[State],'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.[State],'') = '' and isnull(d.[State],'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Zip',
				count(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') = isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') <> '' and isnull(s.Zip,'') != isnull(d.Zip,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') <> '' and isnull(d.Zip,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Zip,'') = '' and isnull(d.Zip,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Country',
				count(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') = isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') <> '' and isnull(s.CountryID,'') != isnull(d.CountryID,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') <> '' and isnull(d.CountryID,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.CountryID,'') = '' and isnull(d.CountryID,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Phone',
				count(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') = isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') <> '' and isnull(s.Phone,'') != isnull(d.Phone,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') <> '' and isnull(d.Phone,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Phone,'') = '' and isnull(d.Phone,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Job Title',
				count(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') = isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') <> '' and isnull(s.Title,'') != isnull(d.Title,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') <> '' and isnull(d.Title,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Title,'') = '' and isnull(d.Title,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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
				
				insert into #TempSummary
				select
				'Company Name',
				count(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') <> '' and isnull(s.Company,'') != isnull(d.Company,'') then s.SubscriptionID end) * 100) / @totalCount else 0 end,
				count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') <> '' and isnull(d.Company,'') = '' then s.SubscriptionID end)* 100)/ @totalCount else 0 end,
				count(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end),
				case when @totalCount > 0 then (count(case when isnull(s.Company,'') = '' and isnull(d.Company,'') <> '' then d.SubscriberFinalId end) * 100) / @totalCount else 0 end
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

	select * from #TempSummary
	
	DROP TABLE #TempSummary;			
end
