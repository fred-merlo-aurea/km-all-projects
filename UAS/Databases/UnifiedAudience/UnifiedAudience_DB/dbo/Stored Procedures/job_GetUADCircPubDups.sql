

IF EXISTS (SELECT 1 FROM Sysobjects where name = 'Job_GetUADCircPubDups')
DROP Proc Job_GetUADCircPubDups
GO

CREATE Proc [dbo].[Job_GetUADCircPubDups]
	@PUBCode varchar(100)
	

AS
--select * from pubS where iscirc = 1
--execute tradepressMasterDB.dbo.Job_GetUADCircPubDups @PubCode = 'BOM'
--select * from tradepressMasterDB.dbo.tmp_GetUADCircDups 
--execute Job_GetUADCircPubDups @PubCode = 'REW'


declare @FIRSTNAME varchar(100), @LASTNAME varchar(100), @COMPANY varchar(100), @ADDRESS1 varchar(100), @CITY varchar(100), @REGIONCODE varchar(100), @ZIPCODE varchar(100), @PHONE varchar(100), @EMAIL varchar(100),
		@i int,
		@KeepPubSubscriptionID int,
		@pubID int,
		@pubsubscriptionID int,
		@codesheetID int

set @i = 1

select @pubcode = @PUBCode

select @pubID = PubID from Pubs where PubCode = @pubcode and Iscirc = 1

declare @tmp_dupes table(ID int, loop int, 	KeepPubSubscriptionID int, DupePubSubscriptionID int)

set nocount on

create table #curtable (KeepPubSubscriptionID int, DupePubSubscriptionID int, rnk int)	

CREATE INDEX IDX_curtable_KeepPubSubscriptionID ON #curtable(KeepPubSubscriptionID)

declare @loopcounter int = 1


Create Table #MatchGroups (
PubsubscriptionID int, FirstName varchar(50), LastName varchar(50), FirstName3 varchar(3), LastName6 varchar(6), Address5 varchar(255), City varchar(50), Zipcode varchar(50), Title5 varchar(5), Company5 varchar(5), Phone varchar(50), Email varchar(255) )

Create Index t_firstLast on #matchGroups (FirstName, LastName)
Create Index t_first3Last6 on #matchGroups (FirstName3, LastName6)
Create Index t_email on #matchGroups (Email)

insert into #MatchGroups
	select PubsubscriptionID, 
	IsNULL(FirstName,''), 
	IsNULL(LastName,''), 
	IsNULL(LEFT(FirstName,3),''), 
	IsNULL(LEFT(LastName,6),''), 
	IsNULL(LEFT(Address1,5),''), 
	IsNULL(City,''), 
	IsNULL(ZipCode,''), 
	IsNULL(LEFT(Title, 5),''), 
	IsNULL(LEFT(Company,5),''), 
	IsNULL(Phone,''), 
	IsNULL(Email,'') 
	from PubSubscriptions 
	where PubID = @pubID



