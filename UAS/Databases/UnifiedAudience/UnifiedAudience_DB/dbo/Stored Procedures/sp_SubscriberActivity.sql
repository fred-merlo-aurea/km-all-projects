CREATE PROCEDURE [dbo].[sp_SubscriberActivity]
(@SubscriptionID int,
@BrandID int)	
AS

BEGIN
	   SET NOCOUNT ON

       if @BrandID = 0
       Begin
       select 
			case when so.PubSubscriptionID is null then s.EMAIL else ps.Email end as Email, 
			'open' as activity, 
			ActivityDate, 
			bl.BlastID, '' as link , 
			'' as linkAlias, 
			Emailsubject, 
			SendTime
       from   
			Subscriptions s with (NOLOCK) join 
            SubscriberOpenActivity so with (NOLOCK) on so.SubscriptionID = s.SubscriptionID  left outer join
            PubSubscriptions ps with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID left outer join
            Blast bl with (NOLOCK) on so.BlastID = bl.BlastID
       where 
			s.SubscriptionID = @SubscriptionID --and BlastID <> 0
       union
       select 
			case when sc.PubSubscriptionID is null then s.EMAIL else ps.Email end as Email,  
			'click' as activity, 
			ActivityDate, 
			bl.BlastID,  
			link, 
			isnull(linkAlias, ''), 
			Emailsubject, 
			SendTime
       from   
			Subscriptions s with (NOLOCK) join 
            SubscriberClickActivity sc with (NOLOCK) on sc.SubscriptionID = s.SubscriptionID left outer join
            PubSubscriptions ps with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID left outer join
            Blast bl with (NOLOCK) on sc.BlastID = bl.BlastID 
       where 
			s.SubscriptionID = @SubscriptionID  -- and BlastID <> 0
       order by ActivityDate desc
       end
       else
       begin
       select 
			Email, 
            'open' as activity, 
            ActivityDate, 
            bl.BlastID, '' as link , 
            '' as linkAlias, 
            Emailsubject, 
            SendTime
       from   
			Subscriptions s with (NOLOCK) join 
            SubscriberOpenActivity so with (NOLOCK) on so.SubscriptionID = s.SubscriptionID left outer join 
            Blast bl with (NOLOCK) on so.BlastID = bl.BlastID
       where 
			s.SubscriptionID = @SubscriptionID and so.PubSubscriptionID is null
       union
       select 
			Email, 
            'click' as activity, 
            ActivityDate, 
            bl.BlastID,  
            link, 
            isnull(linkAlias, '') , 
            Emailsubject, 
            SendTime
       from   
			Subscriptions s  with (NOLOCK) join 
            SubscriberClickActivity sc with (NOLOCK) on sc.SubscriptionID = s.SubscriptionID left outer join 
            Blast bl with (NOLOCK) on sc.BlastID = bl.BlastID
       where 
			s.SubscriptionID = @SubscriptionID  and sc.PubSubscriptionID is null
       union
       select Email, 
             'open' as activity, 
             ActivityDate, 
             bl.BlastID, '' as link , 
             '' as linkAlias, 
             Emailsubject, 
             SendTime
       from   
			PubSubscriptions ps with (NOLOCK) join 
			SubscriberOpenActivity so with (NOLOCK) on so.PubSubscriptionID = ps.PubSubscriptionID left outer join 
			Blast bl with (NOLOCK) on so.BlastID = bl.BlastID JOIN
			branddetails bd with (NOLOCK) ON bd.pubID = ps.pubID JOIN
			Brand b with (NOLOCK) on b.BrandID = bd.BrandID
       where 
			ps.SubscriptionID = @SubscriptionID and bd.BrandID = @BrandID and b.Isdeleted = 0 
       union
       select 
			Email, 
            'click' as activity, 
            ActivityDate, 
            bl.BlastID,  
            link, 
            isnull(linkAlias, '') , 
            Emailsubject, 
            SendTime
       from   
			PubSubscriptions ps  with (NOLOCK) join 
            SubscriberClickActivity sc with (NOLOCK) on sc.PubSubscriptionID = ps.PubSubscriptionID left outer join 
            Blast bl with (NOLOCK) on sc.BlastID = bl.BlastID JOIN
            branddetails bd with (NOLOCK) ON bd.pubID = ps.pubID JOIN
            Brand b with (NOLOCK) on b.BrandID = bd.BrandID
       where 
			ps.SubscriptionID = @SubscriptionID  and bd.BrandID = @BrandID and b.Isdeleted = 0 
       order by ActivityDate desc
       end
END
