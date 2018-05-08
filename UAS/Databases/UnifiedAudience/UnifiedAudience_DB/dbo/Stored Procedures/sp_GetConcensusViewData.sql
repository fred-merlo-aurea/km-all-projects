CREATE PROCEDURE [dbo].[sp_GetConcensusViewData]
(
	@SubscriptionIdCollection XML,
	@PubId INT
)
as
BEGIN
	
	SET NOCOUNT ON

	declare @rgcolumns varchar(4000)
	select @rgcolumns = COALESCE(@rgcolumns+',' ,'') + '['+ responsegroupname + ']' 
	from responsegroups 
	WHERE ResponseGroupID in (SELECT ResponseGroupID FROM ResponseGroups WHERE PubID = @PubId)
				
	IF OBJECT_ID('tempdb..#tmpSubscriptionID') IS NOT NULL 
	BEGIN 
		DROP TABLE #tmpSubscriptionID;
	END 
	      
	CREATE TABLE #tmpSubscriptionID (subsID int); 

	insert into #tmpSubscriptionID
		SELECT tempTable.T.value('.', 'INT') AS SubscriptionId FROM @SubscriptionIDCollection.nodes('/ArrayOfInt/int/text()') tempTable(T);	

	select subscriptionID, rg.ResponseGroupName,  c.Responsevalue 
	into #tmpPubSubscriptionDetails 
	from PubSubscriptionDetail psd 
		join CodeSheet c on psd.CodesheetID = c.CodeSheetID 
		join ResponseGroups rg on rg.ResponseGroupID = c.ResponseGroupID
		join #tmpSubscriptionID t on t.subsID = psd.SubscriptionID
	where rg.PubID = @pubID

	if (len(@rgcolumns) = 0 or @rgcolumns is null)
		Begin
			exec ('select s.SubscriptionID
						 ,s.FNAME
						 ,s.LNAME
						 ,s.TITLE
						 ,s.COMPANY
						 ,s.ADDRESS
						 ,s.MAILSTOP
						 ,s.CITY
						 ,s.STATE
						 ,s.ZIP
						 ,s.PLUS4
						 ,s.FORZIP
						 ,s.COUNTY
						 ,s.COUNTRY
						 ,s.PHONE
						 ,s.FAX
						 ,s.FaxExists
						 ,s.EMAIL
						 ,s.EMPLOY
						 ,s.EmailExists
						 ,s.CategoryID
						 ,s.CGRP_NO
						 ,s.TransactionID
						 ,s.TransactionDATE
						 ,s.QDate
						 ,s.QSourceID
						 ,s.REGCODE
						 ,s.Score
						 ,s.SUBSRC
						 ,s.Demo31
						 ,s.Demo32
						 ,s.Demo33
						 ,s.Demo34
						 ,s.Demo35
						 ,s.Demo36
						 ,s.Demo7
						 ,s.mobile
						 ,s.PAR3C
						 ,s.InSuppression 
				from subscriptions s 
				left outer join  QSource q 	on q.QSourceID = s.QSourceID 
				left outer join [Transaction] tr on tr.TransactionID =  s.TransactionID 	
				join #tmpSubscriptionID t on s.subscriptionID = t.subsID')
		End
	Else
		Begin
			exec ('
				select  s.SubscriptionID
						,s.FNAME
						,s.LNAME
						,s.TITLE
						,s.COMPANY
						,s.ADDRESS
						,s.MAILSTOP
						,s.CITY
						,s.STATE
						,s.ZIP
						,s.PLUS4
						,s.FORZIP
						,s.COUNTY
						,s.COUNTRY
						,s.PHONE
						,s.FAX
						,s.FaxExists
						,s.EMAIL
						,s.EMPLOY
						,s.EmailExists
						,s.CategoryID
						,s.CGRP_NO
						,s.TransactionID
						,s.TransactionDATE
						,s.QDate
						,s.QSourceID
						,s.REGCODE
						,s.Score
						,s.SUBSRC
						,s.Demo31
						,s.Demo32
						,s.Demo33
						,s.Demo34
						,s.Demo35
						,s.Demo36
						,s.Demo7
						,s.mobile
						,s.PAR3C
						,s.InSuppression
						,demos.* 
				from subscriptions s 
				left outer join  QSource q 	on q.QSourceID = s.QSourceID 
				left outer join [Transaction] tr on tr.TransactionID =  s.TransactionID 	
				join #tmpSubscriptionID t on s.subscriptionID = t.subsID left outer join
			 (
			 SELECT * 
			 FROM
			 (
				SELECT 
					  [subscriptionID], ResponseGroupName,
					  STUFF((
						SELECT '', '' + CAST([Responsevalue] AS VARCHAR(MAX)) 
						FROM #tmpPubSubscriptionDetails 
						WHERE ([subscriptionID] = Results.[subscriptionID] and ResponseGroupName= Results.ResponseGroupName) 
						FOR XML PATH (''''))
					  ,1,2,'''') AS CombinedValues
					FROM #tmpPubSubscriptionDetails Results
					GROUP BY [subscriptionID], ResponseGroupName
			 ) u
			 PIVOT
			 (
			 MAX (CombinedValues)
			 FOR ResponseGroupName in (' + @rgcolumns + ')) as pvt
			 ) demos on demos.subscriptionID = s.subscriptionID
			')
		End
	
	DROP TABLE #tmpSubscriptionID;
	DROP TABLE #tmpPubSubscriptionDetails

End