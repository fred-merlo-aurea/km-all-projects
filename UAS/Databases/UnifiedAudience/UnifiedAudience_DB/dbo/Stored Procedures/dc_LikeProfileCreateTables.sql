CREATE PROCEDURE dc_LikeProfileCreateTables
@LikeCriteriaCount int = 0,
@LikeClause nvarchar(max) = '',
@Target varchar(20),
@TableName varchar(250),
@ProcessCode varchar(50),
@standardList varchar(max) = '',
@premiumList varchar(max) = '',
@ProductId varchar(50) = '0',
@BrandId varchar(50) = '0',
@MarketId varchar(50) = '0'
as
BEGIN

	set nocount on

	if LEN(@standardList) > 0
		begin
			if @LikeCriteriaCount = 0
				begin
					declare @sql varchar(max)
			
					if(@Target = 'Product')
						begin
							set @sql = (' select distinct s.SubscriptionID, ' +  @standardList +
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  where ps.PubID = ' + @ProductId + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Brand'
						begin
							set @sql = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  where bd.BrandID = ' + @BrandId + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Market'
						begin
							set @sql = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  where m.MarketID = ' + @MarketId + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Consensus'
						begin
							set @sql = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  where s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
				
				
					exec(@sql)	
				end
			else
				begin		
					declare @sqlClause varchar(max)
			
					if(@Target = 'Product')
						begin
							set @sqlClause = (' select distinct s.SubscriptionID, ' +  @standardList +
											  ' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
											  ' from Subscriptions s with(nolock)
												join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
												left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
												left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
												where ps.PubID = ' + @ProductId + '
												and ' + @LikeClause + '
												and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
												)
						end
					else if @Target = 'Brand'
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join SubscriberMasterValues smv with(nolock) on s.SubscriptionID = smv.SubscriptionID
										  left join MasterGroups mg with(nolock) on smv.MasterGroupID = mg.MasterGroupID   
										  left join Mastercodesheet mcs with(nolock) on mg.MasterGroupID = mcs.MasterGroupID
										  where bd.BrandID = ' + @BrandId + '
										  and ' + @LikeClause + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Market'
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and ' + @LikeClause + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Consensus'
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberMasterValues smv with(nolock) on s.SubscriptionID = smv.SubscriptionID
										  left join MasterGroups mg with(nolock) on smv.MasterGroupID = mg.MasterGroupID   
										  left join Mastercodesheet mcs with(nolock) on mg.MasterGroupID = mcs.MasterGroupID
										  where ' + @LikeClause + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end

					exec(@sqlClause)	
				end
		end

	IF (NOT EXISTS (SELECT * 
                 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'tmpStdProf_' + @TableName))
		BEGIN
			declare @sqlStdTableClause varchar(max) 
			set @sqlStdTableClause = ' select 0 as SubscriptionID
								  into UAD_TEMP.dbo.tmpStdProf_' + @TableName
			
			exec(@sqlStdTableClause)
		END

	if LEN(@premiumList) > 0
		begin
			if @LikeCriteriaCount = 0
				begin
					declare @sqlPrem varchar(max)
			
					if(@Target = 'Product')
						begin
							set @sqlPrem = (' select distinct s.SubscriptionID, ' +  @premiumList +
										' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  where ps.PubID = ' + @ProductId + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Brand'
						begin
							set @sqlPrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
										' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandDetails bd with(nolock) on ps.PubID = bd.PubID 
										  where bd.BrandID = ' + @BrandId + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Market'
						begin
							set @sqlPrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
										' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  where m.MarketID = ' + @MarketId + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Consensus'
						begin
							set @sqlPrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
										' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  where s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
				
				
					exec(@sqlPrem)	
				end
			else
				begin		
					declare @sqlClausePrem varchar(max)
			
					if(@Target = 'Product')
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, ' +  @premiumList +
											  ' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
											  ' from Subscriptions s with(nolock)
												join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
												left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
												left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
												where ps.PubID = ' + @ProductId + '
												and ' + @LikeClause + '
												and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
												)
						end
					else if @Target = 'Brand'
						begin
							set @sqlClausePrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
										' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandDetails bd with(nolock) on ps.PubID = bd.PubID 
										  left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID 
										  where bd.BrandID = ' + @BrandId + '
										  and ' + @LikeClause + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Market'
						begin
							set @sqlClausePrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
										' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and ' + @LikeClause + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end
					else if @Target = 'Consensus'
						begin
							set @sqlClausePrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
										' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberMasterValues smv with(nolock) on s.SubscriptionID = smv.SubscriptionID
										  left join MasterGroups mg with(nolock) on smv.MasterGroupID = mg.MasterGroupID   
										  left join Mastercodesheet mcs with(nolock) on mg.MasterGroupID = mcs.MasterGroupID
										  where ' + @LikeClause + '
										  and s.IGRP_NO not in (select IGrp_No from SubscriberFinal with(nolock) where ProcessCode = ''' +  @ProcessCode + ''')'
										)
						end

					exec(@sqlClausePrem)	
				end
		end

	IF (NOT EXISTS (SELECT * 
                 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'tmpPremProf_' + @TableName))
		BEGIN
			declare @sqlPremTableClause varchar(max) 
			set @sqlPremTableClause = ' select 0 as SubscriptionID
								  into UAD_TEMP.dbo.tmpPremProf_' + @TableName
			
			exec(@sqlPremTableClause)
		END

END
go