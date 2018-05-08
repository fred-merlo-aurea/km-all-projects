CREATE PROCEDURE [dbo].[sp_GetSubscriberDataWithCustomColumns]
	-- Add the parameters for the stored procedure here
(
	@SubscriptionID  XML,
	@StandardColumns varchar(4000),
	@CustomColumnDisplayNames varchar(4000)
)
as  
BEGIN

	SET NOCOUNT ON

	declare @query varchar(4000);


	IF OBJECT_ID('tempdb..#TempSubscription') IS NOT NULL 
		BEGIN 
			DROP TABLE #TempSubscription;
		END 
                            
	CREATE TABLE #TempSubscription (subsID int); 

	insert into #TempSubscription
		SELECT SubscriptionID.ID.value('./@SubscriptionID','INT')FROM @SubscriptionID.nodes('/Subscriptions') as SubscriptionID(ID) ;	
			SET  	@query =
					'select * FROM 
					(
						SELECT s.SubscriptionID, ' + @StandardColumns + ', mg.DisplayName, smv.MastercodesheetValues
						from subscriptions s 
							left outer join  QSource q on q.QSourceID = s.QSourceID 
							left outer join [Transaction] t on t.TransactionID =  s.TransactionID 
							left outer join SubscriberMasterValues smv on smv.SubscriptionID = s.SubscriptionID
							join MasterGroups mg on smv.MasterGroupID = mg.MasterGroupID	
						WHERE s.SubscriptionID IN (SELECT subsID FROM #TempSubscription)
					) x
					PIVOT
					(
						max(MastercodesheetValues)
						for DisplayName in (' + @CustomColumnDisplayNames + ')
					) p'

			Exec (@query)	
						
End