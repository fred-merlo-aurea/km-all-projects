
CREATE proc [dbo].[spDataRefreshPart3]
as
BEGIN

	select CodeSheetID,PubID,ResponseGroup,  
		(case when LEN(responsevalue)= 1 and ISNUMERIC(responsevalue) = 1 then '0' + Responsevalue else responsevalue end) as Responsevalue, 
		Responsedesc,ResponseGroupID
	into #Tmp_Codesheet
	from codesheet
	
	CREATE INDEX IDX_tmpcodesheet ON #Tmp_Codesheet(pubID,responsegroup,responsevalue )
	
	update IncomingDataDetails 
		set responsevalue = REPLACE(responsevalue, ' ' ,'')
	
	update IncomingDataDetails 
		set responsevalue =  '0' + responsevalue
		where len(Responsevalue) = 1 and ISNUMERIC(responsevalue) = 1
	
	update IncomingDataDetails 
		set notexists = 0 --where notexists = 1
		where cdid in 
		(
			select cdid
			from IncomingDataDetails idata 
				left outer join #Tmp_Codesheet c on c.pubID = idata.pubID 
					and c.responsegroup = idata.responsegroup and c.responsevalue = idata.responsevalue
			where c.CodeSheetID is not null 
		)

	drop table #Tmp_Codesheet

	select distinct idata.pubID, p.pubname, responsegroup, responsevalue, count(cdid) 
	from IncomingDataDetails idata 
		join Pubs p on idata.pubid = p.PubID
	where isnull(notexists,1) = 1 group by  idata.pubID, p.pubname, responsegroup, responsevalue 
	order by 5 desc
	
	insert into PubSubscriptionDetail (PubSubscriptionID, SubscriptionID, CodeSheetID)
	select   ps.PubSubscriptionID, ps.SubscriptionID, cs.CodeSheetID--, idetail.responsegroup, idetail.responsevalue
	from PubSubscriptions ps 
		join IncomingDataDetails idetail on ps.SubscriptionID = idetail.subscriptionID and ps.PubID = idetail.pubid
		join ResponseGroups rg on idetail.responsegroup = rg.ResponseGroupName and idetail.pubid = rg.PubID
		join CodeSheet cs on rg.ResponseGroupID = cs.ResponseGroupID and rg.PubID = cs.PubID and (case when LEN(idetail.responsevalue)= 1 and ISNUMERIC(idetail.responsevalue) = 1 then '0' + idetail.Responsevalue else idetail.responsevalue end) = (case when LEN(cs.responsevalue)= 1 and ISNUMERIC(cs.responsevalue) = 1 then '0' + cs.Responsevalue else cs.responsevalue end)
		order by ps.subscriptionID, cs.codesheetID
	
	---------------------------------------------------------------------------------------------------------------
END