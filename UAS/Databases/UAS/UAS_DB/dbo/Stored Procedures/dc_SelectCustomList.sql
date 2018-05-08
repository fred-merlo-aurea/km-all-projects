CREATE PROCEDURE dc_SelectCustomList
@dcResultQueId int,
@DemoCodeTypeName varchar(50),
@DcOptionName varchar(50),
@MatchTarget varchar(50)
AS
BEGIN

	set nocount on

	declare @dcOptId int = (select DataCompareOptionId from DataCompareOption with(nolock) where OptionName = @DcOptionName)
	declare @ctDC int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = @DemoCodeTypeName)
	declare @prefix varchar(3) = 'ps.'
	if(@MatchTarget = 'Consensus' or @MatchTarget = 'Brand')
		begin
			set @prefix = 's.'
		end

	declare @customList varchar(max)

	if(@DcOptionName = 'Matching Profiles' or @DcOptionName = 'Like Profiles')
		begin
			select @customList = COALESCE(@customList + ', ', '') + '' +  @prefix + c.CodeName + ' as ''' + c.CodeName + ''''
			from DataCompareUserMatrix um with(nolock)
			join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
			join UAD_Lookup..Code c with(nolock) on cm.CodeId = c.CodeId 
			where um.DataCompareResultQueId = @dcResultQueId
			and cm.DataCompareOptionId = @dcOptId
			and um.IsActive = 'true'
			and cm.CodeTypeId = @ctDC
			and c.IsActive = 'true'
		end
	else
		begin
			select @customList = COALESCE(@customList + ', ', '') + '''' +  c.CodeName + ''''
			from DataCompareUserMatrix um with(nolock)
			join DataCompareOptionCodeMap cm with(nolock) on um.DataCompareOptionCodeMapId = cm.DataCompareOptionCodeMapId
			join UAD_Lookup..Code c with(nolock) on cm.CodeId = c.CodeId 
			where um.DataCompareResultQueId = @dcResultQueId
			and cm.DataCompareOptionId = @dcOptId
			and um.IsActive = 'true'
			and cm.CodeTypeId = @ctDC
			and c.IsActive = 'true'
		end

	if(@MatchTarget != 'Consensus' and @MatchTarget != 'Brand')
		begin
			set @customList = REPLACE(@customList,'Address','Address1')
			set @customList = REPLACE(@customList,'MailStop','Address2')
			set @customList = REPLACE(@customList,'State','RegionCode')
			set @customList = REPLACE(@customList,'Zip','ZipCode')
			set @customList = REPLACE(@customList,'ForZip','Plus4')
			set @customList = REPLace(@customList,'s.Home_Work_Address','')
			set @customList = REPLace(@customList,'ps.Home_Work_Address','')
			set @customList = REPLACE(@customList,'IsLatLonValid','IsAddressValidated')
			set @customList = REPLACE(@customList,'LatLonMsg','AddressValidationMessage')
			set @customList = REPLACE(@customList,'FName','FirstName')
			set @customList = REPLACE(@customList,'LName','LastName')
			set @customList = REPLACE(@customList,'Verified','Verify')

			set @customList = REPLACE(@customList,'TransactionDate','PubTransactionDate')
			set @customList = REPLACE(@customList,'QDate','Qualificationdate')
			set @customList = REPLACE(@customList,'s.RegCode','')
			set @customList = REPLACE(@customList,'ps.RegCode','')
			set @customList = REPLACE(@customList,'s.SubSrc','')
			set @customList = REPLACE(@customList,'ps.SubSrc','')
			set @customList = REPLACE(@customList,'s.Par3C','')
			set @customList = REPLACE(@customList,'ps.Par3C','')
			set @customList = REPLACE(@customList,'s.Priority','')
			set @customList = REPLACE(@customList,'ps.Priority','')
			set @customList = REPLACE(@customList,'s.Sic','')
			set @customList = REPLACE(@customList,'ps.Sic','')
			set @customList = REPLACE(@customList,'s.SicCode','')
			set @customList = REPLACE(@customList,'ps.SicCode','')
			set @customList = REPLACE(@customList,'s.IGrp_Rank','')
			set @customList = REPLACE(@customList,'ps.IGrp_Rank','')
			set @customList = REPLACE(@customList,'s.Score','')
			set @customList = REPLACE(@customList,'ps.Score','')
			set @customList = REPLACE(@customList,'s.IsMailable','')
			set @customList = REPLACE(@customList,'ps.IsMailable','')
			set @customList = REPLACE(@customList,'s.Demo31','')
			set @customList = REPLACE(@customList,'ps.Demo31','')
			set @customList = REPLACE(@customList,'s.Demo32','')
			set @customList = REPLACE(@customList,'ps.Demo32','')
			set @customList = REPLACE(@customList,'s.Demo33','')
			set @customList = REPLACE(@customList,'ps.Demo33','')
			set @customList = REPLACE(@customList,'s.Demo34','')
			set @customList = REPLACE(@customList,'ps.Demo34','')
			set @customList = REPLACE(@customList,'s.Demo35','')
			set @customList = REPLACE(@customList,'ps.Demo35','')
			set @customList = REPLACE(@customList,'s.Demo36','')
			set @customList = REPLACE(@customList,'ps.Demo36','')

			set @customList = REPLACE(@customList,'Address13','Address3')
			set @customList = REPLACE(@customList,'Plus4Code','Plus4')
		end

	select distinct @customList

END
go