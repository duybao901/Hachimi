SELECT TOP (1000) [Id]
      ,[Title]
      ,[Content]
      ,[CreatedOnUtc]
      ,[ModifiedOnUtc]
      ,[IsDelete]
  FROM [HachimiDatabase].[dbo].[Post]
SELECT TOP (1000) [Id]
      ,[Type]
      ,[Content]
      ,[OccurredOnUtc]
      ,[ProcessedOnUtc]
      ,[Error]
  FROM [HachimiDatabase].[dbo].[OutboxMessages]
  
TRUNCATE TABLE [HachimiDatabase].[dbo].[OutboxMessages]
Delete From [HachimiDatabase].[dbo].[Post]

UPDATE [HachimiDatabase].[dbo].[OutboxMessages] set ProcessedOnUtc = null where Id = '3F078B28-2555-4121-9D03-FC526C95DCB0'

-- Seed Tags
 INSERT INTO Tags (Id, Name, Slug, Color, IsDelete,CreatedOnUtc)
VALUES
('8f5d6f9e-861d-4d3c-a11d-1e4e556cb500', 'CSharp', 'csharp', '#2396F3',0, SYSDATETIMEOFFSET()),
('2ab2d841-80dd-4ce3-bb18-368a51ac3c01', 'DotNet', 'dotnet', '#512BD4',0, SYSDATETIMEOFFSET()),
('f04b26a2-3ef6-46f9-9f65-d13bacd7e09e', 'JavaScript', 'javascript', '#F7DF1E',0, SYSDATETIMEOFFSET()),
('9a66b8fc-60fb-4a7d-9dba-f6011a179e45', 'TypeScript', 'typescript', '#3178C6',0, SYSDATETIMEOFFSET());

-- Seed Role
IF NOT EXISTS (SELECT 1 FROM [HachimiDatabase].[dbo].AppRoles  WHERE NormalizedName = 'AUTHOR')
BEGIN
    INSERT INTO [HachimiDatabase].[dbo].AppRoles (Id, Name, NormalizedName, ConcurrencyStamp, Description, RoleCode)
    VALUES (NEWID(), 'Author', 'AUTHOR', NEWID(), 'Default author role', 'AUTHOR');
END

IF NOT EXISTS (SELECT 1 FROM [HachimiDatabase].[dbo].AppRoles WHERE NormalizedName = 'ADMIN')
BEGIN
    INSERT INTO [HachimiDatabase].[dbo].AppRoles (Id, Name, NormalizedName, ConcurrencyStamp, Description, RoleCode)
    VALUES (NEWID(), 'Admin', 'ADMIN', NEWID(), 'Administrator role', 'ADMIN');
END



-- Tag
SELECT TOP (1000) [Id]
      ,[Name]
      ,[Slug]
      ,[Color]
      ,[CreatedOnUtc]
      ,[ModifiedOnUtc]
      ,[IsDelete]
FROM [HachimiDatabase].[dbo].[Tags]


