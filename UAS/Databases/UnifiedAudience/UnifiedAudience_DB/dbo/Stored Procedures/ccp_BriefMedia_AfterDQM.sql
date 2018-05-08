CREATE PROCEDURE [dbo].[ccp_BriefMedia_AfterDQM]
	--@param1 int = 0,
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @err_message VARCHAR(255)

	-- Update [country_of_licensure] with countries that are in [state_of_licensure]
	IF EXISTS(select * from SubscriberDemographicTransformed sdt with(nolock) inner join SubscriberTransformed st with(nolock) on sdt.STRecordIdentifier = st.STRecordIdentifier where MAFFIeld in ('country_of_licensure'))
	BEGIN TRY
		UPDATE t1
		set value = t2.value
					from SubscriberDemographicTransformed t1 with(nolock)
						inner join (select SORecordIdentifier,value from SubscriberDemographicTransformed where MAFField = 'state_of_Licensure') t2
							on t1.SORecordIdentifier = t2.SORecordIdentifier
						inner join (select SORecordIdentifier,Country from SubscriberTransformed) t3
							on t1.SORecordIdentifier = t3.SORecordIdentifier
					where ISNULL(t3.[Country],'') not in ('') and (t3.[Country] not like 'United State%' or t3.[Country] not like 'U.S%') and MAFFIELD = 'country_of_licensure'

	END TRY
	BEGIN CATCH
		set @err_message = ERROR_MESSAGE();
		PRINT(@err_message);
	END CATCH
END