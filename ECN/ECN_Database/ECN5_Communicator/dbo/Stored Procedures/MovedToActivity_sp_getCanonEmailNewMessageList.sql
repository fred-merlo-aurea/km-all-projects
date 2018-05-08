CREATE PROCEDURE [dbo].[MovedToActivity_sp_getCanonEmailNewMessageList]      
 @BlastID varchar(100),      
 @bSubscriberID int      
AS  
  
 set NOCOUNT ON      
      
Begin      
   	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getCanonEmailNewMessageList', GETDATE())    
  
 declare @GroupID int,      
   @CustomerID int,      
   @groupdatafieldsID int ,
	@bid int 
    
create table #blasts (blastID int)  
  
 insert into #blasts  
 select * from dbo.fn_split(@blastID, ',')  

 select top 1 @bID = blastID from #blasts  
        
 select @GroupID = GroupID, @CustomerID = CustomerID from [BLAST] where blastID = @bID       
 select @groupdatafieldsID = groupdatafieldsID from groupdatafields where groupID = @GroupID and shortname = 'subscriberID'      

create table #tblEmail1 (EmailID int)

insert into #tblEmail1
select EmailID from EmailActivityLog el where BlastID  in (select blastID from #blasts) and (ActionTypeCode = 'bounce' or ActionTypeCode = 'click' or ActionTypeCode = 'open' or (ActionTypeCode = 'subscribe' and ActionValue = 'U'))

      
 --((ActionTypeCode = 'bounce'  and Actionvalue='hard' )  removed hard bounce filter on 11/27/2006 per kirk's request    
    
 if (@bsubscriberID = 1)      
 begin  
  select inn1.EmailID, inn1.EmailAddress, inn1.FirstName, inn1.LastName, inn1.Company,   
   inn1.Address, inn1.Address2, inn1.City, inn1.State, inn1.Zip, inn1.Country, inn1.Voice, inn1.Mobile, inn1.Fax,  
   max(datavalue) as subsriberID from  
  (      
  select Emails.EmailID, EmailAddress, FirstName, LastName, Company, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax   
  from        
   Emails join       
   EmailActivityLog on EmailActivityLog.EmailID = Emails.EmailID       
  where       
   EmailActivityLog.BlastID in (select blastID from #blasts) and         
   ActionTypeCode = 'send' and       
   Emails.EmailID not in  (select emailID from #tblEmail1)      
  ) inn1 left outer join emaildatavalues edv on inn1.emailID = edv.EmailID and edv.groupdatafieldsID = @groupdatafieldsID   
  group by inn1.EmailID, inn1.EmailAddress, inn1.FirstName, inn1.LastName, inn1.Company,   
   inn1.Address, inn1.Address2, inn1.City, inn1.State, inn1.Zip, inn1.Country, inn1.Voice, inn1.Mobile, inn1.Fax  
  --order by inn1.EmailID      
 end    
 else      
 begin     
  select distinct Emails.EmailID, EmailAddress, FirstName, LastName, Company, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax      
  from        
   Emails join       
   EmailActivityLog on EmailActivityLog.EmailID = Emails.EmailID       
  where       
   EmailActivityLog.BlastID in (select blastID from #blasts) and         
   ActionTypeCode = 'send' and       
   Emails.EmailID not in  (select emailID from #tblEmail1)
  --order by Emails.EmailID   
 end 
  
  drop table #blasts   
  drop table #tblEmail1   
End
