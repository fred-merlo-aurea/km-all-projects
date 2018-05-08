CREATE PROCEDURE [dbo].[ccp_Anthem_BeforeDQM]
@SourceFileID int,
@ClientID int = 3,
@processCode varchar(50) = '' 
AS
	BEGIN
	SET NOCOUNT ON;
	DECLARE @err_message VARCHAR(255)
	
	-- Update codes for EmailStatus
	--IF EXISTS(select * from SubscriberDemographicTransformed where MAFField in ('EMAILSTATUS'))
	--  BEGIN
	--	BEGIN TRY
	--		update sdt
	--		set value = CASE
	--			WHEN Value='A' THEN 'Active'
	--			WHEN Value='Y' THEN 'Active'
	--			WHEN Value='K' THEN 'UnSubscribe'
	--			WHEN Value='U' THEN 'UnSubscribe'
	--			ELSE Value
	--					END
	--		from SubscriberDemographicTransformed sdt
	--				inner join SubscriberTransformed st
	--					on sdt.SubscriberTransformedID = st.SubscriberTransformedID
	--		where st.ClientID=3 and sdt.MAFField = 'EMAILSTATUS'
	--	END TRY
	--	BEGIN CATCH
	--		set @err_message = ERROR_MESSAGE();
	--		RAISERROR(@err_message,12,1);
	--	END CATCH
	--  END
	--ELSE
	--  BEGIN
	--		set @err_message = 'REGION(EMAILSTATUS) column does not exist'
	--		RAISERROR(@err_message,11,1);
	--  END
	
	-- Add commas to multiple-occurrence field from our pub source to add commas, then trim
	--IF EXISTS(select * from SubscriberDemographicTransformed where MAFField in ('DEMO10'))
	--BEGIN
	--		update sdt
	--		set value = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Value, 'A','A,'),'B','B,'),'C','C,'),'D','D,'),'E','E,'),'F','F,'),'G','G,'),'H','H,'),'I','I,'),'J','J,'),'K','K,'),'L','L,'),'M','M,'),'N','N,'),'O','O,'),'P','P,'),'Q','Q,'),'R','R,'),'S','S,'),'T','T,'),'U','U,'),'V','V,'),'W','W,'),'X','X,'),'Y','Y,'),'Z','Z,')
	--		from SubscriberDemographicTransformed sdt
	--				inner join SubscriberTransformed st
	--					on sdt.STRecordIdentifier = st.STRecordIdentifier
	--		where st.ClientID=3 and sdt.MAFField = 'DEMO10'
			
	--		update sdt
	--		set value = case WHEN RIGHT(RTRIM(Value),1) = ',' THEN LEFT(Value,LEN(Value)-1) ELSE Value END
	--		from SubscriberDemographicTransformed sdt
	--				inner join SubscriberTransformed st
	--					on sdt.STRecordIdentifier = st.STRecordIdentifier
	--		where st.ClientID=3 and sdt.MAFField = 'DEMO10' and isnull(sdt.Value,'')!=''
	--END


	-- TODO: Split source KCM...xlsx?
	 
	-- update some qdates
	--BEGIN TRY
	--	UPDATE SubscriberTransformed set QDATE = '3/3/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=108  --KCM
	--	UPDATE SubscriberTransformed set QDATE = '2/25/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=109  --IMAG
		 	 
	--	UPDATE SubscriberTransformed set QDATE = '2/25/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=71  --CLP
	--	UPDATE SubscriberTransformed set QDATE = '2/11/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=97  --HEAR
	--	UPDATE SubscriberTransformed set QDATE = '2/25/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=98  --ORPR
	--	UPDATE SubscriberTransformed set QDATE = '2/11/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=99  --PSP
	--	UPDATE SubscriberTransformed set QDATE = '2/25/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=100 --PTP
	--	UPDATE SubscriberTransformed set QDATE = '2/25/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=102 --RHBM
	--	UPDATE SubscriberTransformed set QDATE = '2/11/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=111 --RTDM
	--	UPDATE SubscriberTransformed set QDATE = '2/25/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=104 --SLPR
	--	UPDATE SubscriberTransformed set QDATE = '2/25/2014'
	--	 where qdate<'1/1/1970' and clientID=3 and SourceFileID=105 --X247
	--END TRY
	--BEGIN CATCH
	--	set @err_message = ERROR_MESSAGE();
	--	RAISERROR(@err_message,12,1);
	--END CATCH
	
	-- drop records from Pubs that have 'Zero Date' as qdate
	
END