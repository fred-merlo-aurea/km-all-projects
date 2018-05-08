CREATE PROCEDURE [dbo].[ccp_TenMissions_AfterDQM]
@SourceFileID int,
@ProcessCode varchar(50) = '',
@ClientId int = 1
AS
BEGIN

	set nocount on

	DECLARE @err_message VARCHAR(255)

	-- Remove commas in demographic fields
	--update SubscriberDemographicTransformed
	--set Value = REPLACE(value,',','')
	--from SubscriberTransformed st
	--where MAFField in ('A2177_4','A2241_33','ShopSize') 
	--	and st.ClientID = 1


	
	IF exists(select * from SubscriberDemographicTransformed sdt inner join SubscriberTransformed st on sdt.STRecordIdentifier = st.STRecordIdentifier where MAFFIeld in ('DEMO90'))
		begin
			begin try

				-- Replace ADMSblank
				update sdt
				set value = null 
				from SubscriberDemographicTransformed sdt
						inner join SubscriberTransformed st
							on sdt.STRecordIdentifier = st.STRecordIdentifier
				where sdt.MAFField = 'DEMO90'

				-- Update field VEHICLETYPESERVICE(DEMO90) to 1 or 2 depending on company name
				update sdt
				set Value = case when COMPANY like '%IMPORT%' Or COMPANY like '%IMPORTS%' OR COMPANY like '%AUDI%' or COMPANY like '%BMW%'
									  OR COMPANY like '%JAGUAR%' OR Company like '%LAND ROVER%' OR Company like '%MERCEDES-BENZ%' OR Company like '%MERCEDES BENZ%'
									  OR COMPANY like '%PORSCHE%' OR COMPANY like '%ROLLS ROYCE%' or Company like '%SAAB%' or company like '%Smart Car%' or company like '%Volvo%'
									  or company like '%honda%' or company like '%Toyota%' or Company like '%Nissan%' or Company like '%Volkswagen%'
									  or company like '%Mitsubishi%' or company like '%Hyundai%' or company like '%Mazda%' or company like '%Suzuki%' or company like '%Subaru%'
									  or company like '%Isuzu%' or company like '%Fiat%' or company like '%Kia%' then 2
								 when company like '%Dodge%' OR company like '%Chrysler%' or Company like '%Saturn%' or company like '%Ford%' or Company like '%Chevrolet%'
									or company like '%Jeep%' or company like '%GMC%' or company like '%Hummer%' or company like '%Pontiac%' or company like '%Oldsmobile%'
									or company like '%Lincoln%' or company like '%Eagle%' or company like '%Buick%' or company like '%Cadillac%' or company like '%Mercury%' 
									or company like '%Plymouth%' or company like '%DOMESTIC%' then 1 else Value end
				from SubscriberDemographicTransformed sdt
					inner join SubscriberTransformed st
						on sdt.STRecordIdentifier = st.STRecordIdentifier
				where sdt.MAFField = 'DEMO90'

			end try
			begin catch

				set @err_message = ERROR_MESSAGE();
				RAISERROR(@err_message,12,1);

			end catch
		end 

END