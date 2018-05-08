CREATE proc [dbo].[spDataRefreshPart4]
as
BEGIN

	SET NOCOUNT ON    

	select CodeSheetID,PubID,ResponseGroup,  
		(case when LEN(responsevalue)= 1 and ISNUMERIC(responsevalue) = 1 then '0' + Responsevalue else responsevalue end) as Responsevalue, 
		Responsedesc,ResponseGroupID
	into #Tmp_Codesheet
	from codesheet
	
	CREATE INDEX IDX_tmpcodesheet ON #Tmp_Codesheet(pubID,responsegroup,responsevalue )

	insert into subscriptiondetails
	select distinct subscriptionID, cb.masterID 
	from IncomingDataDetails idata 
		join #Tmp_Codesheet c on c.pubID = idata.pubID and c.responsegroup = idata.responsegroup and c.responsevalue = idata.responsevalue 
		join CodeSheet_Mastercodesheet_Bridge cb on cb.CodeSheetID = c.CodeSheetID
	drop table #Tmp_Codesheet

	--insert into igrpdetails
	--select distinct igrp_no, masterID from subscriptions s join subscriptiondetails sd on s.subscriptionID = sd.subscriptionID
	--order by igrp_no, masterID
	
	---------------------------------------------------------------------------------------------------------------
END