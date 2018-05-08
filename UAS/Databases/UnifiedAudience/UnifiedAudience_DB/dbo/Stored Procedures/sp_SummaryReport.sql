CREATE proc [dbo].[sp_SummaryReport]    
(    
 @MasterGroupID varchar(100) = '',
 @BrandID int
)    
as     
BEGIN

	SET NOCOUNT ON
   
		declare @MasterGroupCScount Table (MasterID int, count int)
	
		if @BrandID = 0		
			Begin					
				insert into @MasterGroupCScount
				select m.MasterID,  
					COUNT(distinct s.IGRP_NO) as count
				from Subscriptions s  with (NOLOCK) 
					join SubscriptionDetails sd  with (NOLOCK) on s.subscriptionID = sd.SubscriptionID 
					join Mastercodesheet m  with (NOLOCK) on sd.MasterID = m.MasterID
				where (LEN(@MasterGroupID) = 0 or m.MasterGroupID = @MasterGroupID)
				group by m.MasterID 
					
				select m.MasterValue, m.MasterDesc, mg.DisplayName , isnull(count,0) as count
				from MasterGroups mg  with (NOLOCK)  
					join Mastercodesheet m  with (NOLOCK) on mg.MasterGroupID= m.MasterGroupID 
					left outer join @MasterGroupCScount inn on inn.MasterID = m.MasterID 
				where (LEN(@MasterGroupID) = 0 or mg.MasterGroupID = @MasterGroupID)
				order by mg.DisplayName, m.MasterValue, m.MasterDesc						
			End
		Else
			Begin
				insert into @MasterGroupCScount
				select	m.MasterID, COUNT(distinct s.IGRP_NO) as count
				from Subscriptions s  with (NOLOCK) 
					join pubsubscriptions ps WITH (NOLOCK) ON s.subscriptionID = ps.subscriptionID 
					join vw_BrandConsensus v WITH (nolock) ON s.subscriptionid = v.subscriptionid 
					JOIN branddetails bd ON bd.BrandID = v.BrandID 
					join mastercodesheet m ON m.masterid = v.masterid 					
				where bd.BrandID = @BrandID  and(LEN(@MasterGroupID) = 0 or m.MasterGroupID = @MasterGroupID)
				group by m.MasterID 
					
				select distinct v.MasterValue, v.MasterDesc, v.DisplayName , isnull(count,0) as count
				from vw_Mapping v  with (NOLOCK)  
					join BrandDetails bd  with (NOLOCK) on bd.PubID = v.PubID 
					join brand b on bd.BrandID = b.BrandID 
					left outer join @MasterGroupCScount inn on inn.MasterID = v.MasterID 
				where (LEN(@MasterGroupID) = 0 or v.MasterGroupID = @MasterGroupID) and b.IsDeleted = 0 and b.BrandID = @BrandID 
				order by v.DisplayName, v.MasterValue, v.MasterDesc	
			End   
		
End