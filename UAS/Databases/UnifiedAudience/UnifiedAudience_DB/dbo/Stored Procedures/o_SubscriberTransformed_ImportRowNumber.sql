CREATE PROCEDURE [dbo].[o_SubscriberTransformed_ImportRowNumber]
	@ProcessCode varchar(50)
AS
begin
	set nocount on
	Select st.ImportRowNumber as 'SubscriberImportRowNumber' from SubscriberDemographicTransformed sdt WITH(NOLOCK)   
		join SubscriberTransformed st WITH(NOLOCK) on sdt.STRecordIdentifier = st.STRecordIdentifier  
		where st.ProcessCode = @ProcessCode and sdt.NotExists = 1
end