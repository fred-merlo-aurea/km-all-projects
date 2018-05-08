CREATE PROCEDURE [dbo].[spClickActivity] (
	@BlastID int,
	@HowMuch varchar (10)
)
as
Begin
/* 
	HAD TO MODIFY THE BELOW SPROC 'COS OF THE PDF REPORTING FOR CANON COMMUNICATIONS. 
	THE MODIFIED ONE BELOW WILL CREATE ANOTHER COLUMN 'SmallActionValue' WHICH WILL CHECK FOR THE PDF LINK STARTING WITH  'http://www.kmpsgroup.com/pdf/' 
	IF YES THEN IT WILL STRIP THE REST OF THE URL AFTER 'pdf/' & SAVE IT
	IT WILL CHECK IF ITS CANNON CUSTOMER (1209) 
	IF YES IT WILL USE THE NEW COLUMN 'SmallActionValue' FOR CLICK PROCESSING 
	ELSE THE OLD PROCESS CONTUNUES 
	- Ashok 02/23/2006
*/
/*	DECLARE @ClickActivityLogs TABLE (
	  	NewActionValue Varchar(800)
	)
	
	insert into @ClickActivityLogs
	SELECT NewActionValue = 
	case 
	when CHARINDEX('eid',ActionValue) = 0 then ActionValue
	else SUBSTRING(ActionValue,1,CHARINDEX('eid',ActionValue)-2)
	END
	FROM EmailActivityLog WHERE ActionTypeCode='click' AND BlastID=@BlastID
	select top 10 Count(NewActionValue) as ClickCount, NewActionValue, 
	CASE WHEN LEN(NewActionValue) > 6 THEN LEFT(RIGHT(NewActionValue,LEN(NewActionValue)-7),65) ELSE NewActionValue END AS SmallLink
	from @ClickActivityLogs
	group by NewActionValue  order by ClickCount desc
*/

	declare	@custID int

	select @custID = customerID from ecn5_communicator..[BLAST] where blastID = @BlastID

	if @custID = 1209
	begin
		exec ('select '+@HowMuch+' Count(SmallActionValue) as ClickCount, SmallActionValue as NewActionValue, ' +
			' CASE WHEN LEN(SmallActionValue) > 6 THEN LEFT(SmallActionValue,65) ELSE SmallActionValue END AS SmallLink ' +
			' from (' +
					'SELECT Case when CHARINDEX(''eid'',URL) = 0 then URL else SUBSTRING(URL,1,CHARINDEX(''eid'',URL)-2) END as ''NewActionValue'', ' +
							' CASE ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0306/pdf.html%'' THEN ''http://www.kmpsgroup.com/pdf/0306/pdf.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0406/pdf.html%'' THEN ''http://www.kmpsgroup.com/pdf/0406/pdf.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0506/pdf.html%'' THEN ''http://www.kmpsgroup.com/pdf/0506/pdf.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0306/pdf_out.html%'' THEN ''http://www.kmpsgroup.com/pdf/0306/pdf_out.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0406/pdf_out.html%'' THEN ''http://www.kmpsgroup.com/pdf/0406/pdf_out.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0506/pdf_out.html%'' THEN ''http://www.kmpsgroup.com/pdf/0506/pdf_out.html'' ' +
							' ELSE URL END as ''SmallActionValue'' ' +
					' FROM BlastActivityClicks ' +
					' WHERE BlastID=' + @BlastID + 
				' ) inn group by SmallActionValue order by ClickCount desc')
	end 
	else 
	begin
		exec ('select '+@HowMuch+' Count(SmallActionValue) as ClickCount,  NewActionValue, ' +
		' CASE WHEN LEN(NewActionValue) > 6 THEN LEFT(NewActionValue,65) ELSE NewActionValue END AS SmallLink ' +
		' from (' +
					'SELECT Case when CHARINDEX(''eid'',URL) = 0 then URL else SUBSTRING(URL,1,CHARINDEX(''eid'',URL)-2) END as ''NewActionValue'', ' +
							' CASE ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0306/pdf.html%'' THEN ''http://www.kmpsgroup.com/pdf/0306/pdf.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0406/pdf.html%'' THEN ''http://www.kmpsgroup.com/pdf/0406/pdf.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0506/pdf.html%'' THEN ''http://www.kmpsgroup.com/pdf/0506/pdf.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0306/pdf_out.html%'' THEN ''http://www.kmpsgroup.com/pdf/0306/pdf_out.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0406/pdf_out.html%'' THEN ''http://www.kmpsgroup.com/pdf/0406/pdf_out.html'' ' +
							' WHEN URL like ''http://www.kmpsgroup.com/pdf/0506/pdf_out.html%'' THEN ''http://www.kmpsgroup.com/pdf/0506/pdf_out.html'' ' +
							' ELSE URL END as ''SmallActionValue'' ' +
					' FROM BlastActivityClicks ' +
					' WHERE BlastID=' + @BlastID + 
				' ) inn group by NewActionValue  order by ClickCount desc ')
	end

	
END
