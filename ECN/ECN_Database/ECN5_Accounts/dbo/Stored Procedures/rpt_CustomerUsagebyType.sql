CREATE procedure [dbo].[rpt_CustomerUsagebyType]  
(   
 @CustomerType varchar(100)  
)  
as
Begin    
 Set nocount on    
   
  
declare @tbl TABLE   
 (  
  basechannelID int ,   
  basechannelName varchar(100),  
  customerID int,  
  CustomerName varchar(100),  
  CustomerType varchar(50),  
  Usercount int,  
  MTDcount int,  
  YTDcount int,  
  MTDsent int,  
  YTDsent int  
 )  
  
 insert into @tbl  
 select  b.BaseChannelID,    
   b.BaseChannelName,   
   c.customerID,   
   c.CustomerName,  
   c.CustomerType,    
   (select count(distinct u.userID) from KMPlatform..[User] u
						 join KMPlatform..UserClientSecurityGroupMap uc on u.UserID = uc.UserID 
						 where uc.ClientID = c.PlatformClientID and u.IsActive = 1 and uc.IsActive = 1
						 ) as UserCount,      
   sum(case when month(bl.sendtime) = Month(getdate()) and year(bl.sendtime) = Year(getdate())  and statuscode in ('sent','deleted') and TestBlast <> 'Y' then 1 else 0 end) as MTDcount,      
   sum(case when month(bl.sendtime) <= Month(getdate()) and year(bl.sendtime) = Year(getdate()) and statuscode in ('sent','deleted') and TestBlast <> 'Y' then 1 else 0 end) as YTDcount,      
   sum(case when month(bl.sendtime) = Month(getdate()) and year(bl.sendtime) = Year(getdate())  and statuscode in ('sent','deleted') and TestBlast <> 'Y' then sendtotal else 0 end) as MTDsent,      
   sum(case when month(bl.sendtime) <= Month(getdate()) and year(bl.sendtime) = Year(getdate()) and statuscode in ('sent','deleted') and TestBlast <> 'Y' then sendtotal else 0 end) as YTDsent   
 From BaseChannel b join   
   Customer c on b.basechannelID = c.basechannelID left outer join  
   [ECN5_COMMUNICATOR].[DBO].Blast bl on bl.customerID = c.customerID  
 where   
   c.customerType = (case when @customerType = '' then c.customerType else @CustomerType end)  
 group by   
 b.BaseChannelID,    
 b.BaseChannelName,   
 c.customerID,   
 c.CustomerName,  
 c.CustomerType  ,
 c.PlatformClientID


 select distinct BasechannelID, basechannelName, customerType, count(distinct customerID) as cCount, sum(usercount) as ucount, sum(mtdcount) as mtdcount, sum(ytdcount) as ytdcount, sum(mtdsent) as mtdsent, sum(ytdsent) as ytdsent   
 from @tbl   
 group by BasechannelID, basechannelName, customerType  
 order by basechannelName  
   
 select BasechannelID, customerID, customerName, customerType, Usercount, MTDcount, YTDcount, MTDsent, YTDsent from @tbl order by basechannelID, customerName  
  
  
End
