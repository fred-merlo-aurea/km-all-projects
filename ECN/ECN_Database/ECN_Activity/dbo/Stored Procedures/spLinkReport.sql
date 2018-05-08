--declare
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

CREATE proc [dbo].[spLinkReport]
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
 
	 declare @sqlstring nvarchar(MAX),
			@ParmDefinition nvarchar(500);
	  
	 set @sqlstring = '
	 WITH CTE (BlastID, emailsubject,  blastcategory, ActionDate, linkownerIndexID, linkownername, linktype, Alias, Link)
		AS
		(	
	 select distinct b.BlastID, b.emailsubject, cm.CampaignName as blastcategory, b.sendtime as ActionDate, lon.linkownerIndexID, lon.linkownername, co.CodeDisplay as linktype,  la.Alias, la.Link' +
		 ' from ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) join ' +  
		 ' ECN5_COMMUNICATOR..layout l WITH (NOLOCK) on l.layoutID = b.layoutID join ' +
		 ' ECN5_COMMUNICATOR..Content c WITH (NOLOCK) on (c.contentID = l.ContentSlot1 or c.contentID = l.ContentSlot2 or c.contentID = l.ContentSlot3 or c.contentID = l.ContentSlot4 or c.contentID = l.ContentSlot5 or 
				c.contentID = l.ContentSlot6 or c.contentID = l.ContentSlot7 or c.contentID = l.ContentSlot8 or c.contentID = l.ContentSlot9 ) join ' +
		 ' ECN5_COMMUNICATOR..linkalias la WITH (NOLOCK) on la.contentID = c.contentID and la.IsDeleted = 0 join ' + 
		 ' ECN5_COMMUNICATOR..linkownerindex lon WITH (NOLOCK) on lon.LinkOwnerIndexID = la.LinkOwnerID left outer join' +  
		 ' ECN5_COMMUNICATOR..Code co WITH (NOLOCK) on CodeType = ''LinkType'' AND co.codeID = la.LinkTypeID left outer join' +  
		 ' ECN5_COMMUNICATOR..CampaignItemBlast cib WITH (NOLOCK) on  cib.BlastID = b.BlastID  left outer join' +
		 ' ECN5_COMMUNICATOR..CampaignItem ci WITH (NOLOCK) on  cib.CampaignItemID = ci.CampaignItemID  left outer join' +    
		' ECN5_COMMUNICATOR..Campaign cm WITH (NOLOCK) on  ci.CampaignID = cm.CampaignID' +  
		 ' where b.testblast = ''N'' and b.customerID = @CustID and CAST(b.sendtime as date) between @fd and @td '
	  
	 if len(@linkownerID) > 0  
	  set @sqlstring = @sqlstring + ' and lon.LinkOwnerIndexID in ('+ @linkownerID +')'  
	  
	 if len(@linktypeID) > 0  
	  set @sqlstring = @sqlstring + ' and la.LinkTypeID in ('+ @linktypeID +')'  
	  
	 if len(@campaignID) > 0  
	  set @sqlstring = @sqlstring + '  and cm.CampaignID in ('+ @campaignID +') '    
	  
	  
	 set @sqlstring = @sqlstring + ' and cast(b.sendtime as date ) between @fd and @td'
	  
	set @sqlstring = @sqlstring +  ' ) 
		 select t.*, ''Email'' as ActionFrom, 
				(select count (bac.ClickID) from BlastActivityClicks bac WITH (NOLOCK) where t.blastID = bac.blastID and bac.url = t.link) as clickcount ,
				(select count(distinct bac.emailID) from BlastActivityClicks bac WITH (NOLOCK) where t.blastID = bac.blastID and bac.url = t.link) as Uclickcount, 
				(select count(bao.OpenID) from BlastActivityOpens bao WITH (NOLOCK) where t.blastID = bao.blastID)  as viewcount,
				(select count(bas.SendID) from BlastActivitySends bas WITH (NOLOCK) where t.blastID = bas.blastID)  as sendcount  
		 from	CTE t  WITH (NOLOCK)
		 group by  t.BlastID, t.EmailSubject, t.blastcategory, t.ActionDate, t.LinkOwnerIndexID, t.LinkOwnerName, t.LinkType, t.Alias, t.Link
		 order by t.LinkOwnerIndexID';
	 
	SET @ParmDefinition = N'@CustID int, @fd date, @td date';

	EXECUTE sp_executesql
		@SQLString
		,@ParmDefinition
		,@CustID = @customerID
		,@fd = @fromdate
		,@td = @todate;

 
  
END