while @loopcounter <= 16
Begin

	delete from #curtable
	-- NEW Match
	if @loopcounter = 1 -- First Name (3 char), Last Name (6 char), address (5 char) and either city
	Begin
		insert into #curtable
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		From
		(
			select FirstName3, LastName6, Address5, City,  MIN(PubSubscriptionID) as minpubSubscriptionID
			from #MatchGroups with (NOLOCK)
			Where FirstName3<>'' AND LastName6<>'' and Address5 <> '' AND City <> ''
			group by
			FirstName3, LastName6, Address5, City
			having COUNT(pubSubscriptionID) > 1
		)
		x 
		join #MatchGroups s  with (NOLOCK) on 
				Left(s.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.FirstName3),Len(x.FirstName3)),3)) = Left(x.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(len(s.FirstName3),Len(x.FirstName3)),3)) 
			and Left(s.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.LastName6),Len(x.LastName6)),6)) = Left(x.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(LEN(s.LastName6),Len(x.LastName6)),6))
			AND s.Address5 = x.Address5 
			AND s.City = x.City
		Where (s.FirstName<> '' and s.LastName<>'') 
		order by minpubSubscriptionID		
	end
	-- NEW Match
	else if @loopcounter = 2 -- First Name (3 char), Last Name (6 char), address (5 char) and zip 
	Begin
		insert into #curtable
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		From
		(
			select FirstName3, LastName6, Address5, Zipcode,  MIN(PubSubscriptionID) as minpubSubscriptionID
			from #MatchGroups with (NOLOCK)
			Where FirstName3<>'' AND LastName6<>'' and Address5 <> '' AND Zipcode <> ''
			group by
			FirstName3, LastName6, Address5, Zipcode
			having COUNT(pubSubscriptionID) > 1
		)
		x 
		join #MatchGroups s  with (NOLOCK) on 
				Left(s.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.FirstName3),Len(x.FirstName3)),3)) = Left(x.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(len(s.FirstName3),Len(x.FirstName3)),3)) 
			and Left(s.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.LastName6),Len(x.LastName6)),6)) = Left(x.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(LEN(s.LastName6),Len(x.LastName6)),6))
			AND s.Address5 = x.Address5 
			AND s.Zipcode = x.Zipcode
		Where (s.FirstName<> '' and s.LastName<>'') 
		order by minpubSubscriptionID
	end
	-- NEW Match
	else if @loopcounter = 3 -- First Name (3 char), Last Name (6 char), Company and TItle
	Begin
		insert into #curtable
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		From
		(
			select FirstName3, LastName6, Company5, Title5,  MIN(PubSubscriptionID) as minpubSubscriptionID
			from #MatchGroups with (NOLOCK)
			Where FirstName3<>'' AND LastName6<>'' and Company5 <> '' AND Title5 <> ''
			group by
			FirstName3, LastName6, Company5, Title5
			having COUNT(pubSubscriptionID) > 1
		)
		x 
		join #MatchGroups s  with (NOLOCK) on 
				Left(s.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.FirstName3),Len(x.FirstName3)),3)) = Left(x.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(len(s.FirstName3),Len(x.FirstName3)),3)) 
			and Left(s.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.LastName6),Len(x.LastName6)),6)) = Left(x.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(LEN(s.LastName6),Len(x.LastName6)),6))
			AND s.Company5 = x.Company5 
			AND s.Title5 = x.Title5
		Where (s.FirstName<> '' and s.LastName<>'') 
		order by minpubSubscriptionID
	end
	-- NEW Match
	else if @loopcounter = 4 -- First Name (3 char), Last Name (6 char) and Phone
	Begin
		insert into #curtable
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		From
		(
			select FirstName3, LastName6, Phone,  MIN(PubSubscriptionID) as minpubSubscriptionID
			from #MatchGroups with (NOLOCK)
			Where FirstName3<>'' AND LastName6<>'' AND Phone <> ''
			group by
			FirstName3, LastName6, Phone
			having COUNT(pubSubscriptionID) > 1
		)
		x 
		join #MatchGroups s  with (NOLOCK) on 
				Left(s.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.FirstName3),Len(x.FirstName3)),3)) = Left(x.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(len(s.FirstName3),Len(x.FirstName3)),3)) 
			and Left(s.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.LastName6),Len(x.LastName6)),6)) = Left(x.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(LEN(s.LastName6),Len(x.LastName6)),6))
			AND s.Phone = x.Phone
		Where (s.FirstName<> '' and s.LastName<>'') 
		order by minpubSubscriptionID
	end
	-- NEW Match
	else if @loopcounter = 5 -- First Name (3 char), Last Name (6 char), and email
	Begin
		insert into #curtable
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		From
		(
			select FirstName3, LastName6, Email,  MIN(PubSubscriptionID) as minpubSubscriptionID
			from #MatchGroups with (NOLOCK)
			Where FirstName3<>'' AND LastName6<>'' and Email <> ''
			group by
			FirstName3, LastName6, Email
			having COUNT(pubSubscriptionID) > 1
		)
		x 
		join #MatchGroups s  with (NOLOCK) on 
				Left(s.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.FirstName3),Len(x.FirstName3)),3)) = Left(x.FirstName3, master.dbo.fn_smallest(master.dbo.fn_smallest(len(s.FirstName3),Len(x.FirstName3)),3)) 
			and Left(s.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(Len(s.LastName6),Len(x.LastName6)),6)) = Left(x.LastName6, master.dbo.fn_smallest(master.dbo.fn_smallest(LEN(s.LastName6),Len(x.LastName6)),6))
			AND s.Email = x.Email
		Where (s.FirstName<> '' and s.LastName<>'') 
		order by minpubSubscriptionID
	end
	-- NEW Match
	else if @loopcounter = 6 -- Just Email where first and last names are blank
	Begin
		insert into #curtable
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		From
		(
			select Email,  MIN(PubSubscriptionID) as minpubSubscriptionID
			from #MatchGroups with (NOLOCK)
			Where Email <> ''
			group by
			Email
			having COUNT(pubSubscriptionID) > 1
		)
		x 
		join #MatchGroups xx on xx.Email = x.Email
		join #MatchGroups s  with (NOLOCK) on 
				s.Email = x.Email
		Where (xx.FirstName= '' and xx.LastName='')  
		order by minpubSubscriptionID
	end


