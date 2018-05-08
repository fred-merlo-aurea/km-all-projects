CREATE PROCEDURE dc_MatchProfileCreateTables
@MatchCriteriaCount int = 0,
@MatchClause nvarchar(max) = '',
@MatchTarget varchar(20),
@TableName varchar(250),
@ProcessCode varchar(50),
@standardList varchar(max) = '',
@premiumList varchar(max) = '',
@ProductId varchar(50) = '0',
@BrandId varchar(50) = '0',
@MarketId varchar(50) = '0'
AS
BEGIN

	set nocount on

	if LEN(@standardList) > 0
		begin
			if @MatchCriteriaCount = 0
				begin
					declare @sql varchar(max)
			
					if(@MatchTarget = 'Product')
						begin
							set @sql = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and ps.PubID = ' + @ProductId )
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sql = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
										  where bd.BrandID = ' + @BrandId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''')
						end
					else if @MatchTarget = 'Market'
						begin
							set @sql = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
										  where m.MarketID = ' + @MarketId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''')
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sql = ('select distinct s.SubscriptionID, ' +  @standardList + 
										' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  where sf.ProcessCode = ''' +  @ProcessCode + '''')
						end
				
				
					exec(@sql)	
				end
			else
				begin		
					declare @sqlClause varchar(max)
			
					if(@MatchTarget = 'Product')
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, ' +  @standardList + 
												' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
												' from Subscriptions s with(nolock)
												  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
												  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
												  left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
												  left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
												  where sf.ProcessCode = ''' +  @ProcessCode + '''
												  and ps.PubID = ' + @ProductId + ' and ' + @MatchClause)
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, ' +  @standardList + 
												' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
												' from Subscriptions s with(nolock)
												  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
												  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
												  left join SubscriberMasterValues smv with(nolock) on s.SubscriptionID = smv.SubscriptionID
												  left join MasterGroups mg with(nolock) on smv.MasterGroupID = mg.MasterGroupID   
												  left join Mastercodesheet mcs with(nolock) on mg.MasterGroupID = mcs.MasterGroupID
												  where bd.BrandID = ' + @BrandId + '
												  and sf.ProcessCode = ''' +  @ProcessCode + '''
												  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Market'
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, ' +  @standardList + 
												' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
												' from Subscriptions s with(nolock)
												  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
												  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
												  join Markets m with(nolock) on bpm.BrandID = m.BrandID
												  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
												  left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
												  left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
												  where m.MarketID = ' + @MarketId + '
												  and sf.ProcessCode = ''' +  @ProcessCode + '''
												  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, ' +  @standardList + 
												' into UAD_TEMP.dbo.tmpStdProf_' + @TableName + 
												' from Subscriptions s with(nolock)
												  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
												  left join SubscriberMasterValues smv with(nolock) on s.SubscriptionID = smv.SubscriptionID
												  left join MasterGroups mg with(nolock) on smv.MasterGroupID = mg.MasterGroupID   
												  left join Mastercodesheet mcs with(nolock) on mg.MasterGroupID = mcs.MasterGroupID
												  where sf.ProcessCode = ''' +  @ProcessCode + '''
												  and ' + @MatchClause)
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
			if @MatchCriteriaCount = 0
			begin
				declare @sqlPrem varchar(max)
			
				if(@MatchTarget = 'Product')
					begin
						set @sqlPrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
									' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
									' from Subscriptions s with(nolock)
									  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
									  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
									  where sf.ProcessCode = ''' +  @ProcessCode + '''
									  and ps.PubID = ' + @ProductId )
					end
				else if @MatchTarget = 'Brand'
					begin
						set @sqlPrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
									' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
									' from Subscriptions s with(nolock)
									  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
									  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
									  where bd.BrandID = ' + @BrandId + '
									  and sf.ProcessCode = ''' +  @ProcessCode + '''')
					end
				else if @MatchTarget = 'Market'
					begin
						set @sqlPrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
									' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
									' from Subscriptions s with(nolock)
									  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
									  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
									  join Markets m with(nolock) on bpm.BrandID = m.BrandID
									  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
									  where m.MarketID = ' + @MarketId + '
									  and sf.ProcessCode = ''' +  @ProcessCode + '''')
					end
				else if @MatchTarget = 'Consensus'
					begin
						set @sqlPrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
									' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
									' from Subscriptions s with(nolock)
									  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
									  where sf.ProcessCode = ''' +  @ProcessCode + '''')
					end
				
				
				exec(@sqlPrem)	
			end
		else
			begin		
				declare @sqlClausePrem varchar(max)
			
				if(@MatchTarget = 'Product')
					begin
						set @sqlClausePrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
											' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
											' from Subscriptions s with(nolock)
											  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
											  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
											  left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
											  left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
											  where sf.ProcessCode = ''' +  @ProcessCode + '''
											  and ps.PubID = ' + @ProductId + ' and ' + @MatchClause)
					end
				else if @MatchTarget = 'Brand'
					begin
						set @sqlClausePrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
											' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
											' from Subscriptions s with(nolock)
											  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
											  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
											  left join SubscriberMasterValues smv with(nolock) on s.SubscriptionID = smv.SubscriptionID
											  left join MasterGroups mg with(nolock) on smv.MasterGroupID = mg.MasterGroupID   
											  left join Mastercodesheet mcs with(nolock) on mg.MasterGroupID = mcs.MasterGroupID
											  where bd.BrandID = ' + @BrandId + '
											  and sf.ProcessCode = ''' +  @ProcessCode + '''
											  and ' + @MatchClause)
					end
				else if @MatchTarget = 'Market'
					begin
						set @sqlClausePrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
											' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
											' from Subscriptions s with(nolock)
											  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
											  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
											  join Markets m with(nolock) on bpm.BrandID = m.BrandID
											  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No
											  left join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
											  left join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
											  where m.MarketID = ' + @MarketId + '
											  and sf.ProcessCode = ''' +  @ProcessCode + '''
											  and ' + @MatchClause)
					end
				else if @MatchTarget = 'Consensus'
					begin
						set @sqlClausePrem = ('select distinct s.SubscriptionID, ' +  @premiumList + 
											' into UAD_TEMP.dbo.tmpPremProf_' + @TableName + 
											' from Subscriptions s with(nolock)
											  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
											  left join SubscriberMasterValues smv with(nolock) on s.SubscriptionID = smv.SubscriptionID
											  left join MasterGroups mg with(nolock) on smv.MasterGroupID = mg.MasterGroupID   
											  left join Mastercodesheet mcs with(nolock) on mg.MasterGroupID = mcs.MasterGroupID
											  where sf.ProcessCode = ''' +  @ProcessCode + '''
											  and ' + @MatchClause)
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