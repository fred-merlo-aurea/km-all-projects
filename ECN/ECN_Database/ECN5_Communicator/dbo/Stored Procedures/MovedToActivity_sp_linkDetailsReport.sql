CREATE proc [dbo].[MovedToActivity_sp_linkDetailsReport]
(
	@customerID int,
	@linkownerID varchar(400),
	@linktypeID varchar(400),
	@fromdate varchar(10),
	@todate varchar(10),
	@blastcategoryID varchar(400)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_linkDetailsReport', GETDATE())
	declare @sqlcolumn varchar(1000),
			@sqlfrom varchar(1000),
			@sqlwhere varchar(2000)

	set @sqlcolumn = 'select distinct emailID '

 set @sqlfrom = 
     ' from emailactivitylog eal  join ' +  
     ' [BLAST] b on b.blastID = eal.blastID join ' +  
	 ' [LAYOUT] l on l.layoutID = b.layoutID join ' +
	 ' Content c on (c.contentID = ContentSlot1 or c.contentID = ContentSlot2 or c.contentID = ContentSlot3 or c.contentID = ContentSlot4 or c.contentID = ContentSlot5 or c.contentID = ContentSlot6 or c.contentID = ContentSlot7 or c.contentID = ContentSlot8 or c.contentID = ContentSlot9 ) join ' +
	 ' linkalias la on eal.actionvalue = la.link and la.contentID = c.contentID join ' + 
     ' linkownerindex lon on lon.LinkOwnerIndexID = la.LinkOwnerID left outer join' +  
     ' Code co on CodeType = ''LinkType'' AND co.CustomerID = ' + convert(varchar,@customerID) + ' and co.codeID = la.LinkTypeID left outer join' +  
     ' Code co1 on  co1.CodeType = ''BlastCategory'' AND co1.CustomerID = ' + convert(varchar,@customerID) + ' and co1.codeID = b.CodeID'   
  
 set @sqlwhere = ' where  eal.actiontypecode = ''click'' and b.testblast = ''N'' and c.customerID = ' + convert(varchar,@customerID) + ' and b.customerID = ' + convert(varchar,@customerID)  
  


	if len(@linkownerID) > 0
		set @sqlwhere = @sqlwhere + ' and lon.LinkOwnerIndexID in ('+ @linkownerID +')'

	if len(@linktypeID) > 0
		set @sqlwhere = @sqlwhere + ' and la.LinkTypeID in ('+ @linktypeID +')'

	if len(@fromdate) > 0 
		set @sqlwhere = @sqlwhere + ' and convert(datetime, convert(varchar(10),b.sendtime,101)) >= '''+ @fromdate +''''

	if len(@todate) > 0 
		set @sqlwhere = @sqlwhere + ' and convert(datetime, convert(varchar(10),b.sendtime,101)) <= ''' + @todate + ''''


	if len(@blastcategoryID) > 0
		set @sqlwhere = @sqlwhere + ' and co1.codeID in ('+ @blastcategoryID +') '

	exec(' select EmailAddress, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, City, State, Zip, Country, Voice from emails where emailID in (' + @sqlcolumn + @sqlfrom + @sqlwhere +')')

end
