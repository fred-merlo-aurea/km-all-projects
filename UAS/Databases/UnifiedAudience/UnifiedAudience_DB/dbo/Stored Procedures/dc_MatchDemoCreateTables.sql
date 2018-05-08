CREATE PROCEDURE dc_MatchDemoCreateTables
@MatchCriteriaCount int = 0,
@MatchClause nvarchar(max) = '',
@MatchTarget varchar(20),
@TableName varchar(250),
@ProcessCode varchar(50),
@standardList varchar(max) = '',
@premiumList varchar(max) = '',
@customList varchar(max) = '',
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
							set @sql = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and ps.PubID = ' + @ProductId + ' and cs.ResponseGroup in (' +  @standardList + ')')
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sql = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
										  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join Mastercodesheet mcs with(nolock) on bd.MasterID = mcs.MasterID
										  join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
										  where bd.BrandID = ' + @BrandId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and mg.Name in (' +  @standardList + ')')
						end
					else if @MatchTarget = 'Market'
						begin
							set @sql = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and cs.ResponseGroup in (' +  @standardList + ')')
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sql = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
										  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join SubscriptionDetails sd with(nolock) on s.SubscriptionID = sd.SubscriptionID 
										  join Mastercodesheet mcs with(nolock) on sd.MasterID = mcs.MasterID
										  join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and mg.Name in (' +  @standardList + ')')
						end
	
					exec(@sql)	
					print '@sql: ' + @sql
				end
			else
				begin
					declare @sqlClause varchar(max) 

					if(@MatchTarget = 'Product')
						begin
							set @sqlClause = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsevalue as Value
										  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and ps.PubID = ' + @ProductId + ' and cs.ResponseGroup in (' +  @standardList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sqlClause = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
										  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join Mastercodesheet mcs with(nolock) on bd.MasterID = mcs.MasterID
										  join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
										  where bd.BrandID = ' + @BrandId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and mg.Name in (' +  @standardList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Market'
						begin
							set @sqlClause = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsevalue as Value
										  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and cs.ResponseGroup in (' +  @standardList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sqlClause = ('select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
											   into UAD_TEMP.dbo.tmpStdDemo_' + @TableName + 
											 ' from Subscriptions s with(nolock)
											   join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
											   join SubscriptionDetails sd with(nolock) on s.SubscriptionID = sd.SubscriptionID 
											   join Mastercodesheet mcs with(nolock) on sd.MasterID = mcs.MasterID
											   join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
											   where sf.ProcessCode = ''' +  @ProcessCode + '''
											   and mg.Name in (' +  @standardList + ')
											   and ' + @MatchClause)
						end
				
					exec(@sqlClause)	
					print '@sqlClause: ' + @sqlClause
				end
		end

	IF (NOT EXISTS (SELECT * 
                 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'tmpStdDemo_' + @TableName))
		BEGIN
			declare @sqlStdDemoTableClause varchar(max) 
			set @sqlStdDemoTableClause = ' select 0 as SubscriptionID, '''' as Demographic, '''' as Value
								  into UAD_TEMP.dbo.tmpStdDemo_' + @TableName
			
			exec(@sqlStdDemoTableClause)
		END

	if LEN(@premiumList) > 0
		begin
			if @MatchCriteriaCount = 0
				begin		
					declare @sqlPrem varchar(max)
			
					if(@MatchTarget = 'Product')
						begin
							set @sqlPrem = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and ps.PubID = ' + @ProductId + ' and cs.ResponseGroup in (' +  @premiumList + ')')
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sqlPrem = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
										  into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join Mastercodesheet mcs with(nolock) on bd.MasterID = mcs.MasterID
										  join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
										  where bd.BrandID = ' + @BrandId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and mg.Name in (' +  @premiumList + ')')
						end
					else if @MatchTarget = 'Market'
						begin
							set @sqlPrem = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and cs.ResponseGroup in (' +  @premiumList + ')')
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sqlPrem = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
											   into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
											 ' from Subscriptions s with(nolock)
											   join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
											   join SubscriptionDetails sd with(nolock) on s.SubscriptionID = sd.SubscriptionID 
											   join Mastercodesheet mcs with(nolock) on sd.MasterID = mcs.MasterID
											   join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
											   where sf.ProcessCode = ''' +  @ProcessCode + '''
											   and mg.Name in (' +  @premiumList + ')')
						end
				
					exec(@sqlPrem)	
					print '@sqlPrem: ' + @sqlPrem
				end
			else
				begin
					declare @sqlClausePrem varchar(max) 

					if(@MatchTarget = 'Product')
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and ps.PubID = ' + @ProductId + ' and cs.ResponseGroup in (' +  @premiumList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
										  into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join Mastercodesheet mcs with(nolock) on bd.MasterID = mcs.MasterID
										  join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
										  where bd.BrandID = ' + @BrandId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and mg.Name in (' +  @premiumList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Market'
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and cs.ResponseGroup in (' +  @premiumList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
											   into UAD_TEMP.dbo.tmpPremDemo_' + @TableName + 
											 ' from Subscriptions s with(nolock)
											   join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
											   join SubscriptionDetails sd with(nolock) on s.SubscriptionID = sd.SubscriptionID 
											   join Mastercodesheet mcs with(nolock) on sd.MasterID = mcs.MasterID
											   join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
											   where sf.ProcessCode = ''' +  @ProcessCode + '''
											   and mg.Name in (' +  @premiumList + ')
											   and ' + @MatchClause)
						end
				
					exec(@sqlClausePrem)	
					print '@sqlClausePrem: ' + @sqlClausePrem
				end
		end

	IF (NOT EXISTS (SELECT * 
                 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'tmpPremDemo_' + @TableName))
		BEGIN
			declare @sqlPremDemoTableClause varchar(max) 
			set @sqlPremDemoTableClause = ' select 0 as SubscriptionID, '''' as Demographic, '''' as Value
								  into UAD_TEMP.dbo.tmpPremDemo_' + @TableName
			
			exec(@sqlPremDemoTableClause)
		END

	if LEN(@customList) > 0
		begin
			if @MatchCriteriaCount = 0
				begin
					declare @sqlCust varchar(max)
			
					if(@MatchTarget = 'Product')
						begin
							set @sqlCust = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and ps.PubID = ' + @ProductId + ' and cs.ResponseGroup in (' +  @customList + ')')
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sqlCust = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
										  into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join Mastercodesheet mcs with(nolock) on bd.MasterID = mcs.MasterID
										  join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
										  where bd.BrandID = ' + @BrandId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and mg.Name in (' +  @customList + ')')
						end
					else if @MatchTarget = 'Market'
						begin
							set @sqlCust = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and cs.ResponseGroup in (' +  @customList + ')')
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sqlCust = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
											   into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
											 ' from Subscriptions s with(nolock)
											   join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
											   join SubscriptionDetails sd with(nolock) on s.SubscriptionID = sd.SubscriptionID 
											   join Mastercodesheet mcs with(nolock) on sd.MasterID = mcs.MasterID
											   join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
											   where sf.ProcessCode = ''' +  @ProcessCode + '''
											   and mg.Name in (' +  @customList + ')')
						end
				
					exec(@sqlCust)	
					print '@sqlCust: ' + @sqlCust
				end
			else
				begin
					declare @sqlClauseCust varchar(max) 

					if(@MatchTarget = 'Product')
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where sf.ProcessCode = ''' +  @ProcessCode + '''
										  and ps.PubID = ' + @ProductId + ' and cs.ResponseGroup in (' +  @customList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Brand'
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
										  into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join vw_BrandConsensus bd with(nolock) on s.SubscriptionID = bd.SubscriptionID 
										  join Mastercodesheet mcs with(nolock) on bd.MasterID = mcs.MasterID
										  join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
										  where bd.BrandID = ' + @BrandId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and mg.Name in (' +  @customList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Market'
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, cs.ResponseGroup as Demographic, cs.Responsedesc as Value
										  into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
										' from Subscriptions s with(nolock)
										  join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
										  join PubSubscriptions ps with(nolock) on s.SubscriptionID = ps.SubscriptionID
										  join BrandProductMap bpm with(nolock) on ps.PubID = bpm.ProductID 
										  join Markets m with(nolock) on bpm.BrandID = m.BrandID
										  join PubSubscriptionDetail psd with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
										  join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
										  where m.MarketID = ' + @MarketId + '
										  and sf.ProcessCode = ''' +  @ProcessCode + '''
										  and cs.ResponseGroup in (' +  @customList + ')
										  and ' + @MatchClause)
						end
					else if @MatchTarget = 'Consensus'
						begin
							set @sqlClausePrem = (' select distinct s.SubscriptionID, mg.Name as Demographic, mcs.MasterDesc as Value
											   into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName + 
											 ' from Subscriptions s with(nolock)
											   join SubscriberFinal sf with(nolock) on s.IGRP_NO = sf.IGrp_No 
											   join SubscriptionDetails sd with(nolock) on s.SubscriptionID = sd.SubscriptionID 
											   join Mastercodesheet mcs with(nolock) on sd.MasterID = mcs.MasterID
											   join MasterGroups mg with(nolock) on mcs.MasterGroupID = mg.MasterGroupID 
											   where sf.ProcessCode = ''' +  @ProcessCode + '''
											   and mg.Name in (' +  @customList + ')
											   and ' + @MatchClause)
						end
				
					exec(@sqlClauseCust)	
					print '@sqlClauseCust: ' + @sqlClauseCust
				end
		end

	IF (NOT EXISTS (SELECT * 
                 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'tmpCustomDemo_' + @TableName))
		BEGIN
			declare @sqlCustDemoTableClause varchar(max) 
			set @sqlCustDemoTableClause = ' select 0 as SubscriptionID, '''' as Demographic, '''' as Value
								  into UAD_TEMP.dbo.tmpCustomDemo_' + @TableName
			
			exec(@sqlCustDemoTableClause)
		END
END
go