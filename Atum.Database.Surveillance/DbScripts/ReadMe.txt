Add-Migration DbMigrationB
Update-Database -Script -SourceMigration: DbMigrationA -TargetMigration: DbMigrationB