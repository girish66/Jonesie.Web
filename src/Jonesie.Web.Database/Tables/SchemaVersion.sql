CREATE TABLE [dbo].SchemaVersion
(
  [SchemaSetName] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Version] INT NOT NULL, 
    [LastUpdated] DATETIMEOFFSET NOT NULL
)
