CREATE proc[dbo].[job_ImportSubscriberDemographics]
(
    @XML Text
) 
AS   
BEGIN

	SET NOCOUNT ON
       
       declare @docHandle int

       EXEC sp_xml_preparedocument @docHandle OUTPUT, @XML

       INSERT INTO SubscriberDemographicTransformed 
              (PubID,SORecordIdentifier,STRecordIdentifier,MAFField,Value,NotExists,NotExistReason,DateCreated,CreatedByUserID,DemographicUpdateCodeId,IsAdhoc,
              ResponseOther
              )
       select 
              PUBID, SORECORDIDENTIFIER, STRECORDIDENTIFIER, MAFFIELD, VALUE, NOTEXISTS, NOTEXISTREASON, DATECREATED, CREATEDBYUSERID, DEMOGRAPHICUPDATECODEID, 
              ISADHOC, RESPONSEOTHER
       FROM OPENXML(@docHandle, N'/XML/SUBSCRIBER') 
              WITH 
              ( 
                 PUBID int 'PUBID', SORECORDIDENTIFIER varchar(50) 'SORECORDIDENTIFIER', STRECORDIDENTIFIER varchar(50) 'STRECORDIDENTIFIER', 
                 MAFFIELD varchar(255) 'MAFFIELD', VALUE varchar(5) 'VALUE', NOTEXISTS bit 'NOTEXISTS', NOTEXISTREASON varchar(50) 'NOTEXISTREASON',
                 DATECREATED datetime 'DATECREATED', CREATEDBYUSERID int 'CREATEDBYUSERID', DEMOGRAPHICUPDATECODEID int 'DEMOGRAPHICUPDATECODEID',
                 ISADHOC bit 'ISADHOC', RESPONSEOTHER varchar(256) 'RESPONSEOTHER'
              )       

       EXEC sp_xml_removedocument @docHandle 
END