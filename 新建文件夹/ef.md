> 避免迁移

有时候迁移由主系统来实现，子系统使用主系统的库的时候，要避免迁移

使用`ExcludeFromMigrations`
```csharp
builder.Entity<SYS_INST_CENTER>()
    .ToTable("SYS_INST_CENTER", t => t.ExcludeFromMigrations());
```

