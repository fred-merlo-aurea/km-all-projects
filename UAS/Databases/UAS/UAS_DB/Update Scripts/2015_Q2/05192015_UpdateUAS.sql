

-- Move deliverability table from circ to uas..codeType and uas..code table
declare @deliverId int

insert into uas..CodeType
values('Deliver','Deliverability or Demo7',1,GETDATE(),null,29,null)

set @deliverId = (select codetypeid from uas..CodeType where CodeTypeName = 'Deliver')

insert into uas..Code
values(@deliverId,'Print','A','Print','',1,0,null,1,GETDATE(),null,29,null),
(@deliverId,'Digital','B','Digital','',2,0,null,1,GETDATE(),null,29,null),
(@deliverId,'Both','C','Both','',3,0,null,1,GETDATE(),null,29,null)

-- Action Table
INSERT INTO UAS..Action(ActionTypeID,CategoryCodeID,TransactionCodeID,Note,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
SELECT ActionTypeID,CategoryCodeID,TransactionCodeID,Note,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID FROM Circulation..ACTION

-- Par3C
insert into uas..Par3c
select DisplayName,DisplayOrder,isActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID from Circulation..Par3c

-- QualificationSourceType
insert into uas..QualificationSourceType
select [QSourceTypeName]
      ,[QSourceTypeCode]
      ,[DisplayName]
      ,[DisplayOrder]
      ,[IsActive]
      ,[DateCreated]
      ,[DateUpdated]
      ,[CreatedByUserID]
      ,[UpdatedByUserID]
from Circulation..QualificationSourceType   

-- QualificationSource
insert into uas..QualificationSource
select [QSourceTypeID]
      ,[QSourceName]
      ,[QSourceCode]
      ,[DisplayName]
      ,[DisplayOrder]
      ,[IsActive]
      ,[DateCreated]
      ,[DateUpdated]
      ,[CreatedByUserID]
      ,[UpdatedByUserID]
from Circulation..QualificationSource    

-- TransactionCodeType
insert into uas..TransactionCodeType
select [TransactionCodeTypeName],[IsActive],[IsFree],[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID]
from Circulation..TransactionCodeType
Order by TransactionCodeTypeID

-- TransactionCode
insert into uas..TransactionCode
select [TransactionCodeTypeID],[TransactionCodeName],[TransactionCodeValue],[IsActive],[DateCreated],[DateUpdated]
      ,[CreatedByUserID],[UpdatedByUserID],[IsKill]
from Circulation..TransactionCode
order by TransactionCodeID
      
-- CategoryCodeType
insert into uas..CategoryCodeType
select [CategoryCodeTypeName],[IsActive],[IsFree],[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID]
From Circulation..CategoryCodeType    
order by CategoryCodeTypeID

-- CategoryCode
insert into uas..CategoryCode
select [CategoryCodeTypeID],[CategoryCodeName],[CategoryCodeValue],[IsActive],[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID]
from Circulation..CategoryCode      
order by CategoryCodeID

-- Par3C
insert into uas..Par3c
select [DisplayName],[DisplayOrder],[IsActive],[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID]
from Circulation..Par3c     


----
---- Pick any uad, doesn't have to be France
----
update r
set sort_order = s.sort_order,country_sort_order = s.country_sort_order
from uas..Region r join FranceMasterDB_Test..State s on r.RegionCode = s.state


-- EmailStatus to UAS code tables
declare @es int 

insert into uas..CodeType
values('Email Status','',1,GETDATE(),null,29,null)

set @es = (select codetypeid from uas..CodeType where CodeTypeName = 'Email Status')

insert into uas..Code
values(@es,'Active','A','Active','',1,0,null,1,GETDATE(),null,29,null),
(@es,'Bounced','B','Bounced','',2,0,null,1,GETDATE(),null,29,null),
(@es,'Invalid','I','Invalid','',3,0,null,1,GETDATE(),null,29,null),
(@es,'MasterSuppressed','MS','MasterSuppressed','',4,0,null,1,GETDATE(),null,29,null),
(@es,'Spam','S','Spam','',5,0,null,1,GETDATE(),null,29,null),
(@es,'UnSubscribe','US','UnSubscribe','',6,0,null,1,GETDATE(),null,29,null),
(@es,'Unverified','U','Unverified','',7,0,null,1,GETDATE(),null,29,null)


-- Export Type to Code tables
--declare @et int

--insert into uas..CodeType
--values('Export Type','',1,GETDATE(),null,29,null)

--set @et = (select codetypeid from uas..CodeType where CodeTypeName = 'Export Type')

--insert into uas..Code
--values(@et,'FTP Export','FE','FTP Export','',1,0,null,1,GETDATE(),null,29,null),
--(@et,'Export to ECN','EE','Export to ECN','',2,0,null,1,GETDATE(),null,29,null),
--(@et,'Export to Campaign','EC','Export to Campaign','',3,0,null,1,GETDATE(),null,29,null)


-- Update Client table with ECnCustomerID
update c
set ECNCustomerID = e.CustomerID
--select *
from circulation..Publisher p join circulation..ECNCustomertoPublisherMapping e on p.PublisherID = e.PublisherID
							  join uas..Client c on c.ClientID = p.ClientID


-- Insert data to SubscriptionStatus
insert into uas..SubscriptionStatus
select StatusName,StatusCode,Color,Icon,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID from Circulation..SubscriptionStatus order by SubscriptionStatusID

-- Insert data to SubscriptionStatusMatrix
insert into uas..SubscriptionStatusMatrix
select subscriptionStatusID,CategoryCodeID,TransactionCodeID,IsActive,datecreated,dateupdated,createdbyuserid,updatedbyuserid from Circulation..SubscriptionStatusMatrix

-- Add SubscriptionStatusID to PubSubscription, Do for every UAD
update ps
set SubscriptionStatusID = ssm.SubscriptionStatusID
--select ps.PubCategoryID,ps.PubTransactionID,c.CategoryCodeID,t.TransactionCodeID,ssm.CategoryCodeID,ssm.TransactionCodeID,ssm.StatusMatrixID,ssm.SubscriptionStatusID
from PubSubscriptions ps join uas..CategoryCode c on ps.PubCategoryID = c.CategoryCodeValue
					     join uas..TransactionCode t on ps.PubTransactionID = t.TransactionCodeValue
					     join uas..SubscriptionStatusMatrix ssm on c.CategoryCodeID = ssm.CategoryCodeID and t.TransactionCodeID = ssm.TransactionCodeID

-- Add IsSubscribed data
update PubSubscriptions
set IsSubscribed = case when subscriptionStatusid in (1,3,4) then 1 else 0 end

-- Marketing Data
insert into uas..Marketing
select MarketingName,MarketingCode,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID from Circulation..Marketing order by MarketingID
