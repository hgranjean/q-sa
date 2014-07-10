ALTER TABLE [dbo].[Events] ADD CONSTRAINT [FK_dbo.Events_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE

