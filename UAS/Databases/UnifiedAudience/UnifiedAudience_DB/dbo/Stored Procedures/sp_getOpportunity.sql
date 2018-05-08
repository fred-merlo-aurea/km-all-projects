create proc [dbo].[sp_getOpportunity]  
 @userID uniqueidentifier,
 @SectorID varchar(100),  
 @FunctionID varchar(100),  
 @company varchar(100),  
 @title varchar(100),  
 @city varchar(50),  
 @state varchar(100),
 @country int ,
 @zipcode varchar(5)
AS  
BEGIN
	
	SET NOCOUNT ON

	--set @firstname = 'A'  
	--set @lastname = 'D'  
	--set @company = 'g'  
	--set @city = 'PLYMOUTH'  
	--set @state = 'MN'  
  
	create table #tmpSubscriptions (SubscriptionID int, Agingyr decimal(5,2))
	
 	CREATE UNIQUE CLUSTERED INDEX tmpSubscriptions_ind on  #tmpSubscriptions(SubscriptionID, Agingyr) with ignore_dup_key
 	
 	print ('1 / '+ convert(varchar(100),getdate(),113))
 	
 	insert into #tmpSubscriptions
	select SubscriptionID, DATEDIFF(dd,QDate, GETDATE())/365.0
	from Subscriptions s  with (NOLOCK) join Country c  with (NOLOCK) on s.CountryID = c.countryID
	where isExcluded = 0 and  rtrim(ltrim(isnull(company, ''))) <> '' and
	(  
		(len(@company)=0 or COMPANY LIKE @company + '%')  and
		(len(@Title)=0 or TITLE LIKE '%' + @title + '%')  and
		(len(@city)=0 or CITY LIKE @city + '%')  and  
		(len(@state)=0 or STATE in (select ITEMS from dbo.fn_Split(@state,',')))  and
		(@country=0 or s.CountryID = @country)  and
		(len(@zipcode)=0 or ZIP = @zipcode)
	)   
	order by s.SubscriptionID
	
	print ('2 / '+ convert(varchar(100),getdate(),113))
	
	--select s.subscriptionID, s.company, s.countryID, sd.MasterID from  Subscriptions s join SubscriptionDetails sd on s.SubscriptionID = sd.SubscriptionID join Mastercodesheet m on sd.MasterID = m.MasterID
	--	join Country c on s.CountryID = c.countryID
	--	where isExcluded = 0 and m.MasterGroup = 'MAST_BUS'  and
	--	(len(@company)=0 or COMPANY LIKE @company + '%')  and
	--	(len(@country)=0 or s.CountryID = @country)
	--order by s.SubscriptionID	
		
	if len(@SectorID) > 0
		Begin
			delete from #tmpSubscriptions 
			where SubscriptionID not in 
			(
				select s.subscriptionID
				from #tmpSubscriptions s  with (NOLOCK)  
					join SubscriptionDetails sd  with (NOLOCK) on s.SubscriptionID = sd.SubscriptionID 
					join Mastercodesheet m  with (NOLOCK) on sd.MasterID = m.MasterID 
					join (select ITEMS from dbo.fn_Split(@SectorID,',')) sec on sec.Items = sd.MasterID
				where	m.MasterGroup = 'MAST_SEC'
			)
		End
	Else
		Begin
			delete from #tmpSubscriptions 
			where SubscriptionID not in 
			(
				select s.subscriptionID
				from #tmpSubscriptions s  with (NOLOCK)  
					join SubscriptionDetails sd  with (NOLOCK) on s.SubscriptionID = sd.SubscriptionID 
					join Mastercodesheet m  with (NOLOCK) on sd.MasterID = m.MasterID 
					join SectorCategoryMapping scm  with (NOLOCK) on m.MasterID = scm.MasterID 
					join usersectors us  with (NOLOCK) on scm.SectorCategoryID = us.SectorCategoryID
				where us.UseriD = @userID and ResponseGroup='sector' and m.MasterGroup = 'MAST_SEC'
			)		
		End
	
	print ('3 / '+ convert(varchar(100),getdate(),113))
	
	if LEN(@FunctionID) > 0
	Begin
		delete from #tmpSubscriptions 
		where SubscriptionID not in 
		(
			select s.subscriptionID
			from #tmpSubscriptions s  with (NOLOCK) 
				join SubscriptionDetails sd  with (NOLOCK) on s.SubscriptionID = sd.SubscriptionID 
				join Mastercodesheet m  with (NOLOCK) on sd.MasterID = m.MasterID  
				join (select ITEMS from dbo.fn_Split(@FunctionID,',')) fn on fn.Items = sd.MasterID
			where	m.MasterGroup = 'MAST_FUN' 
		)
	End
	
	print ('4 / '+ convert(varchar(100),getdate(),113))
	
	select s.SubscriptionID, FNAME, LNAME, COMPANY, Address, CITY,STATE,TITLE, c.Country, s.ZIP, QDate, EmailExists , PhoneExists, FaxExists, max(Price) as PRICE, max(p.BGColor) as BGColor, max(p.FGColor) as FGColor
	from	#tmpSubscriptions tmp 
		join Subscriptions s  with (NOLOCK) on tmp.SubscriptionID = s.SubscriptionID 
		join Country c  with (NOLOCK) on s.CountryID = c.countryID 
		--join SubscriptionDetails sd  with (NOLOCK) on s.SubscriptionID = sd.SubscriptionID 
		join PubSubscriptions ps  with (NOLOCK) on ps.SubscriptionID = s.SubscriptionID 
		join Pubs pb  with (NOLOCK) on  pb.PubID = ps.PubID 
		join Pricing p  with (NOLOCK) on p.PriceGroupID = pb.PriceGroupID
	where  tmp.Agingyr between RangeStart and RangeEnd
	group by s.SubscriptionID, FNAME, LNAME, COMPANY,Address,CITY,STATE,TITLE, c.Country, s.ZIP, QDate, EmailExists , PhoneExists, FaxExists
	
	drop table #tmpSubscriptions
	
	print ('5 / '+ convert(varchar(100),getdate(),113))
	
  END