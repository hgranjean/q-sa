UPDATE [dbo].[AspNetRoles]
SET [Name] = 'Manager' WHERE [Id] = 2
INSERT INTO [dbo].[AspNetRoles]
([Id], [Name]) VALUES (3, 'Team Member')