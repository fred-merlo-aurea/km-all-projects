CREATE PROCEDURE [dbo].[job_CountryRegionCleanse]
@ProcessCode varchar(50),
@SourceFileID int
AS
BEGIN   

	SET NOCOUNT ON 

	declare @USCountryID int

	select @USCountryID = CountryID 
	From UAD_Lookup..Country 
	Where ShortName = 'United States'

	declare @CanadaCountryID int
	select @CanadaCountryID = CountryID From UAD_Lookup..Country Where ShortName = 'Canada'
	declare @MexicoCountryID int
	select @MexicoCountryID = CountryID From UAD_Lookup..Country Where ShortName = 'Mexico'

	--Moved region code assignment to end of process to allow for an additional match on country

	--Removed per Sunil do not assign country using region
	--UPDATE st
	--SET	Country= c.ShortName,
	--	CountryID = r.CountryID,
	--	DateUpdated = GETDATE(),
	--	UpdatedByUserID = 1
	--FROM SubscriberTransformed st
	--	join UAD_Lookup..Region r on st.State = r.RegionCode
	--	join UAD_Lookup..Country c on c.CountryID = r.CountryID
	--WHERE ProcessCode = @ProcessCode and isnull(Country,'') = '' and r.CountryID in (@USCountryID, @CanadaCountryID, @MexicoCountryID)

	UPDATE st
	SET Country=c.ShortName,
		CountryID = c.CountryID,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st	
		join UAD_Lookup..CountryMap cm on cm.CountryDirty = RTRIM(LTRIM(st.Country))
		join UAD_Lookup..Country c on cm.CountryID = c.CountryID
	WHERE RTRIM(LTRIM(st.Country)) = cm.CountryDirty
		AND ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

	------------------start added by JW 5/3/2016 to support FullName, ShortName, Alpha3, Alpha2
	------There are 19 instances of FullName, Alpha2, Alpha3 not being unique - mainly for United Kingdom 
	------code does not break but will grab first instance

	UPDATE st
	SET Country=c.ShortName,
		CountryID = c.CountryID,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st	
		join UAD_Lookup..Country c on c.FullName = RTRIM(LTRIM(st.Country))
	WHERE c.FullName is not null
		and ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

	UPDATE st
	SET Country=c.ShortName,
		CountryID = c.CountryID,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st	
		join UAD_Lookup..Country c on c.ShortName = RTRIM(LTRIM(st.Country))
	WHERE c.ShortName is not null 
		and ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

	UPDATE st
	SET Country=c.ShortName,
		CountryID = c.CountryID,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st	
		join UAD_Lookup..Country c on c.Alpha2 = RTRIM(LTRIM(st.Country))
	WHERE c.Alpha2 is not null
		and ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

	UPDATE st
	SET Country=c.ShortName,
		CountryID = c.CountryID,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st	
		join UAD_Lookup..Country c on c.Alpha3 = RTRIM(LTRIM(st.Country))
	WHERE c.Alpha3 is not null
		and ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID
	-----------------------------------------------------------------------end JW update

	UPDATE st
	SET	CountryID = cm.CountryID,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st
		join UAD_Lookup..CountryMap cm on RTRIM(LTRIM(st.Country)) = cm.CountryDirty COLLATE SQL_Latin1_General_CP1_CI_AS
	WHERE ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

	UPDATE st
	SET	State = r.RegionCode,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st
		join UAD_Lookup..RegionMap rm on st.State = rm.RegionDirty COLLATE SQL_Latin1_General_CP1_CI_AS
		join UAD_Lookup..Region r on rm.RegionID = r.RegionID
	WHERE ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID and r.countryid = st.countryid
	
	UPDATE st
	SET	Zip = Zip + ' ' + Plus4,
		Plus4 = '',
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM SubscriberTransformed st
		join UAD_Lookup..Country c on RTRIM(LTRIM(st.Country)) = c.ShortName COLLATE SQL_Latin1_General_CP1_CI_AS
	WHERE c.CountryID = @CanadaCountryID AND LEN(Plus4) > 0 AND LEN(Zip) > 0
		AND ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID

	UPDATE SubscriberTransformed
	SET State = 'FO',
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	WHERE ProcessCode = @ProcessCode AND SourceFileID = @SourceFileID
		AND ISNULL(CountryID,0) > 0 and CountryID not in (@USCountryID, @CanadaCountryID, @MexicoCountryID)

	--UPDATE SubscriberTransformed
	--SET IsMailable = 1
	--WHERE (LEN(Address) > 0 and LEN(City) > 0 and LEN(State) > 0 and LEN(Zip) > 0)
	--AND ProcessCode = @ProcessCode

END
go