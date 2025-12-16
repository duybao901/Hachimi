-- User
SELECT * FROM [HachimiDatabase].[dbo].[AppUsers]
SELECT * FROM [HachimiDatabase].[dbo].[AppUserRoles]
SELECT * FROM [HachimiDatabase].[dbo].[AppRoles]
SELECT * FROM [HachimiDatabase].[dbo].UserProfiles

-- Clear all users
DELETE FROM [HachimiDatabase].[dbo].[AppUserRoles]
DELETE FROM [HachimiDatabase].[dbo].AppUsers
DELETE FROM [HachimiDatabase].[dbo].UserProfiles


