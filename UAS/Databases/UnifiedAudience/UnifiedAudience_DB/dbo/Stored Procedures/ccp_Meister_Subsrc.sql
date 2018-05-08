create procedure ccp_Meister_Subsrc
@srcFile int = 0,
@ProcessCode varchar(50) = '',
@ClientId int =0
as
BEGIN

	set nocount on

	update sf 
	set SubSrc = STUFF(
         (SELECT ', ' +  sf2.SubSrc
          FROM SubscriberFinal sf2
          where sf.IGrp_No = sf2.IGrp_No
          and ProcessCode = @ProcessCode
          group by sf2.IGrp_No,sf2.SubSrc
          FOR XML PATH (''))
          , 1, 1, '') 
    from SubscriberFinal sf 
    where ProcessCode = @ProcessCode

END
go