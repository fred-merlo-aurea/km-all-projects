CREATE proc [dbo].[rpt_PharmaLive_3a_Report]
(
	--@Business varchar(500),
	--@Industry varchar(500),
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


declare @Business TABLE (ID varchar(5), name varchar(200))
declare @industry TABLE (ID varchar(5), name varchar(200))

insert into @business values ('00','No Response')
insert into @business values ('01','Pharmaceutical manufacturer')
insert into @business values ('03','Healthcare communications company')
insert into @business values ('04','Marketing services company')
insert into @business values ('05','General business services company')
insert into @business values ('06','Hospital')
insert into @business values ('07','Academic/University research institution')
insert into @business values ('08','Government agency')
insert into @business values ('09','Medical equipment manufacturer')
insert into @business values ('10','Data management company')
insert into @business values ('11','Biotechnology company')
insert into @business values ('12','Contract research organization')
insert into @business values ('13','Clinical study site/SMO')
insert into @business values ('14','Clinical lab')
insert into @business values ('15','Generic pharmaceutical manufacturer')
insert into @business values ('99','Other Support or Service Company')
insert into @business values ('40','Packaging Company')
insert into @business values ('41','Executive Recruitment Agency')
insert into @business values ('42','Venture Capital/Investment Firm')
insert into @business values ('43','Consulting Firm')
insert into @business values ('44','Media Company')
insert into @business values ('45','Legal firm')
/*insert into @business values ('20','20')
insert into @business values ('23','23')
insert into @business values ('25','25')
insert into @business values ('26','26')
insert into @business values ('27','27')
insert into @business values ('29','29')
insert into @business values ('30','30')
*/

insert into @industry values ('00','No Response')
insert into @industry values ('09','R&D management')
insert into @industry values ('10','Senior management')
insert into @industry values ('12','Product management')
insert into @industry values ('14','Sales management')
insert into @industry values ('15','Agency account management')
insert into @industry values ('19','Marketing services')
insert into @industry values ('20','Media management (incl. directors/planners)')
insert into @industry values ('22','Quality control')
insert into @industry values ('23','Marketing,advertising or promotion management')
insert into @industry values ('24','Medical director/associate medical director')
insert into @industry values ('25','Clinical trials management')
insert into @industry values ('26','Clinical/drug research')
insert into @industry values ('27','Regulatory affairs')
insert into @industry values ('28','Clinical monitoring/CRC/CRA')
insert into @industry values ('29','Academic research or professor')
insert into @industry values ('30','Data management or analysis')
insert into @industry values ('31','Clinical documentation preparation')
insert into @industry values ('32','Project management')
insert into @industry values ('33','Drug safety')
insert into @industry values ('34','Finance management')
insert into @industry values ('35','Licensing')
insert into @industry values ('36','Manufacturing')
insert into @industry values ('37','IT management')
insert into @industry values ('38','Business strategy')
insert into @industry values ('39','Legal professional')


insert into @industry values ('99','Other functions')
--insert into @industry values ('11','')

declare @tmpReport TABLE (emailID int, BS varchar(255), FN varchar(255))

insert into @tmpReport
select	eg.emailID, 
		isnull(MAX(case when gdf.shortname ='Business' then datavalue end),'00'),
		isnull(MAX(case when gdf.shortname ='Responsibility' then datavalue end),'00')
from	emailgroups eg join 
		groupdatafields gdf on eg.groupID = gdf.groupID and gdf.shortname in ('Business','Responsibility' ) left outer join
		emaildatavalues edv on eg.emailID = edv.emailID and edv.groupdatafieldsID = gdf.groupdatafieldsID 
where	eg.groupID = @groupID and subscribetypecode = 'S' and 
		isnull(lastchanged,CreatedOn) between @startdate and @enddate
group by eg.emailID 
/* 
select	edv.emailID, 
		isnull(MAX(case when gdf.shortname ='Business' then datavalue end),'00'),
		isnull(MAX(case when gdf.shortname ='Responsibility' then datavalue end),'00') 
from 
		emailgroups eg left outer join 
		emaildatavalues edv on eg.emailID = edv.emailID left outer join	
		groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID and gdf.shortname in ('Business','Responsibility' )
where	eg.groupID = @groupID and 
		CreatedOn between @startdate and @enddate
group by edv.EmailID
*/
select	bID as BS, BusinessValue, Iid as FN, IndustryValue,
		count(t.BS) as counts 
from	(select b.ID as bID, b.name businessvalue, i.id Iid, i.name Industryvalue from @Business b cross join @industry i)
		 inn left outer join 
		 @tmpReport t on inn.bid = bs and inn.IID = FN
group by bID, BusinessValue, Iid , IndustryValue
order by bID, Iid

End
