CREATE proc [dbo].[spLinkDetailsReport]
(
	@customerID int,
	@linkownerID varchar(MAX),
	@linktypeID varchar(MAX),
	@fromdate date,
	@todate date,
	@campaignID varchar(MAX)
)
as
Begin
--	declare
-- @customerID int,  
-- @linkownerID varchar(2000),  
-- @linktypeID varchar(2000),  
-- @fromdate varchar(20),  
-- @todate varchar(20),  
-- @blastcategoryID varchar(400)  

--set @customerID =  2069
--set @linkownerID = ''
--set @linktypeID = ''
--set @fromdate = '8/1/2011'
--set @todate = '8/15/2011'
--set @blastcategoryID = ''


	      declare @sqlcolumn varchar(MAX),
                  @sqlfrom varchar(MAX),
                  @sqlwhere varchar(MAX)

      set @sqlcolumn = 'select distinct emailID '

  set @sqlfrom = 
     ' from BlastActivityClicks bac WITH (NOLOCK) join ' +  
     ' ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.blastID = bac.blastID join ' +  
       ' ECN5_COMMUNICATOR..layout l WITH (NOLOCK) on l.layoutID = b.layoutID join ' +
      ' ECN5_COMMUNICATOR..Content c WITH (NOLOCK) on (c.contentID = ContentSlot1 or c.contentID = ContentSlot2 or c.contentID = ContentSlot3 or c.contentID = ContentSlot4 or c.contentID = ContentSlot5 or c.contentID = ContentSlot6 or c.contentID = ContentSlot7 or c.contentID = ContentSlot8 or c.contentID = ContentSlot9 ) join ' +
      ' ECN5_COMMUNICATOR..linkalias la WITH (NOLOCK) on bac.URL = la.link and la.contentID = c.contentID join ' + 
     ' ECN5_COMMUNICATOR..linkownerindex lon WITH (NOLOCK) on lon.LinkOwnerIndexID = la.LinkOwnerID left outer join' +  
     ' ECN5_COMMUNICATOR..Code co WITH (NOLOCK) on CodeType = ''LinkType'' AND co.CustomerID = ' + convert(varchar,@customerID) + ' and co.codeID = la.LinkTypeID left outer join' +  
     ' ECN5_COMMUNICATOR..CampaignItemBlast cib WITH (NOLOCK) on  cib.BlastID = b.BlastID  left outer join' +
      ' ECN5_COMMUNICATOR..CampaignItem ci WITH (NOLOCK) on  cib.CampaignItemID = ci.CampaignItemID  left outer join' +    
       ' ECN5_COMMUNICATOR..Campaign cm WITH (NOLOCK) on  ci.CampaignID = cm.CampaignID  AND cm.CustomerID = ' + convert(varchar,@customerID) + '' 
       
 set @sqlwhere = ' where  b.testblast = ''N'' and c.customerID = ' + convert(varchar,@customerID) + ' and b.customerID = ' + convert(varchar,@customerID)  
  


      if len(@linkownerID) > 0
            set @sqlwhere = @sqlwhere + ' and lon.LinkOwnerIndexID in ('+ @linkownerID +')'

      if len(@linktypeID) > 0
            set @sqlwhere = @sqlwhere + ' and la.LinkTypeID in ('+ @linktypeID +')'


	  set @sqlwhere = @sqlwhere + ' and cast(b.sendtime as date) between ''' + convert(varchar(10),@fromdate) + ''' and ''' + convert(varchar(10),@todate ) + ''''


      if len(@campaignID) > 0
            set @sqlwhere = @sqlwhere + ' and cm.CampaignID in (' + @campaignID+ ') '

      exec( ' select EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, City, State, Zip, 
      Country, Voice from ECN5_COMMUNICATOR..emails WITH (NOLOCK) where emailID in (' + @sqlcolumn + @sqlfrom + @sqlwhere +')')

end