/*************************************  This section covered by above matching ***************************************************
	Else if @loopcounter = 7 -- MATCH ON COMLETE FIRSTNAME,LASTNAME,COMPANY, ADDRESS1, CITY, REGIONCODE, ZIPCODE, PHONE, EMAIL
	Begin
		insert into #curtable
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
			select isnull(FIRSTNAME,'') FIRSTNAME, isnull(LASTNAME,'') LASTNAME, isnull(COMPANY,'') COMPANY, isnull(ADDRESS1,'') ADDRESS1, isnull(CITY,'')CITY, isnull(REGIONCODE,'')REGIONCODE, 
			isnull(ZIPCODE,'') ZIPCODE, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, COUNT(pubSubscriptionID) as dupecount, MIN(PubSubscriptionID) as minpubSubscriptionID
			from PubSubscriptions with (NOLOCK)
			Where PubID = @pubID
			group by
			isnull(FIRSTNAME,''), isnull(LASTNAME,''), isnull(COMPANY,''), isnull(ADDRESS1,''), isnull(CITY,''), isnull(REGIONCODE,''), isnull(ZIPCODE,''), isnull(PHONE,''), isnull(EMAIL,'')
			having COUNT(pubSubscriptionID) > 1
		)
		x 
		join PubSubscriptions s  with (NOLOCK) on isnull(s.FIRSTNAME,'') = x.FIRSTNAME and 
					isnull(s.LASTNAME,'') = x.LASTNAME and 
					isnull(s.COMPANY,'') = x.COMPANY and 
					isnull(s.ADDRESS1,'') = x.ADDRESS1 and 
					isnull(s.CITY,'') = x.CITY and 
					isnull(s.REGIONCODE,'') = x.REGIONCODE and 
					isnull(s.ZIPCODE,'') = x.ZIPCODE and 
					isnull(s.PHONE,'') = x.PHONE and 
					isnull(s.EMAIL,'') = x.EMAIL 		
		Where s.PubID = @pubID
		order by minpubSubscriptionID
				
	End
	else if @loopcounter = 8 -- COMLETE FIRSTNAME,LASTNAME,ADDRESS1
	Begin
		insert into #curtable
		select minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select ISNULL(FIRSTNAME,'') FIRSTNAME, ISNULL(LASTNAME,'') LASTNAME,ISNULL("ADDRESS1",'') "ADDRESS1",ZIPCODE,CountryID, MIN(pubSubscriptionID) as minpubSubscriptionID
		from PubSubscriptions with (NOLOCK)
		where  PubID = @pubID and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> '' and ISNULL("ADDRESS1",'') <> ''
		group by FIRSTNAME,LASTNAME,"ADDRESS1",ZIPCODE,CountryID
		having COUNT(*) > 1 
		)
		x 
		join PubSubscriptions s  with (NOLOCK) on 
					isnull(s.FIRSTNAME,'') = isnull(x.FIRSTNAME,'') and 
					isnull(s.LASTNAME,'') = isnull(x.LASTNAME,'') and 
					isnull(s.ADDRESS1,'') = isnull(x.ADDRESS1,'') and 
					s.ZIPCODE = x.ZIPCODE and 
					ISNULL(x.CountryID,0) = ISNULL(S.CountryID,0)
		Where s.PubID = @pubID
		order by minpubSubscriptionID		
	End
	else if @loopcounter = 9 -- MATCH FIELDS FIRSTNAME,LASTNAME,ADDRESS1
	Begin
		insert into #curtable	
		select minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select left(FIRSTNAME,3) FIRSTNAME,left(LASTNAME,6) LASTNAME,left("ADDRESS1",15) "ADDRESS1",ZIPCODE, COUNT(pubSubscriptionID) as counts, MIN(pubSubscriptionID) as minpubSubscriptionID
		from PubSubscriptions  WITH (nolock) 
		where PubID = @pubID and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> '' and ISNULL("ADDRESS1",'') <> ''
		group by left(FIRSTNAME,3),left(LASTNAME,6),left("ADDRESS1",15),ZIPCODE
		having COUNT(*) > 1
		)
		x 
		join PubSubscriptions s WITH (nolock)  on 
					left(S.FIRSTNAME,3) = isnull(x.FIRSTNAME,'') and 
					left(S.LASTNAME,6) = isnull(x.LASTNAME,'') and 
					left(S.ADDRESS1,15) = isnull(x.ADDRESS1,'') and 
					s.ZIPCODE = x.ZIPCODE 
		Where s.PubID = @pubID
		order by minpubSubscriptionID		
	End
	else if @loopcounter = 10 -- COMPLETE FIRSTNAME,LASTNAME,EMAIL
	Begin
		insert into #curtable	
		select distinct minpubSubscriptionID, s1.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select ISNULL(FIRSTNAME,'') FIRSTNAME, ISNULL(LASTNAME,'') LASTNAME, ISNULL( s.email,'') Email, COUNT(distinct s.pubSubscriptionID) as counts, MIN(s.pubSubscriptionID ) as minpubSubscriptionID
		from PubSubscriptions s WITH (nolock)
		where PubID = @pubID and	ISNULL( s.email,'') <> '' and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> ''
		group by FIRSTNAME,LASTNAME, s.EMAIL
		having COUNT(distinct s.pubSubscriptionID)  > 1
		)
		x 
		join PubSubscriptions s1 WITH (nolock)   on 
					s1.FIRSTNAME = x.FIRSTNAME and 
					s1.LASTNAME = x.LASTNAME and 
					s1.EMAIL = x.EMAIL
		where s1.PubID = @pubID and ISNULL(s1.email,'') <> '' and ISNULL(s1.FIRSTNAME,'') <> '' and ISNULL(s1.LASTNAME,'') <> ''
		order by minpubSubscriptionID		
	end
	else if @loopcounter = 11 -- MATCH FIELDS partial FIRSTNAME,LASTNAME,COMPANY, ZIPCODE
	Begin
		insert into #curtable	
		select minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select left(FIRSTNAME,3) FIRSTNAME ,left(LASTNAME,6) LASTNAME,left(company,8) company, ZIPCODE, COUNT(pubSubscriptionID) as counts, MIN(pubSubscriptionID) as minpubSubscriptionID
		from PubSubscriptions with (NOLOCK)
		where PubID = @pubID and ISNULL(company,'') <> '' and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> ''  and ISNULL(ZIPCODE,'') <> ''
		group by left(FIRSTNAME,3),left(LASTNAME,6),left(company,8), ZIPCODE
		having COUNT(*) > 1
		)
		x 
		join PubSubscriptions s WITH (nolock) on 
					LEFT(s.FIRSTNAME ,3) = x.FIRSTNAME and 
					LEFT(s.LASTNAME,6)  = x.LASTNAME and 
					LEFT(s.company,8) = x.company  and
					s.ZIPCODE = x.ZIPCODE
		where  s.PubID = @pubID and  ISNULL(s.company,'') <> '' and ISNULL(s.FIRSTNAME,'') <> '' and ISNULL(s.LASTNAME,'') <> '' and ISNULL(s.ZIPCODE,'') <> ''
		order by minpubSubscriptionID		
	End			
	else if @loopcounter = 12 -- COMPLETE FIRSTNAME,LASTNAME,COMPANY, ZIPCODE
	Begin
		insert into #curtable	
		select minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select FIRSTNAME,LASTNAME,company, ZIPCODE, COUNT(pubSubscriptionID) as counts, MIN(pubSubscriptionID) as minpubSubscriptionID
		from PubSubscriptions with (NOLOCK)
		where PubID = @pubID and ISNULL(company,'') <> '' and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> '' and ISNULL(ZIPCODE,'') <> ''
		group by FIRSTNAME,LASTNAME,company,ZIPCODE
		having COUNT(*) > 1
		)
		x 
		join PubSubscriptions s WITH (nolock) on 
					s.FIRSTNAME = x.FIRSTNAME and 
					s.LASTNAME = x.LASTNAME and 
					s.company = x.company  and
					s.ZIPCODE = x.ZIPCODE
		where s.PubID = @pubID and  ISNULL(s.company,'') <> '' and ISNULL(s.FIRSTNAME,'') <> '' and ISNULL(s.LASTNAME,'') <> '' and ISNULL(s.ZIPCODE,'') <> ''
		order by minpubSubscriptionID			
	End			
	else if @loopcounter = 13 -- MATCH FIELDS FIRSTNAME,LASTNAME,EMAIL
	Begin
		insert into #curtable	
		select minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select LEFT(FIRSTNAME,3) FIRSTNAME,LEFT(LASTNAME,6) LASTNAME,Email, COUNT(pubSubscriptionID) as counts, MIN(pubSubscriptionID) as minpubSubscriptionID
		from PubSubscriptions  with (NOLOCK)
		where PubID = @pubID and ISNULL(email,'') <> '' and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> ''
		group by LEFT(FIRSTNAME,3),LEFT(LASTNAME,6),EMAIL
		having COUNT(*) > 1
		)
		x 
		join PubSubscriptions s WITH (nolock) on 
					LEFT(s.FIRSTNAME ,3) = x.FIRSTNAME and 
					LEFT(s.LASTNAME,6)  = x.LASTNAME and 
					s.EMAIL = x.EMAIL
		where s.PubID = @pubID and ISNULL(s.email,'') <> '' and ISNULL(s.FIRSTNAME,'') <> '' and ISNULL(s.LASTNAME,'') <> ''
		order by minpubSubscriptionID		
	End			
	else if @loopcounter = 14 -- COMPLETE FIRSTNAME,LASTNAME,PHONE
	Begin
		insert into #curtable	
		select minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select ISNULL(FIRSTNAME,'') FIRSTNAME, ISNULL(LASTNAME,'') LASTNAME, ISNULL(phone,'') phone, COUNT(pubSubscriptionID) as counts, MIN(pubSubscriptionID) as minpubSubscriptionID
		from PubSubscriptions with (NOLOCK)
		where PubID = @pubID and ISNULL(phone,'') <> '' and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> ''
		group by FIRSTNAME,LASTNAME,phone
		having COUNT(*) > 1
		)
		x 
		join PubSubscriptions s WITH (nolock) on 
					ISNULL(s.FIRSTNAME,'')  = x.FIRSTNAME and 
					ISNULL(s.LASTNAME,'')   = x.LASTNAME and 
					ISNULL(s.phone,'')  = x.phone
		where s.PubID = @pubID and  ISNULL(s.phone,'') <> '' and ISNULL(s.FIRSTNAME,'') <> '' and ISNULL(s.LASTNAME,'') <> ''
		order by minpubSubscriptionID		
	End			
	else if @loopcounter = 15 -- MATCH FIELDS Partial FIRSTNAME,LASTNAME,PHONE
	Begin
		insert into #curtable	
		select minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
		select left(FIRSTNAME,3) FIRSTNAME,left(LASTNAME,6) LASTNAME,phone , COUNT(pubSubscriptionID) as counts, MIN(pubSubscriptionID) as minpubSubscriptionID
		from PubSubscriptions with (NOLOCK)
		where PubID = @pubID and  ISNULL(phone,'') <> '' and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> ''
		group by left(FIRSTNAME,3),left(LASTNAME,6),phone
		having COUNT(*) > 1 
		)
		x 
		join PubSubscriptions s WITH (nolock) on   
					left(ISNULL(s.FIRSTNAME,''),3)  = x.FIRSTNAME and 
					left(ISNULL(s.LASTNAME,''),6)  = x.LASTNAME and 
					ISNULL(s.phone,'')  = x.phone
		where s.PubID = @pubID and  ISNULL(s.phone,'') <> '' and ISNULL(s.FIRSTNAME,'') <> '' and ISNULL(s.LASTNAME,'') <> ''
		order by minpubSubscriptionID		
	End			
	else if @loopcounter = 16 -- EMAIL ONLY PROFILES MATCHING WITH ANOTHER PROFILE WITH FIRSTNAME/LASTNAME MASTER EMAIL ONLY
	Begin
		insert into #curtable	
		select  minpubSubscriptionID, s.pubSubscriptionID, DENSE_RANK() OVER (order BY minpubSubscriptionID) AS RowNumber
		from
		(
			select FIRSTNAME,LASTNAME,email, pubSubscriptionID as minpubSubscriptionID from PubSubscriptions with (NOLOCK) 
			where PubID = @pubID and ISNULL(FIRSTNAME,'') <> '' and ISNULL(LASTNAME,'') <> '' and ISNULL(email,'') <> ''
		)
		x 
		join PubSubscriptions s WITH (nolock) on 
					s.EMAIL = x.email
		where s.PubID = @pubID and ISNULL(s.FIRSTNAME,'') = '' and ISNULL(s.LASTNAME,'') = '' and ISNULL(s.email,'') <> ''
		order by minpubSubscriptionID		
	End	

***************************************** END ****************************************************************************/

	
	declare @distinctcount int
	
	select @distinctcount = COUNT(distinct KeepPubSubscriptionID) from #curtable
	
	print '===================================================================='
	--print '================== Database : ' + DB_Name()  + '==================='
	print '================== Loop condtion : ' + Convert(varchar(100), @loopcounter) + ' / '+ convert(varchar(20), getdate(), 114) + '==================='
	print '================== Dupes : ' + Convert(varchar(100), @distinctcount) + ' / '+ convert(varchar(20), getdate(), 114) +  '==================='
	print '===================================================================='
	
	insert into @tmp_dupes 
	select rnk, @loopcounter as loop, KeepPubSubscriptionID, DupePubSubscriptionID from #curtable
	where 
			DupePubSubscriptionID not in (select DupePubSubscriptionID from @tmp_dupes)

	Set @i = 1

	set @loopcounter = @loopcounter + 1

