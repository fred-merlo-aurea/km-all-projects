create procedure e_Code_SelectForDemographicAttribute
@CodeTypeName varchar(50),
@DataCompareResultQueId int,
@FtpFolder varchar(50)
as
BEGIN

	set nocount on

	Declare @DemographicStandardAttributes int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Standard Attributes')
	Declare @DemographicPremiumAttributes int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Premium Attributes')
	Declare @DemographicCustomAttributes int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Demographic Custom Attributes')
	declare @CodeTypeId int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = @CodeTypeName)
	
	--declare @ClientId int = (Select ClientId from UAS..DataCompareResultQue with(nolock) where DataCompareResultQueId = @DataCompareResultQueId)
	--declare @clientName varchar(50) = (select ClientName from UAS..Client with(nolock) where ClientID=@ClientId)
	declare @db varchar(50) = (select name from master..sysdatabases where name like @FtpFolder + 'MasterDB%') 
	declare @isConsensus bit = (Select IsConsensus from UAS..DataCompareResultQue with(nolock) where DataCompareResultQueId = @DataCompareResultQueId)
	declare @Scope varchar(50) = (select case when ProductId is not null then 'Product'
											  when BrandId is not null then 'Brand' 
											  when MarketId is not null then 'Market' else 'Consensus' end
								  from UAS..DataCompareResultQue with(nolock) where DataCompareResultQueId = @DataCompareResultQueId)
	declare @ScopeId int = (select case when ProductId is not null then ProductId
											  when BrandId is not null then BrandId
											  when MarketId is not null then MarketId else 0 end
								  from UAS..DataCompareResultQue with(nolock) where DataCompareResultQueId = @DataCompareResultQueId)
								  
	create table #tmpDemo ( DemoName varchar(50) )
	
	declare @Sqlstmt varchar(max)
								  
	If @isConsensus = 'false' and @Scope = 'Product'
		begin
			set @Sqlstmt = 'declare @DemographicPremiumAttributesId int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = ''Demographic Premium Attributes'')
						
						declare @ScopeId int = (select ProductId from UAS..DataCompareResultQue with(nolock) where DataCompareResultQueId =  ' + CAST(@DataCompareResultQueId as varchar(10)) + ')
								  
						insert into #tmpDemo
						select Distinct DisplayName
						from '+ @db +'..ResponseGroups rg with(nolock)
						where rg.PubID = @ScopeId'
			exec(@Sqlstmt)
		end
	else if @isConsensus = 'false' and @Scope = 'Brand'
		begin
			set @Sqlstmt = 'declare @DemographicPremiumAttributesId int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = ''Demographic Premium Attributes'')
						
						declare @ScopeId int = (select BrandId from UAS..DataCompareResultQue with(nolock) where DataCompareResultQueId = ' + CAST(@DataCompareResultQueId as varchar(10)) + ')
						
						insert into #tmpDemo
						select distinct mg.Name 
						from '+ @db +'..vw_BrandConsensus b with(nolock)
						join '+ @db +'..Mastercodesheet mcs with(nolock) on b.MasterID = mcs.MasterID
						join '+ @db +'..MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
						where b.BrandID = @ScopeId'
			exec(@Sqlstmt)			
		end
	else if @isConsensus = 'false' and @Scope = 'Market'
		begin
			set @Sqlstmt = 'declare @DemographicPremiumAttributesId int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = ''Demographic Premium Attributes'')
						
						 declare @ScopeId int = (select BrandId from UAS..DataCompareResultQue with(nolock) where DataCompareResultQueId = ' + CAST(@DataCompareResultQueId as varchar(10)) + ')
						 
						 insert into #tmpDemo
						 select distinct rg.DisplayName 
						 from '+ @db +'..Markets m with(nolock)
						 join '+ @db +'..Brand b with(nolock) on m.BrandID = b.BrandID
						 join '+ @db +'..BrandDetails bd with(nolock) on b.BrandID = bd.BrandID
						 join '+ @db +'..ResponseGroups rg with(nolock) on bd.PubID = rg.PubID
						 where m.MarketID = @ScopeId'
			exec(@Sqlstmt)
		end
	else if @isConsensus = 'true' and @Scope = 'Consensus'
		begin
			set @Sqlstmt = 'declare @DemographicPremiumAttributesId int = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = ''Demographic Premium Attributes'')
						
						insert into #tmpDemo
						--Consensus
						select Distinct DisplayName
						from '+ @db +'..MasterGroups mg with(nolock)'
			exec(@Sqlstmt)
		end
		
	select distinct c.*
	from UAS..DataCompareOptionCodeMap ocm with(nolock)
	join UAD_Lookup..Code c with(nolock) on ocm.CodeTypeId = c.CodeTypeId and ocm.CodeId = c.CodeId
	join #tmpDemo d with(nolock) on c.CodeName = d.DemoName and c.CodeValue = d.DemoName
	left join UAS..DataCompareUserMatrix um with(nolock) on um.DataCompareResultQueId = @DataCompareResultQueId and um.DataCompareOptionCodeMapId = ocm.DataCompareOptionCodeMapId
	where ocm.CodeTypeId in (@DemographicStandardAttributes,@DemographicPremiumAttributes,@DemographicCustomAttributes)
	and c.CodeTypeId = @CodeTypeId
	and c.IsActive = 'true'
	--and um.DataCompareOptionCodeMapId is null
	order by c.CodeName

	drop table #tmpDemo

END
go