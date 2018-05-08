--sp_helptext 
CREATE proc [dbo].[sp_GetActivity_TopClicks_download]
(
	@EditionID int,    
	@BlastID int ,
	@linkID int,
	@topCount int,
	@type varchar(10)
)
as
Begin   

	create table #linkIDs (pageno int, linkID int, link varchar(100), totalcount int, uniquecount int)

	if @linkID = 0
	Begin   
			insert into #linkIDs
			exec sp_GetActivity_TopClicks @EditionID, @BlastID, @topCount
	End
	else
	Begin
		insert into #linkIDs
		select 0, @linkID, '', 0, 0
	End

	if @type = 'unique'
	Begin
		select PageNumber, LinkURL as link, max(ActionDate) Actiondate, 
			 eal.emailID, isnull(e.EmailAddress,'Anonymous') as Emailaddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender
		from     
			editionactivitylog eal join     
			link l on l.LinkID = eal.LinkID join    
			#linkIDs lk on l.linkID = lk.linkID join
			Page p on eal.pageID = p.pageID left outer join
			ecn5_communicator..emails e on e.emailID = eal.emailiD
		where      
			eal.EditionID = @EditionID and     
			ActionTypecode='Click'   and blastID = (case when @blastID = -1 then blastID else @blastID end)
		group by PageNumber, LinkURL, SessionID,
			 eal.emailID,e.EmailAddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender
		order by link, Actiondate desc
	end
	else
	begin
		 select 
			 PageNumber, case when isnull(Alias,'') = '' then LinkURL else Alias end as link, ActionDate,   
			 eal.emailID,isnull(e.EmailAddress,'Anonymous') as Emailaddress,e.Title,e.FirstName,e.LastName,e.FullName,e.Company,e.Occupation,e.Address,e.Address2,e.City,e.State,e.Zip,e.Country,e.Voice,e.Mobile,e.Fax,e.Website,e.Age,e.Income,e.Gender
		 from     
			editionactivitylog eal join     
			link l on l.LinkID = eal.LinkID join    
			#linkIDs lk on l.linkID = lk.linkID join
			Page p on eal.pageID = p.pageID left outer join
			ecn5_communicator..emails e on e.emailID = eal.emailiD
		 where      
			eal.EditionID = @EditionID and     
			ActionTypecode='Click'   and blastID = (case when @blastID = -1 then blastID else @blastID end)
			order by link, Actiondate desc
	End

	drop table #linkIDs
end