End

drop table #curtable
drop table #MatchGroups


IF EXISTS (SELECT 1 FROM Sysobjects where name = 'tmp_GetUADCircDups')
	DROP table tmp_GetUADCircDups

SELECT *
into tmp_GetUADCircDups
FROM
(
select Id, loop as matchno, s.SubscriptionID, s.PubSubscriptionID, @pubcode as PubCode, ss.igrp_no, s.SequenceID, s.FIRSTNAME, s.LASTNAME, s.TITLE, s.COMPANY, s.ADDRESS1, s.CITY, s.REGIONCODE, s.ZIPCODE, s.COUNTRY, s.EMAIL, s.phone, 
cc.CategoryCodeValue as CategoryID, tc.transactioncodevalue as TransactionID,
isnull(s.DateUpdated, s.DateCreated) as lastchanged, s.QualificationDate, c.CodeValue as QualificationSource
 --, ''keep'' as comments
from @tmp_dupes t join PubSubscriptions s with (NOLOCK) on t.KeepPubSubscriptionID = s.PubSubscriptionID join Subscriptions ss  with (NOLOCK) on s.SubscriptionID = ss.SubscriptionID
left outer join UAD_Lookup..Code c on c.CodeID = s.PubQSourceID 
left outer join UAD_Lookup..CategoryCode cc on cc.CategoryCodeID = s.PubCategoryID 
left outer join UAD_Lookup..TransactionCode tc on tc.TransactionCodeID = s.pubTransactionID
where KeepPubSubscriptionID <> DupePubSubscriptionID
union
select Id, loop as matchno,  s.SubscriptionID, s.PubSubscriptionID, @pubcode as PubCode, ss.igrp_no, s.SequenceID, s.FIRSTNAME, s.LASTNAME, s.TITLE, s.COMPANY, s.ADDRESS1, s.CITY, s.REGIONCODE, s.ZIPCODE, s.COUNTRY, s.EMAIL, s.phone, 
cc.CategoryCodeValue, tc.transactioncodevalue,
isnull(s.DateUpdated, s.DateCreated) as lastchanged, s.QualificationDate, c.CodeValue as QualificationSource
 --, ''keep'' as comments
from @tmp_dupes t join PubSubscriptions s  with (NOLOCK) on t.DupePubSubscriptionID = s.PubSubscriptionID join Subscriptions ss  with (NOLOCK) on s.SubscriptionID = ss.SubscriptionID
left outer join UAD_Lookup..Code c on c.CodeID = s.PubQSourceID 
left outer join UAD_Lookup..CategoryCode cc on cc.CategoryCodeID = s.PubCategoryID 
left outer join UAD_Lookup..TransactionCode tc on tc.TransactionCodeID = s.pubTransactionID
where KeepPubSubscriptionID <> DupePubSubscriptionID
) x
order by matchno, ID desc


print '================== Total Dupes : ' + Convert(varchar(100), @@ROWCOUNT) + ' / '+ convert(varchar(20), getdate(), 114) +  '==================='


