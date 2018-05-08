CREATE Proc [dbo].[e_Subscription_BulkUpdate_ActionIDs]
(  
	@SubscriptionXML TEXT
) AS

CREATE TABLE #Subs
(  
	RowID int IDENTITY(1, 1)
  ,[SubscriptionID] int
  ,[CurrentAction] int
  ,[PreviousAction] int
)

DECLARE @docHandle int
--DECLARE @SubscriptionXML varchar(8000) --FOR TESTING PURPOSES

--set @SubscriptionXML = '<XML><Subscription><SubID>8366623</SubID><CurrentAction>72</CurrentAction><PreviousAction>72</PreviousAction></Subscription></XML>'

EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriptionXML  
INSERT INTO #Subs 
(
	 SubscriptionID, CurrentAction, PreviousAction
)  
SELECT [SubscriptionID], [CurrentAction], [PreviousAction]
FROM OPENXML(@docHandle,N'/XML/Subscription')
WITH
(
	[SubscriptionID] int 'SubID',
	[CurrentAction] int 'CurrentAction',
	[PreviousAction] int 'PreviousAction'
)

UPDATE Subscription
SET ActionID_Current = s2.CurrentAction, ActionID_Previous = s2.PreviousAction
FROM Subscription s
INNER JOIN #Subs s2
ON s.SubscriptionID = s2.SubscriptionID

DROP TABLE #Subs
