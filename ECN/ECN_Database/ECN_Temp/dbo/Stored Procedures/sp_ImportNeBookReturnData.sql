CREATE PROCEDURE sp_ImportNeBookReturnData
@doc text
AS
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @doc
-- SELECT stmt using OPENXML rowset provider
INSERT INTO DataSet_NEW (RecordKey,STORENUMBER,BOOKSTORE,EMAILADDRESS,BOOK_TITLE,AUTHOR,ISBN,RENTALDATE,RETURNDATE,CARD_NO,PENALTY_AMOUNT,RETAILPRICE,
							CC_Charged,CC_Charged_Date,CC_Failed,CC_Failed_Date)							
SELECT *
FROM   OPENXML (@idoc, '/NewDataSet/newResults')
WITH (RecordKey varchar(255),
      StoreNumber varchar(255),
      BOOKSTORE varchar(255),
      EmailAddress varchar(255),
      BOOK_TITLE varchar(255),
      Author varchar(255),
      ISBN varchar(255),
      RentalDate date,
      ReturnDate date,
      CARD_NO varchar(255),
      PENALTY_AMOUNT varchar(255),
      RetailPrice varchar(255),
      CC_Charged bit,
      CC_Charged_Date date,
      CC_Failed bit,
      CC_Failed_Date date)