create proc ccp_Vcast_ClinicHome
@srcfile varchar(25) = '',
@processcode varchar(50) = '',
@clientid varchar(5) = ''
as
begin
set nocount on
 
DECLARE @TempPubSubIDs TABLE (PubSubscriptionID int, unique (PubSubscriptionID))

/* FIND ALL CLINIC MATCHES */
/*
 * Company matches Clinic or Hosp or Hospital but not values like Hospit or Hospitality
 * Blank Company with 3+ addresses that match
 * Blank Company with 3+ phones that match
 */
Insert into @TempPubSubIDs
Select PubSubscriptionID from PubSubscriptions WITH(NOLOCK) where Company like '%clinic%' or Company like '%HOSPITAL %' or Company like '% HOSPITAL' or Company like '%HOSP' or Company like '%HOSP %'
UNION
Select PubSubscriptionID from PubSubscriptions ps WITH(NOLOCK)
join 
(
	Select Address1, ZipCode
	from
	(
		Select pso.SubscriptionID, pso.Address1, pso.ZipCode
		from PubSubscriptions pso WITH(NOLOCK)
		join
		(
			   Select Address1, ZipCode
			   from PubSubscriptions WITH(NOLOCK) 
			   where ISNULL(Company,'') = '' and ISNULL(Address1,'') <> '' and ISNULL(ZipCode,'') <> ''
			   group by Address1, ZipCode   
		) psi on pso.Address1 = psi.Address1 and pso.ZipCode = psi.ZipCode
		group by pso.SubscriptionID, pso.Address1, pso.ZipCode
	) a
	group by Address1, ZipCode
	having Count(*) > 2	
) ps2 on ps.Address1 = ps2.Address1 and ps.ZipCode = ps2.ZipCode
where ISNULL(ps.Company,'') = ''
UNION
Select PubSubscriptionID from PubSubscriptions ps WITH(NOLOCK)
join 
(
	Select Phone
	from
	(
		Select pso.SubscriptionID, pso.Phone
		from PubSubscriptions pso WITH(NOLOCK)
		join
		(
			   Select Phone
			   from PubSubscriptions WITH(NOLOCK) 
			   where ISNULL(Company,'') = '' and ISNULL(Phone,'') <> ''
			   group by Phone	   
		) psi on pso.Phone = psi.Phone 
		group by pso.SubscriptionID, pso.Phone
	) a
	where Phone not in ('#NAME?', 'DONOTCALL')
	group by Phone
	having Count(*) > 2
) ps2 on ps.Phone = ps2.Phone
where ISNULL(ps.Company,'') = ''

----delete old clinic home
delete psd 
from PubSubscriptionDetail psd with(nolock)
	join CodeSheet c with(nolock) on c.CodeSheetID = psd.CodesheetID 
	join ResponseGroups rg with(nolock) on rg.ResponseGroupID = c.ResponseGroupID 
Where 
	rg.ResponseGroupName = 'CLINIC_HOME'

----Insert clinic
insert into PubSubscriptionDetail
Select distinct t.PubSubscriptionID, ps.SubscriptionID,CodeSheetID,GETDATE(),null,1,null,null 
from PubSubscriptions ps with(nolock)
	join 
	(
		SELECT distinct t1.PubSubscriptionID
		FROM PubSubscriptions  t1 with(nolock)
			JOIN @TempPubSubIDs t2 ON t2.PubSubscriptionID = t1.PubSubscriptionID
	) as t on t.PubSubscriptionID = ps.PubSubscriptionID 
	join ResponseGroups rg with(nolock) on ps.PubID =rg.PubID
	join CodeSheet c with(nolock) on rg.ResponseGroupID = c.ResponseGroupID 
Where 
	rg.ResponseGroupName = 'CLINIC_HOME' and c.responsevalue = 'C'

----Insert Home 
insert into PubSubscriptionDetail
Select distinct t.PubSubscriptionID, ps.SubscriptionID,CodeSheetID,GETDATE(),null,1,null,null
from PubSubscriptions ps with(nolock)
	join 
	(
		SELECT distinct t1.PubSubscriptionID
		FROM PubSubscriptions t1 with(nolock)
			left JOIN @TempPubSubIDs t2 ON t2.PubSubscriptionID = t1.PubSubscriptionID
		WHERE t2.PubSubscriptionID IS NULL
	) as t on t.PubSubscriptionID = ps.PubSubscriptionID 
	join ResponseGroups rg with(nolock) on ps.PubID =rg.PubID
	join CodeSheet c with(nolock) on rg.ResponseGroupID = c.ResponseGroupID 
Where 
	rg.ResponseGroupName = 'CLINIC_HOME' and c.responsevalue = 'H'


end
