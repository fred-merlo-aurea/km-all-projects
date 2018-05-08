CREATE proc [dbo].[rpt_PharmaLive_3b_Report]
(
	@groupID int,
	@startdate varchar(10),
	@enddate varchar(25)
)
as

Begin

	if len(ltrim(rtrim(@startdate))) = 0
		set @startdate = '01/01/1950'

	if len(ltrim(rtrim(@enddate))) = 0
		set @enddate = convert(varchar(10),getdate(), 101)

	set @enddate = @enddate + ' 23:59:59'

	declare @channel TABLE (ID varchar(5), name varchar(200))

	insert into @channel values ('W','Written')
	insert into @channel values ('T','Telecommunication')
	insert into @channel values ('I','Internet and Email')

	declare @currentyear int,
			@age1yrst datetime,
			@age1yred datetime,
			@age2yrst datetime,
			@age2yred datetime,
			@age3yrst datetime,
			@age3yred datetime

	if month(getdate()) > 11 
		set @currentyear = year(getdate())
	else
		set @currentyear = year(getdate())-1

	set @age1yrst = '12/01/' + Convert(varchar, @currentYear)
	set @age1yred = '11/30/' + Convert(varchar, @currentYear + 1) + ' 23:59:59'

	set @currentYear = @currentYear -1

	set @age2yrst = '12/01/' + Convert(varchar, @currentYear)
	set @age2yred = '11/30/' + Convert(varchar, @currentYear + 1) + ' 23:59:59'

	set @currentYear = @currentYear -1

	set @age3yrst = '12/01/' + Convert(varchar, @currentYear)
	set @age3yred = '11/30/' + Convert(varchar, @currentYear + 1) + ' 23:59:59'

	--select @age1yrst, @age1yred, @age2yrst, @age2yred, @age3yrst, @age3yred

	declare @tmpReport TABLE (emailID int, Qdate datetime, Effort_Code varchar(1), Aging int)

	insert into @tmpReport (emailID, Qdate, Effort_code, Aging)
	select	eg.emailID, 
			eg.createdon as Qdate,
			case when gdf.shortname ='Original_Effort_Code' then substring(isnull(datavalue,'N'),1,1) end as Effort_Code,
			case 
				when eg.createdon between @age1yrst and @age1yred then 1
				when eg.createdon between @age2yrst and @age2yred then 2
				when eg.createdon between @age3yrst and @age3yred then 3	
				else 4
			end
	from 
			emailgroups eg join 
			groupdatafields gdf on eg.groupID = gdf.groupID left outer join
			emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID and eg.emailID = edv.emailID
	where eg.groupID = @groupID and gdf.shortname = 'Original_Effort_Code' and
			CreatedOn between @startdate and @enddate and subscribetypecode = 'S'
			--and isnull(datavalue,'') <> ''
	
	select upper(Effort_code) as Effort_code, 
			Case Effort_code 
				when 'W' then 'Written'
				when 'Q' then 'Written'
				when 'T' then 'Telecommunication'
				when 'E' then 'Internet and Email' else 'NO CODE' 
			end as Effort,
			Aging, 
			case Aging 
				when 1 then Convert(varchar,Month(@age1yred)) +'/'+ right(Convert(varchar,Year(@age1yred)),2) + '-' +  Convert(varchar,Month(@age1yrst)) +'/'+ right(Convert(varchar,Year(@age1yrst)),2)
				when 2 then Convert(varchar,Month(@age2yred)) +'/'+ right(Convert(varchar,Year(@age2yred)),2) + '-' +  Convert(varchar,Month(@age2yrst)) +'/'+ right(Convert(varchar,Year(@age2yrst)),2)
				when 3 then Convert(varchar,Month(@age3yred)) +'/'+ right(Convert(varchar,Year(@age3yred)),2) + '-' +  Convert(varchar,Month(@age3yrst)) +'/'+ right(Convert(varchar,Year(@age3yrst)),2)
				else 'Before ' + Convert(varchar,Month(@age3yrst)) +'/'+ right(Convert(varchar,Year(@age3yrst)),2)
			end as AgingYr,
			case Aging 
				when 1 then '1 Yr.'
				when 2 then '2 Yrs'
				when 3 then '3 Yrs'
				else 'Beyond'
			end as AgingText,
			count(EmailID) as counts
	from @tmpReport
	group by Effort_code, Aging
	order by Effort_code, Aging

	--select count(emailID) from emailgroups where groupID = @groupID and subscribetypecode = 'S'
End
