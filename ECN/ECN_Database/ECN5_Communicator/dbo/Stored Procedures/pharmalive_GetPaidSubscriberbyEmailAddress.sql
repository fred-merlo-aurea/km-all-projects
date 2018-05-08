CREATE  proc [dbo].[pharmalive_GetPaidSubscriberbyEmailAddress]
(
	@emailaddress varchar(100)
)
as
Begin
	select * from 
	(
		select	edv.emailID, g.groupID, g.groupname, entryID,
				max(case when shortname='startdate' then datavalue end) as startdate,
				max(case when shortname='enddate' then datavalue end) as enddate,
				max(case when shortname='amountpaid' then datavalue end) as amountpaid,
				max(case when shortname='earnedamount' then datavalue end) as earnedamount,
				max(case when shortname='Deferredamount' then datavalue end) as Deferredamount,
				max(case when shortname='TotalSent' then datavalue end) as TotalSent,
				max(case when shortname='PaymentMethod' then datavalue end) as PaymentMethod,
				max(case when shortname='CardType' then datavalue end) as CardType,
				max(case when shortname='CardNumber' then datavalue end) as CardNumber,
				(select datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = g.groupID and shortname ='paidorfree') as PaidOrFree
		from	emails e join emailgroups eg on e.emailID = eg.emailID join
				groups g on g.groupID = eg.groupID join
				groupdatafields gdf on g.groupID = gdf.groupID join		
				emaildatavalues edv on edv.emailID = eg.emailID and edv.groupdatafieldsID = gdf.groupdatafieldsID
		where 
				e.customerID = 2069 and
				e.emailaddress = @emailaddress and
				g.groupID in (select GroupID from ecn_misc..CANON_PAIDPUB_eNewsletters where CustomerID = 2069) --) and 
				and entryID is not null and eg.subscribetypecode = 'S'
		group by 
				e.emailaddress,
				edv.emailID, 
				g.groupID, 
				g.groupname, 
				entryID
	) inn 
	where 
			--paidorfree = 'PAID' and 
			isnull(convert(decimal,amountpaid),0) > 0
	order by 
			inn.groupname
end
