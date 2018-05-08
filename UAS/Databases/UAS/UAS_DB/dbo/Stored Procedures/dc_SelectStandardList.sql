CREATE PROCEDURE dc_SelectStandardList
@dcResultQueId int,
@DemoCodeTypeName varchar(50),
@DcOptionName varchar(50),
@MatchTarget varchar(50)
AS
BEGIN

	set nocount on

	declare @ctDS int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = @DemoCodeTypeName)
	declare @dcOptId int = (select DataCompareOptionId from DataCompareOption with(nolock) where OptionName = @DcOptionName)
	declare @prefix varchar(3) = 'ps.'
	if(@MatchTarget = 'Consensus' or @MatchTarget = 'Brand')
		begin
			set @prefix = 's.'
		end
	
	declare @standardList varchar(max)

	if(@DcOptionName = 'Matching Profiles' or @DcOptionName = 'Like Profiles')
		begin
			select @standardList = COALESCE(@standardList + ', ', '') + '' + @prefix + c.CodeName + ' as ''' + c.CodeName + ''''
			from DataCompareUserMatrix um with(nolock)
				join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
				join UAD_Lookup..Code c with(nolock) on cm.CodeId = c.CodeId 
			where um.DataCompareResultQueId = @dcResultQueId
				and cm.DataCompareOptionId = @dcOptId
				and um.IsActive = 'true'
				and cm.CodeTypeId = @ctDS
				and c.IsActive = 'true'
		end
	else
		begin
			select @standardList = COALESCE(@standardList + ', ', '') + '''' + c.CodeName + ''''
			from DataCompareUserMatrix um with(nolock)
				join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
				join UAD_Lookup..Code c with(nolock) on cm.CodeId = c.CodeId 
			where um.DataCompareResultQueId = @dcResultQueId
				and cm.DataCompareOptionId = @dcOptId
				and um.IsActive = 'true'
				and cm.CodeTypeId = @ctDS
				and c.IsActive = 'true'
		end

	if(@MatchTarget != 'Consensus' and @MatchTarget != 'Brand')
		begin
			set @standardList = REPLACE(@standardList,'Address','Address1')
			set @standardList = REPLACE(@standardList,'MailStop','Address2')
			set @standardList = REPLACE(@standardList,'State','RegionCode')
			set @standardList = REPLACE(@standardList,'Zip','ZipCode')
			set @standardList = REPLACE(@standardList,'ForZip','Plus4')
			set @standardList = REPLace(@standardList,'s.Home_Work_Address','')
			set @standardList = REPLace(@standardList,'ps.Home_Work_Address','')
			set @standardList = REPLACE(@standardList,'IsLatLonValid','IsAddressValidated')
			set @standardList = REPLACE(@standardList,'LatLonMsg','AddressValidationMessage')
			set @standardList = REPLACE(@standardList,'FName','FirstName')
			set @standardList = REPLACE(@standardList,'LName','LastName')
			set @standardList = REPLACE(@standardList,'Verified','Verify')
			set @standardList = REPLACE(@standardList,'TransactionDate','PubTransactionDate')
			set @standardList = REPLACE(@standardList,'QDate','Qualificationdate')
			set @standardList = REPLACE(@standardList,'Source','SubscriberSourceCode')

			set @standardList = REPLACE(@standardList,'s.RegCode','')
			set @standardList = REPLACE(@standardList,'ps.RegCode','')
			set @standardList = REPLACE(@standardList,'s.SubSrc','')
			set @standardList = REPLACE(@standardList,'ps.SubSrc','')
			set @standardList = REPLACE(@standardList,'s.Par3C','')
			set @standardList = REPLACE(@standardList,'ps.Par3C','')
			set @standardList = REPLACE(@standardList,'s.Priority','')
			set @standardList = REPLACE(@standardList,'ps.Priority','')
			set @standardList = REPLACE(@standardList,'s.Sic','')
			set @standardList = REPLACE(@standardList,'ps.Sic','')
			set @standardList = REPLACE(@standardList,'s.SicCode','')
			set @standardList = REPLACE(@standardList,'ps.SicCode','')
			set @standardList = REPLACE(@standardList,'s.IGrp_Rank','')
			set @standardList = REPLACE(@standardList,'ps.IGrp_Rank','')
			set @standardList = REPLACE(@standardList,'s.Score','')
			set @standardList = REPLACE(@standardList,'ps.Score','')
			set @standardList = REPLACE(@standardList,'s.IsMailable','')
			set @standardList = REPLACE(@standardList,'ps.IsMailable','')
			set @standardList = REPLACE(@standardList,'s.Demo31','')
			set @standardList = REPLACE(@standardList,'ps.Demo31','')
			set @standardList = REPLACE(@standardList,'s.Demo32','')
			set @standardList = REPLACE(@standardList,'ps.Demo32','')
			set @standardList = REPLACE(@standardList,'s.Demo33','')
			set @standardList = REPLACE(@standardList,'ps.Demo33','')
			set @standardList = REPLACE(@standardList,'s.Demo34','')
			set @standardList = REPLACE(@standardList,'ps.Demo34','')
			set @standardList = REPLACE(@standardList,'s.Demo35','')
			set @standardList = REPLACE(@standardList,'ps.Demo35','')
			set @standardList = REPLACE(@standardList,'s.Demo36','')
			set @standardList = REPLACE(@standardList,'ps.Demo36','')

			set @standardList = REPLACE(@standardList,'Address13','Address3')
			set @standardList = REPLACE(@standardList,'Plus4Code','Plus4')
		end

	select distinct @standardList

END
go