CREATE PROCEDURE [dbo].[spGetCanonEmailNewMessageList]      
 @BlastID varchar(100),      
 @bSubscriberID int      
AS  
  
 set NOCOUNT ON      
      
Begin      
       
  
 declare @GroupID int,      
   @CustomerID int,      
   @groupdatafieldsID int ,
	@bid int 
    
create table #blasts (blastID int)  
  
 insert into #blasts  
 select * from ecn5_communicator..fn_split(@blastID, ',')  

 select top 1 @bID = blastID from #blasts  
        
 select @GroupID = GroupID, @CustomerID = CustomerID from ecn5_communicator..[BLAST] where blastID = @bID       
 select @groupdatafieldsID = groupdatafieldsID from ecn5_communicator..groupdatafields where groupID = @GroupID and shortname = 'subscriberID'      

create table #tblEmail1 (EmailID int)

insert into #tblEmail1
select EmailID from BlastActivityBounces where BlastID in (select blastID from #blasts)

insert into #tblEmail1
select EmailID from BlastActivityClicks where BlastID in (select blastID from #blasts)

insert into #tblEmail1
select EmailID from BlastActivityOpens where BlastID in (select blastID from #blasts)

insert into #tblEmail1
select EmailID from BlastActivityUnSubscribes bau join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID where BlastID in (select blastID from #blasts) and uc.UnsubscribeCode != 'subscribe'

    
 if (@bsubscriberID = 1)      
 begin  
  select inn1.EmailID, inn1.EmailAddress, inn1.FirstName, inn1.LastName, inn1.Company,   
   inn1.Address, inn1.Address2, inn1.City, inn1.State, inn1.Zip, inn1.Country, inn1.Voice, inn1.Mobile, inn1.Fax,  
   max(datavalue) as subsriberID from  
  (      
  select Emails.EmailID, EmailAddress, FirstName, LastName, Company, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax   
  from        
   ecn5_communicator..Emails join       
   BlastActivitySends bas on bas.EmailID = Emails.EmailID       
  where       
   bas.BlastID in (select blastID from #blasts) and         
   Emails.EmailID not in  (select emailID from #tblEmail1)      
  ) inn1 left outer join ecn5_communicator..emaildatavalues edv on inn1.emailID = edv.EmailID and edv.groupdatafieldsID = @groupdatafieldsID   
  group by inn1.EmailID, inn1.EmailAddress, inn1.FirstName, inn1.LastName, inn1.Company,   
   inn1.Address, inn1.Address2, inn1.City, inn1.State, inn1.Zip, inn1.Country, inn1.Voice, inn1.Mobile, inn1.Fax  
  --order by inn1.EmailID      
 end    
 else      
 begin     
  select distinct Emails.EmailID, EmailAddress, FirstName, LastName, Company, Address, Address2, City, State, Zip, Country, Voice, Mobile, Fax      
  from        
   ecn5_communicator..Emails join       
   BlastActivitySends bas on bas.EmailID = Emails.EmailID       
  where       
   bas.BlastID in (select blastID from #blasts) and         
   Emails.EmailID not in  (select emailID from #tblEmail1)
  --order by Emails.EmailID   
 end 
  
  drop table #blasts   
  drop table #tblEmail1   
End
