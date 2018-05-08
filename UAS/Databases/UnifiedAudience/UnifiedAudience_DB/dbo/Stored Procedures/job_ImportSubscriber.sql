CREATE proc[dbo].[job_ImportSubscriber]
(
    @XML Text
) 
AS   
BEGIN   
  
       SET NOCOUNT ON 
       
       declare @docHandle int

       EXEC sp_xml_preparedocument @docHandle OUTPUT, @XML

       print 'start - ' + convert(varchar, getdate(),108) 
		--set @XML = '<XML><SUBSCRIBER><SEQUENCE>100025</SEQUENCE><EMAIL>zallen1780@hotmail.com</EMAIL><FNAME>HAROLD</FNAME><LNAME>ZALLEN</LNAME><COMPANY>MALONE GROUP INTERNATIONAL</COMPANY>
		--<TITLE>PRESIDENT</TITLE><ADDRESS>1780 BROOKHAVEN CT</ADDRESS><MAILSTOP></MAILSTOP><CITY>AUBURN</CITY><STATE>AL</STATE><ZIP>36830</ZIP><PLUS4>7548</PLUS4><COUNTRY></COUNTRY><CTRY></CTRY>
		--<PHONE>3348872085</PHONE><FAX></FAX><FORZIP></FORZIP><DEMO7>A</DEMO7><DEMO31></DEMO31><DEMO32></DEMO32><DEMO33></DEMO33><DEMO34></DEMO34><DEMO35></DEMO35><DEMO36></DEMO36><CAT>10</CAT>
		--<XACT>10</XACT><XACTDATE>8/1/2011 12:00:00 AM</XACTDATE><QSOURCE>2</QSOURCE><QDATE>3/2/2011 12:00:00 AM</QDATE><SUBSRC>T1RB2</SUBSRC><PAR3C>1</PAR3C><EXPIRE></EXPIRE><COPIES>1</COPIES>
		--<BUSINESS>W17</BUSINESS><FUNCTION></FUNCTION><DEMO5></DEMO5></SUBSCRIBER></XML>' 

       INSERT INTO SubscriberTransformed 
              ([PubCode],[Sequence],[SORecordIdentifier],[STRecordIdentifier],[SourceFileID],[ProcessCode],[FNAME],[LNAME],[COMPANY],[TITLE],[ADDRESS],[MAILSTOP],[CITY],[STATE],[ZIP],
              [PLUS4],[COUNTY],[COUNTRY],[CountryID],[PHONE],[FAX],[MOBILE],[Par3C],
              [Email],[CategoryID],[TransactionID],[TransactionDate],[QSourceID],[QDate],[Subsrc],[OrigsSrc],[FORZIP],[COPIES],[Demo7],
              [MailPermission],[FaxPermission],[PhonePermission],[OtherProductsPermission],[ThirdPartyPermission],[EmailRenewPermission]
              )
       select PUBCODE, SEQUENCE, SORECORDIDENTIFIER, STRECORDIDENTIFIER, SOURCEFILEID, PROCESSCODE, FNAME, LNAME, COMPANY, TITLE, ADDRESS, MAILSTOP, CITY, STATE, ZIP, PLUS4, 
              COUNTY, COUNTRY, Country_ID, PHONE, FAX, [MOBILE], [PAR3C], [EMAIL],
              Category_ID, Transaction_ID, Transaction_Date, QSOURCE_ID, 
              QualificationDate, SUBSRC, PROMOSRC, FORZIP, 
              ISNULL(COPIES,1) AS COPIES,
              case when RTRIM(LTRIM(ISNULL(DEMO7,''))) not in ('A','B','C') then 'A' else UPPER(DEMO7) end as DEMO7,  --,'O'
              case when Demo31 is null then 1 when Demo31 ='Y' then 1 else 0 end as Demo31, 
              case when DEMO32 is null then 1 when DEMO32 ='Y' then 1 else 0 end as Demo32, 
              case when DEMO33 is null then 1 when DEMO33 ='Y' then 1 else 0 end as Demo33, 
              case when DEMO34 is null then NULL when DEMO34 ='Y' then 1 else 0 end as Demo34, 
              case when DEMO35 is null then 1 when DEMO35 ='Y' then 1 else 0 end as Demo35, 
              case when DEMO36 is null then 1 when DEMO36 ='Y' then 1 else 0 end as Demo36
       FROM OPENXML(@docHandle, N'/XML/SUBSCRIBER') 
              WITH 
              ( 
                 PUBCODE varchar(10) 'PUBCODE', SEQUENCE int 'SEQUENCE', SORECORDIDENTIFIER varchar(50) 'SORECORDIDENTIFIER', STRECORDIDENTIFIER varchar(50) 'STRECORDIDENTIFIER', 
                 PROCESSCODE varchar(100) 'PROCESSCODE', SOURCEFILEID int 'SOURCEFILEID', FNAME varchar(100) 'FNAME', LNAME varchar(100) 'LNAME', COMPANY varchar(100) 'COMPANY',
                 TITLE varchar(255) 'TITLE', ADDRESS varchar(100) 'ADDRESS', MAILSTOP varchar(50) 'MAILSTOP', CITY varchar(50) 'CITY', STATE varchar(50) 'STATE', 
                 ZIP varchar(10) 'ZIP', PLUS4 varchar(10) 'PLUS4', COUNTY varchar(50) 'COUNTY', COUNTRY varchar(50) 'COUNTRY', Country_ID int 'CTRY', 
                 PHONE varchar(50) 'PHONE', FAX varchar(50) 'FAX', MOBILE varchar(50) 'MOBILE', PAR3C varchar(50) 'PAR3C', EMAIL varchar(100) 'EMAIL',
                 Category_ID int 'CAT', Transaction_ID int 'XACT', Transaction_Date date 'XACTDATE', 
                 QSource_ID int 'QSOURCE', QualificationDate date 'QDATE', SUBSRC varchar(50) 'SUBSRC', PROMOSRC varchar(50) 'PROMOSRC', FORZIP varchar(50) 'FORZIP', 
                 COPIES int 'COPIES', EXPIRE date 'EXPIRE', DEMO7 char(1) 'DEMO7', DEMO31 char(1) 'DEMO31', 
                 DEMO32 char(1) 'DEMO32', DEMO33 char(1) 'DEMO33', DEMO34 char(1) 'DEMO34', DEMO35 char(1) 'DEMO35', DEMO36 char(1) 
              )       

       EXEC sp_xml_removedocument @docHandle 

       print '7 END - ' + convert(varchar, getdate(),108) 
END