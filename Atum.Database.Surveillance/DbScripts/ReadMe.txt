How to create a DB migration:


Add-Migration DbMigrationB
Update-Database -Script -SourceMigration: DbMigrationA -TargetMigration: DbMigrationB