--2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE Procedure [dbo].[e_EmailActivityLog_InsertBounce](  
 @xmlDocument varchar(MAX),
 @defaultThreshold int,
 @source varchar(200) = 'e_EmailActivityLog_InsertBounce'   
)    
AS   
BEGIN  
    SET NOCOUNT ON    
                  
    DECLARE @docHandle int    
                
    SET @xmlDocument = master.dbo.fn_npclean_string(@xmlDocument)
                  
    EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument

    CREATE TABLE #EmailGroupUpdate(EmailGroupID int, STC varchar(1))

	CREATE INDEX idx_temp_EmailGroupUpdate_EmailgroupID on #EmailGroupUpdate (EmailGroupID)

    CREATE TABLE #e
    (  
    EmailID int,  
    BlastID int,
    ActionNotes varchar(500),
    ActionValue varchar(255),
    customerID int,  
    mastergroupID int, 
    threshold int,
    bounceScore int,
    softbouncethreshold int,
    softbouncescore int,
    softbounceActionNotes varchar(500),
	ReceivedDate datetime
    )    

    CREATE INDEX idx_temp_Email on #e (EmailId,BlastId)
    CREATE INDEX idx_temp_Email_EmailId on #e (EmailId)

    INSERT INTO #e
    SELECT 
			e.EmailID, 
			BlastID, 
			ActionNotes, 
			ActionValue, 
			e.customerID, 
			g.GroupID, 
			(case when ISNULL(c.BounceThreshold,0) > 0 then c.BounceThreshold else (case when ISNULL(bc.BounceThreshold,0) > 0 then bc.BounceThreshold else @defaultThreshold end) end), 
			NULL, 
			(case when c.SoftBounceThreshold is not null then c.SoftBounceThreshold else (case when bc.SoftBounceThreshold is not null then bc.SoftBounceThreshold else null end) end),
			NULL, '',
			ISNULL(ReceivedDate, GETDATE())
	FROM 
			OPENXML(@docHandle, N'/ROOT/BOUNCE') 
	WITH 
	(
			EmailID INT '@EmailID', 
			BlastID INT '@BlastID', 
			ActionValue varchar(255) '@BounceWeight',
			ActionNotes varchar(500) '@Signature',
			ReceivedDate datetime '@ReceivedDate'
	) inn join emails e WITH(NOLOCK) on e.emailID = inn.emailID 
				join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c WITH(NOLOCK) on e.CustomerID = c.CustomerID 
				join [ECN5_ACCOUNTS].[DBO].[BASECHANNEL] bc WITH(NOLOCK) on c.BaseChannelID = bc.BaseChannelID 
				left join groups g WITH(NOLOCK) on e.customerID = g.customerID  and g.MasterSupression = 1                
                  
	EXEC sp_xml_removedocument @docHandle
                
	/* delete if already exists */ -- added by Sunil on 07/05/2015
	Delete e
	from #E e join ECN_ACTIVITY..BlastActivityBounces b on e.EmailID = b.EmailID and e.BlastID = b.BlastID
                
	Delete e
	from #E e join EmailActivityLog eal on e.EmailID = eal.EmailID and e.BlastID = eal.BlastID and ActionTypeCode ='bounce'
                
	update #E
	set 
			ActionValue = 'blocks'
	WHERE 
			ActionValue in ('hard','hardbounce','softbounce')            and
			(
			ActionNotes like '%block%' or
			ActionNotes like '%banned%' or
			ActionNotes like '%blacklist%' or
			ActionNotes like '%access denied%' or
			ActionNotes like '%transaction failed%' or
			ActionNotes like '%permanently deferred%' or
			--ActionNotes like '%rejected%' or
			--ActionNotes like '%delisted%' or
			ActionNotes like '%mail refused%' or
			ActionNotes like '%you are not allowed to%' or
			ActionNotes like '%not authorized%' or
			ActionNotes like '%found on one or more dnsbls see%' or
			ActionNotes like '%rbl restriction%' or
			ActionNotes like '%poor reputation%'  or
			ActionNotes like '%rejected due to spam%'or
			ActionNotes like '%http://postmaster.info.aol.com/errors/554rlyb1.html%' or
			ActionNotes like '%http://postmaster.info.aol.com/errors/554conb1.html%' or
			ActionNotes like '%http://postmaster.info.aol.com/errors/421dynt1.html%' or
			ActionNotes like '%invalid recipient - refer to error codes section at http://postmaster.cox.net/confluence/display/postmaster/error%' or
			ActionNotes like '%aol.com esmtp not accepting connections%' or
			ActionNotes like '%barracuda%' or
			ActionNotes like '%mxlogic.net%'
			) and ActionNotes not like '%bad-mailbox%'--adding this so hardbounces that come in don't get marked as blocks when bad-mailbox is in the signature JWelter 04182017
                
	update 
			#E
	set 
			ActionValue = 'hardbounce'
	WHERE 
			ActionValue ='softbounce'          
			and        (ActionNotes like '%cannot be delivered. this account has been disabled or discontinued%' )
                
	insert into Emailactivitylog (
			EmailID, 
			BlastID, 
			ActionTypeCode, 
			ActionDate, 
			ActionValue, 
			ActionNotes, 
			Processed)     
	SELECT 
			EmailID, 
			BlastID, 
			'bounce', 
			ReceivedDate, 
			ActionValue, 
			ActionNotes, 
			'Y' 
	FROM 
			#E   

	update 
			emails
	set 
			bouncescore = ISNULL(bouncescore,0) + inn.bcount
	from 
			emails WITH (NOLOCK) 
			join 
	(
			SELECT EmailID, count(EmailID) as bcount 
			FROM #E 
			where ActionValue in ('hard','hardbounce','dnserror') 
			group by EmailID
	) inn on emails.emailID = inn.emailID

	update 
			emails
	set 
			softbouncescore = ISNULL(softbouncescore,0) + inn.bcount
	from 
			emails WITH (NOLOCK) 
			join 
	(
			SELECT EmailID, count(EmailID) as bcount 
			FROM #E 
			where ActionValue in ('softbounce') 
			group by EmailID
	) inn on emails.emailID = inn.emailID
                
	--handle autoresponder with code 5.1.1
	if exists(select top 1 EmailID from #E e1 where e1.ActionValue = 'autoresponder' and e1.ActionNotes like '%5.1.1%')
	begin
        insert into #EmailGroupUpdate(EmailGroupID,STC)
        SELECT EMailGroupID, case when eg.GroupID = e1.mastergroupID then 'B' else 'M' end 
        from EmailGroups eg with(nolock)
        join #E e1 on eg.emailid = e1.EmailID
        where
                        e1.ActionValue = 'autoresponder' 
                        and e1.ActionNotes like '%5.1.1%'

                        --COMMENTING OUT FOR NOW JWelter 3/8/2017
        --update 
        --             emailgroups 
        --set 
        --             SubscribeTypeCode  = case when eg.GroupID = e1.mastergroupID then 'B' else 'M' end, 
        --             LastChanged = getdate(), LastChangedSource = @source 
        ----WGH - Fix 7/17/2014 was: update emailgroups set SubscribeTypeCode  = case when eg.EmailGroupID = e1.mastergroupID then 'B' else 'M' end, LastChanged = getdate() 
        --from  
        --             emailgroups eg WITH (NOLOCK)
        --             join #E e1 on e1.emailID = eg.emailID
        --where 
        --             e1.ActionValue = 'autoresponder' 
        --             and e1.ActionNotes like '%5.1.1%'
                                
        -- insert emailgroups if master suppression not exists  
        insert into EmailGroups (
                        EmailID, 
                        GroupID, 
                        FormatTypeCode, 
                        SubscribeTypeCode, 
                        CreatedOn, 
                        LastChanged,
                        CreatedSource)  
        select 
                        distinct e1.EmailID, 
                        e1.mastergroupID, 
                        'html', 
                        'B', 
                        getdate(), 
                        getdate(), 
                        @source 
        from 
                        #E e1 
                        left outer join emailgroups eg  WITH (NOLOCK) on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID  
        where 
                        eg.emailgroupID is null 
                        and e1.ActionValue = 'autoresponder' 
                        and e1.ActionNotes like '%5.1.1%'
                                
        -- insert emailActivityLog if not exists  
        insert into Emailactivitylog (
                        EmailID, 
                        BlastID, 
                        ActionTypeCode, 
                        ActionDate, 
                        ActionValue, 
                        ActionNotes, 
                        Processed)   
        select  
                        distinct e1.EmailID, 
                        e1.BlastID, 
                        'MASTSUP_UNSUB', 
                        ReceivedDate, 
                        'U', 
                        e1.ActionNotes, 
                        'n'    
        from  
                        #E e1  
        where 
                        e1.ActionValue = 'autoresponder' 
                        and e1.ActionNotes like '%5.1.1%'
                
    end
                                
    --get the bounce score and create comment
    update
                    #E   
    set 
                    BounceScore = e.BounceScore, softbouncescore = e.softbouncescore,
                    ActionNotes = 'AUTO MASTERSUPPRESSED: Bounce Score is ' + CONVERT(varchar,e.BounceScore) + '. Bounce Threshold is ' + CONVERT(varchar,(e1.threshold - 1)),
                    softbounceActionNotes = 'AUTO MASTERSUPPRESSED: Soft Bounce Score is ' + CONVERT(varchar,e.SoftBounceScore) + '. Soft Bounce Threshold is ' + CONVERT(varchar,(isnull(e1.softbouncethreshold,0) - 1))
    from 
                    #E e1 
                    join emails e WITH(NOLOCK) on e1.emailID = e.emailID

    --all of the below used to be e1.bounceScore > e1.threshold and we have changed to >=
    if Exists (select top 1 EmailID from #E e1 where e1.bounceScore >= e1.threshold)
    Begin
        -- update emailgroups for existing records  
        -- for Mastersuppersiongroup  = 'S'  
        -- for other groups = 'U' 

        insert into #EmailGroupUpdate(EmailGroupID,STC)
        SELECT EMailGroupID, case when eg.GroupID = e1.mastergroupID then 'B' else 'M' end 
        from EmailGroups eg with(nolock)
        join #E e1 on eg.emailid = e1.EmailID
        where 
                        e1.bounceScore >= e1.threshold 
                        and eg.SubscribeTypeCode <> 'U'

                        --COMMENTING OUT FOR NOW JWelter 3/8/2017
        --update 
        --             emailgroups 
        --set 
        --             SubscribeTypeCode  = case when eg.GroupID = e1.mastergroupID then 'B' else 'M' end, 
        --             LastChanged = getdate(), LastChangedSource = @source
        ----WGH - Fix 7/17/2014 was: update emailgroups set SubscribeTypeCode  = case when eg.EmailGroupID = e1.mastergroupID then 'B' else 'M' end, LastChanged = getdate()
        --from  
        --             emailgroups eg  WITH (NOLOCK)
        --             join #E e1 on e1.emailID = eg.emailID
        --where 
        --             e1.bounceScore >= e1.threshold 
        --             and eg.SubscribeTypeCode <> 'U'
                                
        -- insert emailgroups if master suppression not exists  
        insert into EmailGroups (
                        EmailID, 
                        GroupID, 
                        FormatTypeCode, 
                        SubscribeTypeCode, 
                        CreatedOn, 
                        LastChanged,
                        CreatedSource)  
        select 
                        distinct e1.EmailID, 
                        e1.mastergroupID, 
                        'html', 
                        'B', 
                        getdate(), 
                        getdate(),
                        @source 
        from 
                        #E e1 
                        left outer join emailgroups eg  WITH (NOLOCK) on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID  
        where 
                        eg.emailgroupID is null 
                        and e1.mastergroupID is not null 
                        and e1.bounceScore >= e1.threshold
                                
        -- insert emailActivityLog if not exists  
        insert into Emailactivitylog (
                        EmailID, 
                        BlastID, 
                        ActionTypeCode, 
                        ActionDate, 
                        ActionValue, 
                        ActionNotes, 
                        Processed)   
        select 
                        distinct e1.EmailID, 
                        e1.BlastID, 
                        'MASTSUP_UNSUB', 
                        ReceivedDate, 
                        'U', 
                        e1.ActionNotes, 
                        'n'    
        from  
                        #E e1 
        where 
                        e1.bounceScore >= e1.threshold
    End   
                
    --softbounce master suppress
    if Exists (select top 1 EmailID from #E e1 where e1.softbouncethreshold is not null and e1.softbounceScore >= e1.softbouncethreshold)
    Begin
        -- update emailgroups for existing records  
        -- for Mastersuppersiongroup  = 'S'  
        -- for other groups = 'U' 

        insert into #EmailGroupUpdate(EmailGroupID,STC)
        SELECT EMailGroupID, case when eg.GroupID = e1.mastergroupID then 'B' else 'M' end 
        from EmailGroups eg with(nolock)
        join #E e1 on eg.emailid = e1.EmailID
        where 
                        e1.softbouncethreshold is not null
                        and e1.softbounceScore >= e1.softbouncethreshold 
                        and eg.SubscribeTypeCode <> 'U'

                        --COMMENTING OUT FOR NOW JWelter 3/8/2017
        --update 
        --             emailgroups 
        --set 
        --             SubscribeTypeCode  = case when eg.GroupID = e1.mastergroupID then 'B' else 'M' end, 
        --             LastChanged = getdate(), LastChangedSource = @source
        ----WGH - Fix 7/17/2014 was: update emailgroups set SubscribeTypeCode  = case when eg.EmailGroupID = e1.mastergroupID then 'B' else 'M' end, LastChanged = getdate()
        --from  
        --             emailgroups eg  WITH (NOLOCK)
        --             join #E e1 on e1.emailID = eg.emailID
        --where 
        --             e1.softbouncethreshold is not null
        --             and e1.softbounceScore >= e1.softbouncethreshold 
        --             and eg.SubscribeTypeCode <> 'U'
                                
        -- insert emailgroups if master suppression not exists  
        insert into EmailGroups (
                        EmailID, 
                        GroupID, 
                        FormatTypeCode, 
                        SubscribeTypeCode, 
                        CreatedOn, 
                        LastChanged,
                        CreatedSource)  
        select 
                        distinct e1.EmailID, 
                        e1.mastergroupID, 
                        'html', 
                        'B', 
                        getdate(), 
                        getdate(),
                        @source
        from 
                        #E e1 
                        left outer join emailgroups eg  WITH (NOLOCK) on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID  
        where 
                        eg.emailgroupID is null 
                        and e1.mastergroupID is not null 
                        and e1.softbouncethreshold is not null
                        and e1.softbounceScore >= e1.softbouncethreshold 
                                
        -- insert emailActivityLog
        insert into Emailactivitylog (
                        EmailID, 
                        BlastID, 
                        ActionTypeCode, 
                        ActionDate, 
                        ActionValue, 
                        ActionNotes, 
                        Processed)   
        select 
                        distinct e1.EmailID, 
                        e1.BlastID, 
                        'MASTSUP_UNSUB', 
                        ReceivedDate, 
                        'U', 
                        e1.softbounceActionNotes, 
                        'n'    
        from  
                        #E e1 
        where 
                        e1.softbouncethreshold is not null
                        and e1.softbounceScore >= e1.softbouncethreshold 
    End 


    if exists (select top 1 1 from #EmailGroupUpdate)
    BEGIN
                    --DO THE UPDATE

                    update eg
                    set eg.SubscribeTypeCode = egu.STC, eg.LastChanged = GETDATE(), eg.LastChangedSource = @source
                    FROM EMailGroups eg
                    join #EmailGroupUpdate egu with(nolock) on eg.EmailGroupID = egu.EmailGroupID
    END

	drop table #EmailGroupUpdate                
	drop table #e
                
END

