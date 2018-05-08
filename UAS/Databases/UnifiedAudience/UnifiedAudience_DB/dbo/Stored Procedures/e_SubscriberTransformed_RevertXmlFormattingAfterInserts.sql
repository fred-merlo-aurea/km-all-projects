create procedure e_SubscriberTransformed_RevertXmlFormattingAfterInserts
@ProcessCode varchar(50)
as
BEGIN

	SET NOCOUNT ON

	update SubscriberTransformed
	set PubCode = UAD_Lookup.dbo.RevertXmlFormatting(PubCode)
	where ProcessCode = @ProcessCode

	update SubscriberTransformed
	set FName = UAD_Lookup.dbo.RevertXmlFormatting(FName)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set LName = UAD_Lookup.dbo.RevertXmlFormatting(LName)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Title = UAD_Lookup.dbo.RevertXmlFormatting(Title)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Company = UAD_Lookup.dbo.RevertXmlFormatting(Company)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Address = UAD_Lookup.dbo.RevertXmlFormatting(Address)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set MailStop = UAD_Lookup.dbo.RevertXmlFormatting(MailStop)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set City = UAD_Lookup.dbo.RevertXmlFormatting(City)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set State = UAD_Lookup.dbo.RevertXmlFormatting(State)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Zip = UAD_Lookup.dbo.RevertXmlFormatting(Zip)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Plus4 = UAD_Lookup.dbo.RevertXmlFormatting(Plus4)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set ForZip = UAD_Lookup.dbo.RevertXmlFormatting(ForZip)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set County = UAD_Lookup.dbo.RevertXmlFormatting(County)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Country = UAD_Lookup.dbo.RevertXmlFormatting(Country)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Phone = UAD_Lookup.dbo.RevertXmlFormatting(Phone)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Fax = UAD_Lookup.dbo.RevertXmlFormatting(Fax)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Email = UAD_Lookup.dbo.RevertXmlFormatting(Email)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set RegCode = UAD_Lookup.dbo.RevertXmlFormatting(RegCode)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Verified = UAD_Lookup.dbo.RevertXmlFormatting(Verified)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set SubSrc = UAD_Lookup.dbo.RevertXmlFormatting(SubSrc)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set OrigsSrc = UAD_Lookup.dbo.RevertXmlFormatting(OrigsSrc)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Par3C = UAD_Lookup.dbo.RevertXmlFormatting(Par3C)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Source = UAD_Lookup.dbo.RevertXmlFormatting(Source)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Priority = UAD_Lookup.dbo.RevertXmlFormatting(Priority)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Sic = UAD_Lookup.dbo.RevertXmlFormatting(Sic)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set SicCode = UAD_Lookup.dbo.RevertXmlFormatting(SicCode)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Gender = UAD_Lookup.dbo.RevertXmlFormatting(Gender)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Address3 = UAD_Lookup.dbo.RevertXmlFormatting(Address3)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Home_Work_Address = UAD_Lookup.dbo.RevertXmlFormatting(Home_Work_Address)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Demo7 = UAD_Lookup.dbo.RevertXmlFormatting(Demo7)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Mobile = UAD_Lookup.dbo.RevertXmlFormatting(Mobile)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set LatLonMsg = UAD_Lookup.dbo.RevertXmlFormatting(LatLonMsg)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set AccountNumber = UAD_Lookup.dbo.RevertXmlFormatting(AccountNumber)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Occupation = UAD_Lookup.dbo.RevertXmlFormatting(Occupation)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set Website = UAD_Lookup.dbo.RevertXmlFormatting(Website)
	where ProcessCode = @ProcessCode
        
	update SubscriberTransformed
	set SubGenRenewalCode = UAD_Lookup.dbo.RevertXmlFormatting(SubGenRenewalCode)
	where ProcessCode = @ProcessCode

END
go