CREATE TABLE [dbo].[WaveMailing]
(
	[WaveMailingID]          INT            IDENTITY (1, 1) NOT NULL, 
    [IssueID] INT NOT NULL, 
    [WaveMailingName] VARCHAR(100) NOT NULL, 
    [PublicationID] INT NOT NULL, 
	[WaveNumber] INT NOT NULL,
	[DateSubmittedToPrinter] DATETIME NULL,
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NULL, 
	[SubmittedToPrinterByUserID] INT NULL,
	[CreatedByUserID] INT NOT NULL, 
    [UpdatedByUserID] INT NULL,
)
