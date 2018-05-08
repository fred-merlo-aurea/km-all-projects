CREATE TABLE [dbo].[Pharmalive_EarnedDeferred] (
    [groupID]                INT             NOT NULL,
    [Month]                  INT             NOT NULL,
    [Year]                   INT             NOT NULL,
    [PreviousDeferredIncome] DECIMAL (10, 2) NOT NULL,
    [NewAdds]                DECIMAL (10, 2) NOT NULL,
    [Renewals]               DECIMAL (10, 2) NOT NULL,
    [EarnedIncome]           DECIMAL (10, 2) NOT NULL,
    [Adjustments]            DECIMAL (10, 2) NOT NULL,
    [CurrentDeferredIncome]  DECIMAL (10, 2) NOT NULL
);

