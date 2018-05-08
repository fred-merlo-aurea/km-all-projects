CREATE TABLE [dbo].[Wizard_BaseFee] (
    [BaseFeeID]     INT      IDENTITY (1, 1) NOT NULL,
    [BaseChannelID] INT      NOT NULL,
    [ChargeCrCard]  CHAR (1) NULL,
    [DetailReports] CHAR (1) NULL,
    CONSTRAINT [PK_Wizard_BaseFee] PRIMARY KEY CLUSTERED ([BaseFeeID] ASC) WITH (FILLFACTOR = 80)
);

