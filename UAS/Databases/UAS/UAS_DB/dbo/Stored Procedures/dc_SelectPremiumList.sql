CREATE PROCEDURE dc_SelectPremiumList
@dcResultQueId int,
@DemoCodeTypeName varchar(50),
@DcOptionName varchar(50),
@MatchTarget varchar(50)
AS
BEGIN

	set nocount on

	declare @dcOptId int = (select DataCompareOptionId from DataCompareOption with(nolock) where OptionName = @DcOptionName)
	declare @ctDP int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = @DemoCodeTypeName)
	declare @prefix varchar(3) = 'ps.'
	if(@MatchTarget = 'Consensus' or @MatchTarget = 'Brand')
		begin
			set @prefix = 's.'
		end

	declare @premiumList varchar(max)

	if(@DcOptionName = 'Matching Profiles' or @DcOptionName = 'Like Profiles')
		begin
			
			select @premiumList = COALESCE(@premiumList + ', ', '') + '' +  @prefix + c.CodeName + ' as ''' + c.CodeName + ''''
			from DataCompareUserMatrix um with(nolock)
			join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
			join UAD_Lookup..Code c with(nolock) on cm.CodeId = c.CodeId 
			where um.DataCompareResultQueId = @dcResultQueId
			and cm.DataCompareOptionId = @dcOptId
			and um.IsActive = 'true'
			and cm.CodeTypeId = @ctDP
			and c.IsActive = 'true'
		end
	else
		begin
			select @premiumList = COALESCE(@premiumList + ', ', '') + '''' +  c.CodeName + ''''
			from DataCompareUserMatrix um with(nolock)
			join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
			join UAD_Lookup..Code c with(nolock) on cm.CodeId = c.CodeId 
			where um.DataCompareResultQueId = @dcResultQueId
			and cm.DataCompareOptionId = @dcOptId
			and um.IsActive = 'true'
			and cm.CodeTypeId = @ctDP
			and c.IsActive = 'true'
		end

	if(@MatchTarget != 'Consensus' and @MatchTarget != 'Brand')
		begin
			set @premiumList = REPLACE(@premiumList,'Address','Address1')
			set @premiumList = REPLACE(@premiumList,'MailStop','Address2')
			set @premiumList = REPLACE(@premiumList,'State','RegionCode')
			set @premiumList = REPLACE(@premiumList,'Zip','ZipCode')
			set @premiumList = REPLACE(@premiumList,'ForZip','Plus4')
			set @premiumList = REPLace(@premiumList,'s.Home_Work_Address','')
			set @premiumList = REPLace(@premiumList,'ps.Home_Work_Address','')
			set @premiumList = REPLACE(@premiumList,'IsLatLonValid','IsAddressValidated')
			set @premiumList = REPLACE(@premiumList,'LatLonMsg','AddressValidationMessage')
			set @premiumList = REPLACE(@premiumList,'FName','FirstName')
			set @premiumList = REPLACE(@premiumList,'LName','LastName')
			set @premiumList = REPLACE(@premiumList,'Verified','Verify')

			set @premiumList = REPLACE(@premiumList,'TransactionDate','PubTransactionDate')
			set @premiumList = REPLACE(@premiumList,'QDate','Qualificationdate')
			set @premiumList = REPLACE(@premiumList,'s.RegCode','')
			set @premiumList = REPLACE(@premiumList,'ps.RegCode','')
			set @premiumList = REPLACE(@premiumList,'s.SubSrc','')
			set @premiumList = REPLACE(@premiumList,'ps.SubSrc','')
			set @premiumList = REPLACE(@premiumList,'s.Par3C','')
			set @premiumList = REPLACE(@premiumList,'ps.Par3C','')
			set @premiumList = REPLACE(@premiumList,'s.Priority','')
			set @premiumList = REPLACE(@premiumList,'ps.Priority','')
			set @premiumList = REPLACE(@premiumList,'s.Sic','')
			set @premiumList = REPLACE(@premiumList,'ps.Sic','')
			set @premiumList = REPLACE(@premiumList,'s.SicCode','')
			set @premiumList = REPLACE(@premiumList,'ps.SicCode','')
			set @premiumList = REPLACE(@premiumList,'s.IGrp_Rank','')
			set @premiumList = REPLACE(@premiumList,'ps.IGrp_Rank','')
			set @premiumList = REPLACE(@premiumList,'s.Score','')
			set @premiumList = REPLACE(@premiumList,'ps.Score','')
			set @premiumList = REPLACE(@premiumList,'s.IsMailable','')
			set @premiumList = REPLACE(@premiumList,'ps.IsMailable','')
			set @premiumList = REPLACE(@premiumList,'s.Demo31','')
			set @premiumList = REPLACE(@premiumList,'ps.Demo31','')
			set @premiumList = REPLACE(@premiumList,'s.Demo32','')
			set @premiumList = REPLACE(@premiumList,'ps.Demo32','')
			set @premiumList = REPLACE(@premiumList,'s.Demo33','')
			set @premiumList = REPLACE(@premiumList,'ps.Demo33','')
			set @premiumList = REPLACE(@premiumList,'s.Demo34','')
			set @premiumList = REPLACE(@premiumList,'ps.Demo34','')
			set @premiumList = REPLACE(@premiumList,'s.Demo35','')
			set @premiumList = REPLACE(@premiumList,'ps.Demo35','')
			set @premiumList = REPLACE(@premiumList,'s.Demo36','')
			set @premiumList = REPLACE(@premiumList,'ps.Demo36','')

			set @premiumList = REPLACE(@premiumList,'Address13','Address3')
			set @premiumList = REPLACE(@premiumList,'Plus4Code','Plus4')
		end

	select distinct @premiumList

END
go