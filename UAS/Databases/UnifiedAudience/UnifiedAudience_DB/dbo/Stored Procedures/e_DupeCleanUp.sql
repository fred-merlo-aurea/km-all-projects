
CREATE PROCEDURE e_DupeCleanUp
WITH EXECUTE as OWNER
AS
BEGIN

	SET NOCOUNT ON



	--declare     @i int,
	--			@KeepSubscriptionID int,
	--			@KeepPubsubscriptionID int,
	--			@pubID int,
	--			@pubsubscriptionID int,
	--			@codesheetID int

	--set @i = 1
	--print (' Process Dupes : ' +  convert(varchar,@i)  + ' / ' +  convert(varchar(20), getdate(), 114))

	--set nocount on

	--create table #curtable (KeepSubscriptionID int, DupeSubscriptionID int) 

	--CREATE INDEX IDX_Users_UserName ON #curtable(KeepSubscriptionID)
	
	--create table #NameSwapID(SubId int,DupeSubId int)
		
	--declare @tmp table (SubscriptionID int UNIQUE CLUSTERED (SubscriptionID))     

	--declare @loopcounter int = 1

	--while @loopcounter <= 15
	--Begin

	--	  delete from #curtable
	--	  delete from #NameSwapID

	--	  if @loopcounter = 1 -- MATCH ON COMLETE FNAME,LNAME,COMPANY, ADDRESS, CITY, STATE, ZIP, PHONE, EMAIL
	--	  Begin
	--			insert into #curtable
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select isnull(FNAME,'') FNAME, isnull(LNAME,'') LNAME, isnull(COMPANY,'') COMPANY, isnull(ADDRESS,'') ADDRESS, isnull(CITY,'')CITY, isnull(State,'')State, 
	--			isnull(ZIP,'') ZIP, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			group by
	--			isnull(FNAME,''), isnull(LNAME,''), isnull(COMPANY,''), isnull(ADDRESS,''), isnull(CITY,''), isnull(State,''), isnull(ZIP,''), isnull(PHONE,''), isnull(EMAIL,'')
	--			having COUNT(subscriptionID) > 1
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on isnull(s.FNAME,'') = x.FNAME and 
	--							  isnull(s.LNAME,'') = x.LNAME and 
	--							  isnull(s.COMPANY,'') = x.COMPANY and 
	--							  isnull(s.ADDRESS,'') = x.ADDRESS and 
	--							  isnull(s.CITY,'') = x.CITY and 
	--							  isnull(s.State,'') = x.State and 
	--							  isnull(s.ZIP,'') = x.ZIP and 
	--							  isnull(s.PHONE,'') = x.PHONE and 
	--							  isnull(s.EMAIL,'') = x.EMAIL        
	--			--where minsubscriptionID = 17840412      
	--			order by minsubscriptionID    
	                        
	--	  End
	--	  else if @loopcounter = 2 -- COMLETE FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #curtable
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname,ISNULL("ADDRESS",'') "ADDRESS",ZIP,CountryID, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
	--			group by FNAME,LNAME,"ADDRESS",ZIP,CountryID
	--			having COUNT(*) > 1 
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on 
	--							  isnull(s.FNAME,'') = isnull(x.FNAME,'') and 
	--							  isnull(s.LNAME,'') = isnull(x.LNAME,'') and 
	--							  isnull(s.ADDRESS,'') = isnull(x.ADDRESS,'') and 
	--							  s.ZIP = x.ZIP and 
	--							  ISNULL(x.CountryID,0) = ISNULL(S.CountryID,0)
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 3 -- MATCH FIELDS FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname,left(lname,6) lname,left("ADDRESS",15) "ADDRESS",ZIP, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
	--			group by left(fname,3),left(lname,6),left("ADDRESS",15),ZIP
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock)  on 
	--							  left(S.fname,3) = isnull(x.FNAME,'') and 
	--							  left(S.lname,6) = isnull(x.LNAME,'') and 
	--							  left(S.ADDRESS,15) = isnull(x.ADDRESS,'') and 
	--							  s.ZIP = x.ZIP 
	--			--where minsubscriptionID = 17840412      
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 4 -- COMPLETE FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(email,'') Email, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,EMAIL
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.FNAME = x.FNAME and 
	--							  s.LNAME = x.LNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End
	--	  else if @loopcounter = 5 -- COMPLETE FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select fname,lname,company, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,company
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.FNAME = x.FNAME and 
	--							  s.LNAME = x.LNAME and 
	--							  s.company = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID                
	--	  End               
	--	  else if @loopcounter = 6 -- MATCH FIELDS FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname ,left(lname,6) lname,left(company,8) company, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by left(fname,3),left(lname,6),left(company,8)
	--			having COUNT(*) > 1

	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,3) = x.FNAME and 
	--							  LEFT(s.LNAME,6)  = x.LNAME and 
	--							  LEFT(s.company,8) = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 7 -- MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select LEFT(fname,3) fname,LEFT(lname,6) lname,Email, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by LEFT(FNAME,3),LEFT(LNAME,6),EMAIL
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,3) = x.FNAME and 
	--							  LEFT(s.LNAME,6)  = x.LNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 8 -- COMPLETE FNAME,LNAME,PHONE
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(phone,'') phone, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by FNAME,LNAME,phone
	--			having COUNT(*) > 1
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  ISNULL(s.FNAME,'')  = x.FNAME and 
	--							  ISNULL(s.LNAME,'')   = x.LNAME and 
	--							  ISNULL(s.phone,'')  = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 9 -- MATCH FIELDS FNAME,LNAME,PHONE
	--	  Begin
	--			insert into #curtable   
	--			select minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select left(fname,3) fname,left(lname,6) lname,phone , COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
	--			from Subscriptions with (NOLOCK)
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			group by left(fname,3),left(lname,6),phone
	--			having COUNT(*) > 1 -- 16924 dupes
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  left(ISNULL(s.FNAME,''),3)  = x.FNAME and 
	--							  left(ISNULL(s.LNAME,''),6)  = x.LNAME and 
	--							  ISNULL(s.phone,'')  = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
	--			order by minsubscriptionID          
	--	  End               
	--	  else if @loopcounter = 10 -- EMAIL ONLY PROFILES MATCHING WITH ANOTHER PROFILE WITH FNAME/LNAME MASTER EMAIL ONLY
	--	  Begin
	--			insert into #curtable   
	--			select  minsubscriptionID, s.subscriptionID
	--			from
	--			(
	--			select fname,lname,email, SubscriptionID as minsubscriptionID from Subscriptions with (NOLOCK) where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(email,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  s.EMAIL = x.email
	--			where ISNULL(s.fname,'') = '' and ISNULL(s.lname,'') = '' and ISNULL(s.email,'') <> ''
	--			order by minsubscriptionID     
	                 
	--	  End               
	--	  else if @loopcounter = 11 -- SAWP NAME MATCH ON COMLETE FNAME,LNAME,COMPANY, ADDRESS, CITY, STATE, ZIP, PHONE, EMAIL
	--	  Begin
				
	--			Insert Into #NameSwapID
	--			select x.SubscriptionID as SubId, s.subscriptionID as DupeSubId
	--			from
	--			(
	--			select isnull(FNAME,'') FNAME, isnull(LNAME,'') LNAME, isnull(COMPANY,'') COMPANY, isnull([ADDRESS],'') [ADDRESS], isnull(CITY,'')CITY, isnull([State],'')[State], 
	--			isnull(ZIP,'') ZIP, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, subscriptionID
	--			from Subscriptions with (NOLOCK)
	--			group by
	--			isnull(FNAME,''), isnull(LNAME,''), isnull(COMPANY,''), isnull([ADDRESS],''), isnull(CITY,''), isnull([State],''), isnull(ZIP,''), isnull(PHONE,''), isnull(EMAIL,''), subscriptionID
	--			)
	--			x 
	--			join subscriptions s  WITH (nolock)  on isnull(s.FNAME,'') = x.LNAME and 
	--							  isnull(s.LNAME,'') = x.FNAME  and 
	--							  isnull(s.COMPANY,'') = x.COMPANY and 
	--							  isnull(s.[ADDRESS],'') = x.[ADDRESS] and 
	--							  isnull(s.CITY,'') = x.CITY and 
	--							  isnull(s.[State],'') = x.[State] and 
	--							  isnull(s.ZIP,'') = x.ZIP and 
	--							  isnull(s.PHONE,'') = x.PHONE and 
	--							  isnull(s.EMAIL,'') = x.EMAIL        
	--			where x.SubscriptionID <> s.subscriptionID
	--			order by x.subscriptionID		

			    
	--	  End		  
	--	  else if @loopcounter = 12 -- SAWP NAME MATCH FIELDS FNAME,LNAME,ADDRESS
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,LEFT(Address,15) [address],LEFT(zip,5) zip,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(Address,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(zip,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  LEFT(s.ADDRESS,15) = x.address and
	--							  LEFT(s.Zip,5) = x.Zip
	--			where ISNULL(s.Address,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' and ISNULL(s.Zip,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID   		        
	--	  End
	--	  else if @loopcounter = 13 -- SAWP NAME MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,Email,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  s.EMAIL = x.EMAIL
	--			where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID           
	--	  End   
	--	  else if @loopcounter = 14 -- SAWP NAME MATCH FIELDS FNAME,LNAME,COMPANY
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,LEFT(company,8) company,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  LEFT(s.company,8) = x.company
	--			where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID            
	--	  End 
	--	  else if @loopcounter = 15 -- SAWP NAME MATCH FIELDS FNAME,LNAME,EMAIL
	--	  Begin
	--			insert into #NameSwapID
	--			select x.subscriptionID,s.SubscriptionID
	--			from
	--			(
	--			select LEFT(lname,6) lname,LEFT(fname,3) fname,phone,SubscriptionID
	--			from Subscriptions  WITH (nolock) 
	--			where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
	--			)
	--			x 
	--			join subscriptions s WITH (nolock) on 
	--							  LEFT(s.FNAME ,6) = x.LNAME and 
	--							  LEFT(s.LNAME,3)  = x.FNAME and 
	--							  s.phone = x.phone
	--			where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' AND x.SubscriptionID != s.SubscriptionID
	--			order by s.subscriptionID          
	--	  End 
	    
	--	  -- clean up swap matches and insert id's into #curtable
	--	    Delete from #NameSwapID
	--	    Where DupeSubId in (select case when SubId > DupeSubId then DupeSubId else SubId end from #NameSwapID)

	--	    Insert into #curtable
	--	    Select SubId,DupeSubId from #NameSwapID	
		    
	--	  -- End SWAP Clean UP

	--	  DECLARE c_Subscriptions CURSOR FOR select distinct KeepSubscriptionID from #curtable

	--	  OPEN c_Subscriptions  
	--	  FETCH NEXT FROM c_Subscriptions INTO @KeepSubscriptionID

	--	  WHILE @@FETCH_STATUS = 0  
	--	  BEGIN  
	--			set @KeepPubsubscriptionID = 0
	--			set @codesheetID = 0
	--			set @pubID = 0
	--			set @pubsubscriptionID = 0
	            
	--			delete from @tmp

	--			insert into @tmp
	--			select DupeSubscriptionID from #curtable 
	--			where KeepSubscriptionID = @KeepSubscriptionID
	            
	--			if (not exists (select 1 from #curtable where KeepSubscriptionID = @KeepSubscriptionID and DupeSubscriptionID = @KeepSubscriptionID))
	--			Begin
	--				  insert into @tmp values (@KeepSubscriptionID)                 
	--			End
	                  
	--			print (convert(varchar,@i) + ' / ' + convert(varchar(20), getdate(), 114) + ' - SubscriptionID :' + convert(varchar(100),@KeepSubscriptionID) ) --+ ' - ' + @FNAME + ' - ' + @LNAME + ' - ' + @COMPANY + ' - ' + @ADDRESS + ' - ' + @CITY + ' - ' + @State + ' - ' + @ZIP + ' - ' + @PHONE + ' - ' + @EMAIL
	            
	--			if exists (select top 1 pubID from PubSubscriptions ps  WITH (NOLOCK) join @tmp t on ps.SubscriptionID = t.SubscriptionID group by PubID having COUNT(pubsubscriptionID) > 1)
	--			Begin
	                              
	--				  DECLARE c_PubSubscriptions CURSOR 
	--				  FOR select ps.pubID, ps.PubSubscriptionID 
	--				  from PubSubscriptions ps  with (NOLOCK) join  @tmp t on t.subscriptionID = ps.subscriptionID
	--				  where ps.SubscriptionID <> @KeepSubscriptionID and
	--				  PubID in (select pubID from PubSubscriptions ps    with (NOLOCK) where SubscriptionID in (select SubscriptionID from @tmp) group by pubID having COUNT(*) > 1)

	--				  OPEN c_PubSubscriptions  
	--				  FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
	                  
	--				  WHILE @@FETCH_STATUS = 0  
	--				  BEGIN  
	--						--print (' OPEN c_PubSubscriptions / ' + convert(varchar(20), getdate(), 114)   )           
	                        
	--						select @KeepPubsubscriptionID = PubSubscriptionID 
	--						from PubSubscriptions with (NOLOCK)
	--						where PubID = @pubID and SubscriptionID = @KeepSubscriptionID

	--						if (@KeepPubsubscriptionID = 0)
	--						Begin
	                        
	--							  print 'Insert KeepPubsubscriptionID : '
	                        
	--							  insert into PubSubscriptions (SubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email)
	--							  select @KeepSubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email
	--							  from PubSubscriptions  WITH (nolock) 
	--							  where pubsubscriptionID = @pubsubscriptionID
	                              
	--							  set @KeepPubsubscriptionID = @@IDENTITY
	--						End

	                        
	--						/********* pubSubscriptiondetail ***********/
	--						DECLARE c_PubSubscriptiondetail CURSOR 
	--						FOR 
	--						select CodesheetID from PubSubscriptionDetail  WITH (NOLOCK)
	--							  where PubSubscriptionID in (@pubsubscriptionID, @KeepPubsubscriptionID)
	--							  group by  CodesheetID
	--							  having COUNT(pubsubscriptiondetailID) > 1
	                        
	--						--print (' c_PubSubscriptiondetail / ' + convert(varchar(20), getdate(), 114)   )
	                              
	--						OPEN c_PubSubscriptiondetail  
	--						FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
	                        
	--						WHILE @@FETCH_STATUS = 0  
	--						BEGIN  
	                              
	--							  delete from PubSubscriptionDetail
	--							  where PubSubscriptionID = @pubsubscriptionID and CodesheetID = @codesheetID
	                        
	--							  FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
	--						END

	--						CLOSE c_PubSubscriptiondetail  
	--						DEALLOCATE c_PubSubscriptiondetail  
	--						/********* pubSubscriptiondetail ***********/

	--						update PubSubscriptionDetail
	--						set PubSubscriptionID = @KeepPubsubscriptionID
	--						where PubSubscriptionID = @pubsubscriptionID
	                        
	--						delete from PubSubscriptions 
	--						where  PubSubscriptionID = @pubsubscriptionID and PubID = @pubID
	                        
	--						FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
	                        
	--				  END

	--				  CLOSE c_PubSubscriptions  
	--				  DEALLOCATE c_PubSubscriptions  
	--			End
	                  
	--			update PubSubscriptionDetail
	--			set SubscriptionID = @KeepSubscriptionID
	--			where SubscriptionID in  (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			update PubSubscriptions
	--			set SubscriptionID = @KeepSubscriptionID
	--			where SubscriptionID in  (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			delete from SubscriberMasterValues where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from SubscriptionsExtension where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from SubscriptionDetails where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	--			delete from Subscriptions where SubscriptionID in (select SubscriptionID from @tmp) and SubscriptionID <> @KeepSubscriptionID
	            
	--			set @i = @i + 1
	                  
	--			FETCH NEXT FROM c_Subscriptions INTO @KeepSubscriptionID

	--	  END

	--	  CLOSE c_Subscriptions  
	--	  DEALLOCATE c_Subscriptions  

	--	  set @loopcounter = @loopcounter + 1      

	--	  truncate table subscriptiondetails

	--	  truncate table dbo.SubscriberMasterValues

	--	  insert into subscriptiondetails
	--		  select distinct subscriptionID, cb.masterID 
	--		  from  PubSubscriptionDetail psd with (NOLOCK) join 
	--					  CodeSheet_Mastercodesheet_Bridge cb  with (NOLOCK)  on psd.CodeSheetID = cb.CodeSheetID
		            
	--	  insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	--		  SELECT 
	--			MasterGroupID, [SubscriptionID] , 
	--			STUFF((
	--				SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
	--				FROM [dbo].[SubscriptionDetails] sd1   WITH (nolock) join Mastercodesheet mc1   WITH (nolock) on sd1.MasterID = mc1.MasterID  
	--				WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
	--				FOR XML PATH (''))
	--			,1,1,'') AS CombinedValues
	--		  FROM 
	--				(
	--				  SELECT DISTINCT sd.SubscriptionID, mg.MasterGroupID
	--				  FROM [dbo].[SubscriptionDetails] sd  WITH (nolock)  join Mastercodesheet mc  WITH (nolock)  on sd.MasterID = mc.MasterID join MasterGroups mg  WITH (nolock)  on mg.MasterGroupID = mc.MasterGroupID	            
	--				)
	--		  Results
	--		  GROUP BY [SubscriptionID] , MasterGroupID
	--		  ORDER BY SubscriptionID             
		      
	--END -- End while loop

	--drop table #curtable
	--drop table #NameSwapID

END