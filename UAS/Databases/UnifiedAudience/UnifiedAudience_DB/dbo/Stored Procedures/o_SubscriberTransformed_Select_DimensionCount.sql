CREATE PROCEDURE [dbo].[o_SubscriberTransformed_Select_DimensionCount]
@processCode varchar(50)
as
	begin
		set nocount on
		--Transformed
		select
		(Select COUNT(sdt.STRecordIdentifier) from SubscriberDemographicTransformed sdt WITH(NOLOCK) 
		join SubscriberTransformed st WITH(NOLOCK) on sdt.STRecordIdentifier = st.STRecordIdentifier
		where st.ProcessCode = @processCode and sdt.NotExists = 1) as 'DimensionErrorTotal',
		(Select COUNT(distinct sdt.STRecordIdentifier) from SubscriberDemographicTransformed sdt WITH(NOLOCK) 
		join SubscriberTransformed st WITH(NOLOCK) on sdt.STRecordIdentifier = st.STRecordIdentifier
		where st.ProcessCode = @processCode and sdt.NotExists = 1) as 'DimensionDistinctSubscriberCount'	
	end
go
