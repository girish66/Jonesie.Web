CREATE TABLE [dbo].[SiteNavigation]
(
    [SiteNavigationId] int identity NOT NULL PRIMARY KEY, 
    [MenuName] NVARCHAR(50) NOT NULL, 
    [ChildMenuName] NVARCHAR(50) NULL, 
    [DisplayLabel] NVARCHAR(50) NOT NULL, 
    [Controller] NVARCHAR(50) NULL, 
    [Action] NVARCHAR(50) NULL, 
    [Url] NVARCHAR(350) NULL, 
    [OptionOrder] INT NOT NULL DEFAULT 0, 
    [Active] BIT NOT NULL DEFAULT 1,
    [Roles] NVARCHAR(MAX) NULL, 
    [row_version] ROWVERSION NOT NULL
)
