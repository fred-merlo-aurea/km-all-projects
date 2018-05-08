create procedure e_AdmsLog_Select_ClientId_RecordSource_StartDate_EndDate
@ClientId int,
@recordSource varchar(50),
@startDate date,
@endDate date
as
	begin
		set nocount on
		--UAD = Audience Data and Data Compare
		--API
		--CIRC = everything except API and Audience Data
		if(@recordSource = 'CIRC')
			begin
				select *
				from AdmsLog with(nolock)
				where ClientId=@clientId
				and cast(FileStart as date) between @startDate and @endDate
				and RecordSource not in ('API', 'Audience Data', 'Data Compare')
				and FileStatusId in (   
				   select CodeId    
				   from UAD_Lookup..Code with(nolock)   
				   where CodeTypeId = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'File Status')    
				   and CodeName in ('Failed','Completed')) 
				order by FileStart 
			end
		else if(@recordSource = 'UAD')
			begin
				select *
				from AdmsLog with(nolock)
				where ClientId=@clientId
				and cast(FileStart as date) between @startDate and @endDate
				and RecordSource in ('Audience Data','Data Compare')
				and FileStatusId in (   
				   select CodeId    
				   from UAD_Lookup..Code with(nolock)   
				   where CodeTypeId = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'File Status')    
				   and CodeName in ('Failed','Completed'))
				order by FileStart 
			end
		else if(@recordSource = 'API')
			begin
				select *
				from AdmsLog with(nolock)
				where ClientId=@clientId
				and cast(FileStart as date) between @startDate and @endDate
				and RecordSource = 'API'
				and FileStatusId in (   
				   select CodeId    
				   from UAD_Lookup..Code with(nolock)   
				   where CodeTypeId = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'File Status')    
				   and CodeName in ('Failed','Completed'))
				order by FileStart 
			end
	end
go
