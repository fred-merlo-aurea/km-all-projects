-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[v_LandingPageAssign_GetParameters_BaseChannel]
@LPAID int,
@BaseChannelID int
AS
	DECLARE @LandingPageName varchar(50)
	
	SELECT @LandingPageName = Name
	FROM
		LandingPageAssign lpa WITH (NOLOCK)
		JOIN LandingPage lp WITH (NOLOCK) ON lpa.LPID = lp.LPID
	WHERE
		lpa.LPAID = @LPAID
		
	--Unsubscribe(returns BlastID, GroupID, CustomerID and EmailAddress
	IF UPPER(LTRIM(RTRIM(@LandingPageName))) = 'UNSUBSCRIBE'
	BEGIN
		SELECT top 1 b.BlastID, b.GroupID, b.CustomerID, e.EmailAddress
		FROM
			ecn5_communicator..Blast b WITH (NOLOCK)
			JOIN ecn_Activity..BlastActivitySends bas WITH (NOLOCK) ON b.BlastID = bas.BlastID
			JOIN ECN5_COMMUNICATOR..EmailGroups eg WITH(NOLOCK) on b.GroupID = eg.GroupID and bas.EmailID = eg.EmailID
			JOIN ecn5_communicator..Emails e WITH (NOLOCK) ON bas.EmailID = e.EmailID

		WHERE
			b.CustomerID in (SELECT CustomerID from Customer c with(nolock) where c.BaseChannelID = @BaseChannelID and c.IsDeleted = 0) AND
			b.StatusCode = 'sent' AND
			b.BlastType = 'html' AND
			b.TestBlast = 'n'
		ORDER BY
			b.SendTime DESC
	END
	ELSE IF	UPPER(@LandingPageName) = 'FORWARD'
	BEGIN
		SELECT top 1 b.BlastID, bas.EmailID
		FROM
			ecn5_communicator..Blast b WITH (NOLOCK)
			JOIN ecn_Activity..BlastActivitySends bas WITH (NOLOCK) ON b.BlastID = bas.BlastID
			JOIN ECN5_COMMUNICATOR..EmailGroups eg WITH(NOLOCK) on b.GroupID = eg.GroupID and bas.EmailID = eg.EmailID
		WHERE
			b.CustomerID in (SELECT CustomerID from Customer c with(nolock) where c.BaseChannelID = @BaseChannelID and c.IsDeleted = 0) AND
			b.StatusCode = 'sent' AND
			b.BlastType = 'html' AND
			b.TestBlast = 'n'
		ORDER BY
			b.SendTime DESC
	END
	ELSE IF UPPER(@LandingPageName) = 'ABUSE'
	BEGIN
		SELECT top 1 convert(varchar(100),e.EmailAddress) + ',' + convert(varchar(100),e.EmailID) + ',' + convert(varchar(100),b.GroupID) + ',' + convert(varchar(100),b.CustomerID) + ',' + convert(varchar(100),b.BlastID)
		FROM 
			ECN5_COMMUNICATOR..Blast b with(nolock)
			JOIN ecn_activity..BlastActivitySends bas with(nolock) on b.BlastID = bas.BlastID
			JOIN ECN5_COMMUNICATOR..EmailGroups eg WITH(NOLOCK) on b.GroupID = eg.GroupID and bas.EmailID = eg.EmailID
			JOIN ECN5_COMMUNICATOR..Emails e with(nolock) on bas.EmailID = e.EmailID
		WHERE
			b.CustomerID in (SELECT CustomerID from Customer c with(nolock) where c.BaseChannelID = @BaseChannelID and c.IsDeleted = 0) AND
			b.StatusCode = 'sent' AND
			b.BlastType = 'html' AND
			b.TestBlast = 'n'
		ORDER BY 
			b.SendTime DESC
	
	
	END