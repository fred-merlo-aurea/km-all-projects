CREATE TABLE [dbo].[LayoutPlanSettings] (
    [SettingID]         INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]        INT           NOT NULL,
    [ScheduleXmlString] VARCHAR (500) NOT NULL,
    [ModifiedUserID]    INT           NOT NULL,
    [ModifiedDate]      INT           NOT NULL,
    CONSTRAINT [PK_LayoutPlanSettings] PRIMARY KEY CLUSTERED ([SettingID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[LayoutPlanSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[LayoutPlanSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LayoutPlanSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[LayoutPlanSettings] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LayoutPlanSettings] TO [reader]
    AS [dbo];

