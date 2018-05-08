CREATE PROCEDURE [dbo].[SelectEmailDatafieldSet] (
	@emailID int, -- NOT NULL
	@datafieldSetID int -- NOT NULL
) AS
SET NOCOUNT ON
-- The first table returned is a list of UDF fields that belong to the specified DatafieldSet.
SELECT
	GroupDatafieldsID,
	ShortName,
	LongName,
	SurveyID
FROM dbo.GroupDatafields
WHERE DatafieldSetID = @datafieldSetID
-- The second table returned is the actual values for the UDF fields listed in the first table.
SELECT
	EDV.EmailDataValuesID,
	EDV.EmailID,
	EDV.GroupDatafieldsID,
	EDV.DataValue,
	EDV.ModifiedDate,
	EDV.SurveyGridID,
	EDV.EntryID,
	GDF.GroupDatafieldsID,
	GDF.ShortName
FROM dbo.EmailDataValues AS EDV
JOIN dbo.GroupDatafields AS GDF ON EDV.GroupDatafieldsID = GDF.GroupDatafieldsID
WHERE EDV.EmailID = @emailID AND
	GDF.DatafieldSetID = @datafieldSetID AND
	EDV.EntryID IS NOT NULL -- Just an extra check for safety, this should never be NULL for rows that meet the other criteria.
ORDER BY EDV.EntryID
RETURN 0
