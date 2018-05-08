create procedure ccp_BriefMedia_PostDataMatch
@SourceFileId int = 0,
@ProcessCode varchar(50) = '',
@ClientId int = 0
as
BEGIN

	set nocount on

	--2. Stuff distinct IP_ADDRESS values based on IGRP_NO 
	declare @tmp table (SDFinalID int, IGrp_No uniqueidentifier, Value varchar(max))
	insert into @tmp (SDFinalID,IGrp_No,Value)
	select sdf.SDFinalID,sf.IGrp_No,
	STUFF(
         (SELECT ', ' +  sdf2.Value
          FROM SubscriberDemographicFinal sdf2
          join SubscriberFinal sf2 on sdf2.SFRecordIdentifier = sf2.SFRecordIdentifier
          where sf.IGRP_NO = sf2.IGRP_NO
          and sdf2.MAFField = 'IP_ADDRESS'
		  and sf2.ProcessCode = @ProcessCode
          group by sf2.IGrp_No,sdf2.Value
          FOR XML PATH (''))
          , 1, 1, '')
    from SubscriberDemographicFinal sdf 
    join SubscriberFinal sf on sdf.SFRecordIdentifier = sf.SFRecordIdentifier
    where sf.ProcessCode = @ProcessCode
    and sdf.MAFField = 'IP_ADDRESS'

    update sdf 
	set sdf.Value = t.Value
    from SubscriberDemographicFinal sdf 
    join @tmp t on sdf.SDFinalID = t.SDFinalID

END
go