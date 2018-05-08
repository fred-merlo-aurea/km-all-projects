create procedure ccp_BriefMedia_PostDQM
@SourceFileId int = 0,
@ProcessCode varchar(50) = '',
@ClientId int = 0
as
BEGIN

	set nocount on

	--3. Apply Opt-Outs from outside list.  

	-- a. Set Demo31 = 0 where pubcode = ‘CB’ and Customer_ID = CustomerID from CB DNP Postal_05_16_2014.csv 
	declare @cbPostal table (SFRecordIdentifier uniqueidentifier)
	insert into @cbPostal (SFRecordIdentifier)
	select sdf.SFRecordIdentifier
	from SubscriberFinal sf with(nolock)
	join SubscriberDemographicFinal sdf with(nolock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'CB'
	and sdf.MAFField = 'Customer_ID'
	and sdf.Value in (select MatchValue
					  from uas..AdHocDimension ad
					  join uas..AdHocDimensionGroup adg on ad.AdHocDimensionGroupId = adg.AdHocDimensionGroupId
					  where adg.AdHocDimensionGroupName = 'BriefMedia_CbDnpPostal'
					  and ad.IsActive = 'true')

	update sf
	set sf.MailPermission = 0
	from SubscriberFinal sf
	join SubscriberDemographicFinal sdf on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	join @cbPostal p on sf.SFRecordIdentifier = p.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'CB'
	
	-- b. Set Demo34 = 0  where pubcode = ‘CB’ and Customer_ID = CustomerID from CB DNP Email_05_16_2014.csv 
	declare @cbEmail table (SFRecordIdentifier uniqueidentifier)
	insert into @cbEmail (SFRecordIdentifier)
	select sdf.SFRecordIdentifier
	from SubscriberFinal sf with(nolock)
	join SubscriberDemographicFinal sdf with(nolock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'CB'
	and sdf.MAFField = 'Customer_ID'
	and sdf.Value in (select MatchValue
					  from uas..AdHocDimension ad
					  join uas..AdHocDimensionGroup adg on ad.AdHocDimensionGroupId = adg.AdHocDimensionGroupId
					  where adg.AdHocDimensionGroupName = 'BriefMedia_CbDnpEmail'
					  and ad.IsActive = 'true')

	update sf
	set sf.OtherProductsPermission = 0
	from SubscriberFinal sf
	join SubscriberDemographicFinal sdf on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	join @cbEmail p on sf.SFRecordIdentifier = p.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'CB'
	
	-- c. Set Demo33 = 0 where pubcode = ‘ CB’ and Customer_ID = CustomerID from CB DNP Phone_05_16_2014.csv 
	declare @cbPhone table (SFRecordIdentifier uniqueidentifier)
	insert into @cbPhone (SFRecordIdentifier)
	select sdf.SFRecordIdentifier
	from SubscriberFinal sf with(nolock)
	join SubscriberDemographicFinal sdf with(nolock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'CB'
	and sdf.MAFField = 'Customer_ID'
	and sdf.Value in (select MatchValue
					  from uas..AdHocDimension ad
					  join uas..AdHocDimensionGroup adg on ad.AdHocDimensionGroupId = adg.AdHocDimensionGroupId
					  where adg.AdHocDimensionGroupName = 'BriefMedia_CbDnpPhone'
					  and ad.IsActive = 'true')

	update sf
	set sf.PhonePermission = 0
	from SubscriberFinal sf
	join SubscriberDemographicFinal sdf on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	join @cbPhone p on sf.SFRecordIdentifier = p.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'CB'
	
	-- d. Set Demo33 = 0 where pubcode = ‘VTB’ and Customer_ID =CustomerID from VTB DNP Phone_05_16_2014.csv 
	declare @vtbPhone table (SFRecordIdentifier uniqueidentifier)
	insert into @vtbPhone (SFRecordIdentifier)
	select sdf.SFRecordIdentifier
	from SubscriberFinal sf with(nolock)
	join SubscriberDemographicFinal sdf with(nolock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'VTB'
	and sdf.MAFField = 'Customer_ID'
	and sdf.Value in (select MatchValue
					  from uas..AdHocDimension ad
					  join uas..AdHocDimensionGroup adg on ad.AdHocDimensionGroupId = adg.AdHocDimensionGroupId
					  where adg.AdHocDimensionGroupName = 'BriefMedia_VtbDnpPhone'
					  and ad.IsActive = 'true')

	update sf
	set sf.PhonePermission = 0
	from SubscriberFinal sf
	join SubscriberDemographicFinal sdf on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	join @vtbPhone p on sf.SFRecordIdentifier = p.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'VTB'
	
	-- e. Set Demo34 = 0 where pubcode = ‘VTB’ and Customer_ID = CustomerID from VTB DNP Email_05_16_2014.csv 
	declare @vtbEmail table (SFRecordIdentifier uniqueidentifier)
	insert into @vtbEmail (SFRecordIdentifier)
	select sdf.SFRecordIdentifier
	from SubscriberFinal sf with(nolock)
	join SubscriberDemographicFinal sdf with(nolock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'VTB'
	and sdf.MAFField = 'Customer_ID'
	and sdf.Value in (select MatchValue
					  from uas..AdHocDimension ad
					  join uas..AdHocDimensionGroup adg on ad.AdHocDimensionGroupId = adg.AdHocDimensionGroupId
					  where adg.AdHocDimensionGroupName = 'BriefMedia_VtbDnpEmail'
					  and ad.IsActive = 'true')

	update sf
	set sf.OtherProductsPermission = 0
	from SubscriberFinal sf
	join SubscriberDemographicFinal sdf on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	join @vtbEmail p on sf.SFRecordIdentifier = p.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode = 'VTB'
	
	--4. Set demo35 = demo34 (apply after step 3 is applied)
	update sf
	set sf.ThirdPartyPermission = sf.OtherProductsPermission
	from SubscriberFinal sf
	where sf.ProcessCode = @ProcessCode
	and sf.PubCode in ('CB','VTB')

END
go