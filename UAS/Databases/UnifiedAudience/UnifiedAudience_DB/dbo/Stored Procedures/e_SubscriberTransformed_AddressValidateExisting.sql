CREATE PROCEDURE [dbo].[e_SubscriberTransformed_AddressValidateExisting]
	@SourcefileId int,
	@ProcessCode varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		update st
		set [Address] = sf.[Address],
			City = sf.City,
			[State] = sf.[State],
			Zip = sf.Zip,
			plus4 = case when isnull(st.plus4,'')='' then sf.plus4 else st.Plus4 end,
			County = case when isnull(st.County,'')='' then sf.County else st.County end,
			Country = case when isnull(st.Country,'')='' then sf.Country else st.Country end,
			CountryID = case when isnull(st.CountryID,'')='' then sf.CountryID else st.CountryID end,
			Latitude = sf.Latitude,
			Longitude = sf.Longitude,
			IsLatLonValid = sf.IsLatLonValid,
			LatLonMsg = sf.LatLonMsg,
			Address3 = case when isnull(st.Address3,'')='' then sf.Address3 else st.Address3 end
		from SubscriberTransformed st with(nolock) 
			inner join SubscriberFinal sf with(nolock) on st.address = sf.address and st.City = sf.City and st.state = sf.state and left(st.Zip,5) = left(sf.Zip,5)									  
		where st.SourceFileID = @SourceFileID and st.ProcessCode = @ProcessCode
		and ISNULL(st.Address,'')!='' and ISNULL(st.City,'')!='' and ISNULL(st.State,'')!='' and ISNULL(st.Zip,'')!=''
		AND sf.IsLatLonValid = 'true'
END