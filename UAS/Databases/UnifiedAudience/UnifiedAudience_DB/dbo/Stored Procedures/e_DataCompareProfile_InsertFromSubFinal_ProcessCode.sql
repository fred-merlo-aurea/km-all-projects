CREATE PROCEDURE [dbo].[e_DataCompareProfile_InsertFromSubFinal_ProcessCode]
@processCode varchar(50)
as
	begin
		insert into DataCompareProfile (SubscriberFinalId,SFRecordIdentifier,SourceFileId,ProcessCode,ExternalKeyId,PubCode,FName,LName,Title,Occupation,Company,
										Address,MailStop,Address3,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,Mobile,Fax,Email,EmailStatusID,Gender,Website,
										DateCreated,ImportRowNumber,IGrp_No,IsNew)

		select SubscriberFinalId,SFRecordIdentifier,SourceFileId,ProcessCode,ExternalKeyId,PubCode,FName,LName,Title,Occupation,Company,
				Address,MailStop,Address3,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,Mobile,Fax,Email,EmailStatusID,Gender,Website,
				DateCreated,ImportRowNumber,IGrp_No,IsNewRecord
		from SubscriberFinal with(nolock)
		where ProcessCode = @processCode
	end
go
