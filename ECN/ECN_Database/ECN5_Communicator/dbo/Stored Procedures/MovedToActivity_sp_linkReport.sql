CREATE proc [dbo].[MovedToActivity_sp_linkReport]
(
 @customerID int,  
 @linkownerID varchar(2000),  
 @linktypeID varchar(2000),  
 @fromdate varchar(20),  
 @todate varchar(20),  
 @blastcategoryID varchar(400)  
)
as
Begin  
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_linkReport', GETDATE())
	 declare @sqlstring nvarchar(4000),
			@ParmDefinition nvarchar(500);
	  
	 set @sqlstring = '
	 WITH CTE (BlastID, emailsubject,  blastcategory, ActionDate, linkownerIndexID, linkownername, linktype, Alias, Link)
		AS
		(	
	 select distinct b.BlastID, emailsubject, co1.CodeDisplay as blastcategory, sendtime as ActionDate, lon.linkownerIndexID, lon.linkownername, co.CodeDisplay as linktype,  la.Alias, la.Link' +
		 ' from [BLAST] b join ' +  
		 ' [LAYOUT] l on l.layoutID = b.layoutID join ' +
		 ' Content c on (c.contentID = l.ContentSlot1 or c.contentID = l.ContentSlot2 or c.contentID = l.ContentSlot3 or c.contentID = l.ContentSlot4 or c.contentID = l.ContentSlot5 or 
				c.contentID = l.ContentSlot6 or c.contentID = l.ContentSlot7 or c.contentID = l.ContentSlot8 or c.contentID = l.ContentSlot9 ) join ' +
		 ' linkalias la on la.contentID = c.contentID join ' + 
		 ' linkownerindex lon on lon.LinkOwnerIndexID = la.LinkOwnerID left outer join' +  
		 ' Code co on CodeType = ''LinkType'' AND co.codeID = la.LinkTypeID left outer join' +  
		 ' Code co1 on  co1.CodeType = ''BlastCategory'' AND co1.codeID = b.CodeID ' +
		 ' where b.testblast = ''N'' and b.customerID = @CustID '
	  
	 if len(@linkownerID) > 0  
	  set @sqlstring = @sqlstring + ' and lon.LinkOwnerIndexID in ('+ @linkownerID +')'  
	  
	 if len(@linktypeID) > 0  
	  set @sqlstring = @sqlstring + ' and la.LinkTypeID in ('+ @linktypeID +')'  
	  
	 if len(@blastcategoryID) > 0  
	  set @sqlstring = @sqlstring + '  and co1.codeID in ('+ @blastcategoryID +') '    
	  
	 if len(@fromdate) > 0   
	  set @sqlstring = @sqlstring + ' and convert(datetime, convert(varchar(10),b.sendtime,101)) >= @fd'  
	  
	 if len(@todate) > 0   
	 Begin
		set @todate = @todate + ' 23:59:59'
		set @sqlstring = @sqlstring + ' and convert(datetime, convert(varchar(10),b.sendtime,101)) <=  @td'  
	 End
	  
	set @sqlstring = @sqlstring +  ' ) 
		 select t.*, ''Email'' as ActionFrom, 
				count(case when actiontypecode = ''click'' then EAID end) as clickcount,
				count(distinct case when actiontypecode = ''click'' then emailID end) as Uclickcount, 
				count(distinct case when actiontypecode = ''open'' then EAID end) as viewcount 
		 from	CTE t join 
				emailactivitylog eal on t.blastID = eal.blastID and (ActionTypeCode = ''open''  or (ActionTypeCode = ''click'' and eal.actionvalue = t.link))
		 group by  t.BlastID, t.EmailSubject, t.BlastCategory, t.ActionDate, t.LinkOwnerIndexID, t.LinkOwnerName, t.LinkType, t.Alias, t.Link
		 order by t.LinkOwnerIndexID';
	 
	SET @ParmDefinition = N'@CustID int, @fd datetime, @td datetime';

	EXECUTE sp_executesql
		@SQLString
		,@ParmDefinition
		,@CustID = @customerID
		,@fd = @fromdate
		,@td = @todate;

 
  
end
