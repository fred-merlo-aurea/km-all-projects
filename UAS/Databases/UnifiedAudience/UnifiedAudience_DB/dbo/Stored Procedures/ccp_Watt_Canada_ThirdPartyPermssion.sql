CREATE proc [dbo].[ccp_Watt_Canada_ThirdPartyPermssion]
@srcFile int = 0,
@ProcessCode varchar(50) = '',
@ClientId int = 0
as
begin
set nocount on

			update SubscriberFinal 
				set thirdpartypermission = 0 
			where (country = 'Canada' or email like '%.ca') and ProcessCode = @ProcessCode

end
