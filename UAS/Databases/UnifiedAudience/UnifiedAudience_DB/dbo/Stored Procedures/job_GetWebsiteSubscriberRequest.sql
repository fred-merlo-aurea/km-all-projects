CREATE PROCEDURE [job_GetWebsiteSubscriberRequest]
(
@PubID int
)
as
BEGIN   

	SET NOCOUNT ON 

	declare @precedence table (PrecedenceID int, status varchar(50))

	insert into @precedence values ('1', 'MasterSuppressed')
	insert into @precedence values ('2', 'Spam')
	insert into @precedence values ('3', 'Bounced')
	insert into @precedence values ('4', 'UnSubscribe')
	insert into @precedence values ('5', 'Invalid')
	insert into @precedence values ('6', 'Unverified')
	insert into @precedence values ('7', 'Active')

	select ps.Email, 
		max(case when isnull(ps.EmailStatusID,0) = 0 then 'Active' else es.[Status] end) as Emailstatus, 
		pubcode as groupID, 
		max(ps.StatusUpdatedDate)
	from PubSubscriptions ps with (NOLOCK) 
		join Pubs p with (NOLOCK) on p.PubID = ps.pubID
		join EmailStatus es with (NOLOCK)  on isnull(ps.EmailStatusID,1) = es.EmailStatusID
	Where p.PubID = @PubID and  ISNULL(ps.email,'') <> ''
	group by ps.Email, PubCode
	having COUNT(distinct es.emailstatusID) = 1
	union all
	select	Email, 
			pd1.status as Emailstatus,
			groupID,
			StatusUpdatedDate
	from
	(
		select ps.Email, 
			MIN(pd.PrecedenceID) as PrecedenceID, 
			pubcode as groupID,
			max(ps.StatusUpdatedDate) as StatusUpdatedDate
		from PubSubscriptions ps with (NOLOCK) 
			join Pubs p with (NOLOCK) on p.PubID = ps.pubID
			join EmailStatus es with (NOLOCK)  on isnull(ps.EmailStatusID,1) = es.EmailStatusID 
			join @precedence pd on pd.status = es.Status
		Where p.PubID = @PubID and   ISNULL(ps.email,'') <> '' 
		group by ps.Email, PubCode
		having COUNT(distinct es.emailstatusID) > 1
	) x 
		join @precedence pd1 on x.PrecedenceID = pd1.PrecedenceID


	--	OLD  logic commented on 5/4/2014 --sunil
	--select 
	--		ps.Email, 
	--		case when isnull(ps.EmailStatusID,0) = 0 then 'Active' else Status end as Emailstatus, 
	--		pubcode as groupID, 
	--		ps.StatusUpdatedDate
	--from	
	--		PubSubscriptions ps with (NOLOCK) join Pubs p with (NOLOCK) on p.PubID = ps.pubID
	--left outer join EmailStatus es on ps.EmailStatusID = es.EmailStatusID
	--Where 
	--	p.PubID = @PubID and ISNULL(ps.email,'') <> ''

End